const geo2svg = require("geo2svg");
const shp = require("shpjs");
const fs = require("fs");
const utm = require("utm");

//geojson lng, lat
//bbox is as geojson standard: [lng_min, lat_min, lng_max, lat_max]
let getBBoxFromGeometry = (geometry) => {
    let bbox = geometry.bbox;
    if(bbox == undefined) {
        bbox = [];
        let outerRingCoords;
        if(geometry.type == "Polygon") {
            //outer ring is first element in coordinates array
            outerRingCoords = geometry.coordinates[0];
        }
        else if(geometry.type == "MultiPolygon") {
            let outerRings = [];
            //get outer rings from each 
            for(let i = 0; i < geometry.coordinates.length; i++) {
                outerRings.push(geometry.coordinates[i][0]);
                outerRingCoords = outerRings.flat();
            }
        }
        else {
            throw new Error("Only Polygon and MultiPolygon coordinate standards implemented");
        }
        let latRange = {
            min: Number.POSITIVE_INFINITY,
            max: Number.NEGATIVE_INFINITY
        };
        let lngRange = {
            min: Number.POSITIVE_INFINITY,
            max: Number.NEGATIVE_INFINITY
        };
        for(let i = 0; i < outerRingCoords.length; i++) {
            let lng = outerRingCoords[i][0];
            let lat = outerRingCoords[i][1];
            if(lat < latRange.min) {
                latRange.min = lat;
            }
            if(lat > latRange.max) {
                latRange.max = lat;
            }
            if(lng < lngRange.min) {
                lngRange.min = lng;
            }
            if(lng > lngRange.max) {
                lngRange.max = lng;
            }
        }
        bbox = [lngRange.min, latRange.min, lngRange.max, latRange.max];
    }
    return bbox;
};


let getOuterBBox = (bboxes) => {
    let latRange = {
        min: Number.POSITIVE_INFINITY,
        max: Number.NEGATIVE_INFINITY
    };
    let lngRange = {
        min: Number.POSITIVE_INFINITY,
        max: Number.NEGATIVE_INFINITY
    };
    for(let i = 0; i < bboxes.length; i++) {
        let lat = {
            min: bboxes[i][1],
            max: bboxes[i][3]
        };
        let lng = {
            min: bboxes[i][0],
            max: bboxes[i][2]
        };
        if(lat.min < latRange.min) {
            latRange.min = lat.min;
        }
        if(lat.max > latRange.max) {
            latRange.max = lat.max;
        }
        if(lng.min < lngRange.min) {
            lngRange.min = lng.min;
        }
        if(lng.max > lngRange.max) {
            lngRange.max = lng.max;
        }
    }
    return [lngRange.min, latRange.min, lngRange.max, latRange.max];
};

let getGroupBBox = (features) => {
    bboxes = [];
    for(let i = 0; i < features.length; i++) {
        bboxes.push(getBBoxFromGeometry(features[i].geometry));
    }
    return getOuterBBox(bboxes);
};

let convertBBoxesToMeters = (bboxes) => {
    let convertedBBoxes = [];
    for(let i = 0; i < bboxes.length; i++) {
        let bbox = bboxes[i];
        //utm package takes lat lng order
        let ll = [bbox[1], bbox[0]];
        let ur = [bbox[3], bbox[2]];
        let mins = utm.fromLatLon(...ll);
        let maxs = utm.fromLatLon(...ur);
        convertedBBoxes.push([mins.easting, mins.northing, maxs.easting, maxs.northing]);
    }
    
    return convertedBBoxes
};

let offSetBBoxes = (offsetX, offsetY, bboxes) => {
    for(let i = 0; i < bboxes.length; i++) {
        bboxes[i][0] -= offsetX;
        bboxes[i][1] -= offsetY;
        bboxes[i][2] -= offsetX;
        bboxes[i][3] -= offsetY;
    }
};

//need to keep a reference file with meter based bounding boxes offset by lower left corner of group


fs.readFile("./testNOAA.zip", (e, data) => {
    if(e) {
        console.log(e);
    }
    else {
        shp(data).then((geojson) => {
            let option = {
                size: [512, 512],           // size[0] is svg width, size[1] is svg height
                padding: [0, 0, 0, 0],  // paddingTop, paddingRight, paddingBottom, paddingLeft, respectively
                output: 'string',           // output type: 'string' | 'element'(only supported in browser)
                precision: 10,               // svg coordinates precision
                stroke: 'red',              // stroke color
                strokeWidth: '0px',         // stroke width
                //background: '#ccc',         // svg background color, and as the fill color of polygon hole
                fill: 'black',              // fill color
                fillOpacity: 1,           // fill opacity
                radius: 5                   // only for `Point`, `MultiPoint`
            };
            //console.log(geojson);

            let bboxes = [];
            let names = [];

            for(let i = 0; i < geojson.features.length; i++) {
                let name = "plane_" + i;
                names.push(name);
                let fname_out = "output/" + name + ".svg";
                let feature = geojson.features[i];
                let bbox = getBBoxFromGeometry(feature.geometry);
                bboxes.push(bbox);
                let utmbbox = convertBBoxesToMeters([bbox]);
                for(let j = 0; j < feature.geometry.coordinates.length; j++) {
                    
                    let utm = utm.fromLatLon(feature.geometry.coordinates[0][1], feature.geometry.coordinates[0][0]);
                    feature.geometry.coordinates[0] = [utm.easting - utmbbox[0], utm.northing - utmbbox[1]];
                }
                let svgStr = geo2svg(feature, option);
                //console.log(feature);
                //already have bboxes
                //console.log(utm.fromLatLon());
                //break;
                fs.writeFile(fname_out, svgStr, (e) => {
                    if(e) {
                        console.log(e);
                    }
                });
            }

            //console.log(bboxes);
            let convertedBBoxes = convertBBoxesToMeters(bboxes);
            //console.log(convertedBBoxes);
            let outerBBox = getOuterBBox(convertedBBoxes);
            offSetBBoxes(outerBBox[0], outerBBox[1], convertedBBoxes);
            console.log(convertedBBoxes);
            let fcontents = "";
            let refName = "output/georef.csv"
            for(let i = 0; i < convertedBBoxes.length; i++) {
                let bbox = convertedBBoxes[i];
                fcontents += names[i] + ",";
                fcontents += bbox[0] + ",";
                fcontents += bbox[1] + ",";
                fcontents += bbox[2] + ",";
                fcontents += bbox[3] + "\n";
            }
            fs.writeFile(refName, fcontents, (e) => {
                if(e) {
                    console.log(e);
                }
            });

            
        }, (e) => {
            console.log(e);
        });
    }
});





