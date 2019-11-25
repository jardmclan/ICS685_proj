using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using Wrld.Space;

public class WaterPlaneLoader : MonoBehaviour
{
    public Material waterMaterial;
    public Material holeMaterial;

    // Start is called before the first frame update
    void Start()
    {
        string county = "oahu";
        string set = "0ft_low";
        float heightft = 0.0f;
        float heightm = heightft * 0.3048f;
        //raise by just over a meter, terrain texturing seems to be +1m for urban, prevents clipping
        float baseHeight = -8.9f;
        float height = baseHeight + heightm;

        string respath = set + "/" + county + "/";

        GameObject wrapper = new GameObject();
        foreach(string record in File.ReadLines("Assets/Resources/slr_planes/" + respath + "/georef.csv")) {
            string[] fields = record.Trim().Split(',');
            string name = fields[0];
            float lng = float.Parse(fields[3]);
            float lat = float.Parse(fields[4]);

            GameObject planePrefab = Resources.Load<GameObject>(respath + name);
            GameObject plane = Instantiate(planePrefab, new Vector3(0, 0, 0), Quaternion.Euler(-90, 0, 180));
            GameObject geoWrapper = new GameObject();
            GeographicTransform geoTransform = geoWrapper.AddComponent<GeographicTransform>();
            //heading seems to be set and overwritten when the geographic transform starts, set to 0 from separate script after initialized
            geoWrapper.AddComponent<HeadingAdjustor>();
            LatLong pos = new LatLong(lat, lng);
            geoTransform.SetPosition(pos);
            geoTransform.SetElevation(height);
            //set planes parent to the geographic positioner
            plane.transform.parent = geoWrapper.transform;
            //set materials in object renderers
            Renderer[] renderers = plane.GetComponentsInChildren<Renderer>();
            foreach(Renderer renderer in renderers) {
                //if main plane renderer apply water material
                if(renderer.gameObject == plane) {
                    renderer.material = waterMaterial;
                }
                //apply hole material to everything else
                else {
                    renderer.material = holeMaterial;
                }
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
