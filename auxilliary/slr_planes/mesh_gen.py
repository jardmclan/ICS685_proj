import bpy
import os


#conversion to mesh and extrusion can be done with multiple selected, apply to main then holes since different heights
def convert_and_extrude(height):
    bpy.ops.object.convert(target = 'MESH')


    bpy.ops.object.mode_set(mode = 'EDIT')
    bpy.ops.mesh.select_mode(type = 'FACE')
    bpy.ops.mesh.select_all(action = 'SELECT')

    bpy.ops.mesh.extrude_region_move(
        TRANSFORM_OT_translate = {"value": (0, 0, height)}
    )

    bpy.ops.object.mode_set(mode = 'OBJECT')

cd = os.getcwd()
wd = cd + "/2.80/scripts/slr_planes/"
counties = ["hawaii", "maui", "oahu", "kauai"]
#counties = ["oahu"]
drange = range(0, 11)
for sl in drange:
    for county in counties:
        indir_rel = "input/" + str(sl) + "ft_low/" + county + "/"
        outdir_rel = "output/" + str(sl) + "ft_low/" + county + "/"
        indir = wd + indir_rel
        outdir = wd + outdir_rel
        if not os.path.exists(outdir):
            os.makedirs(outdir)

        ref_data = []
        with open(indir + "georef.csv") as f:
            for line in f:
                record = line.rstrip()
                ref_data.append(record.split(','))
                
        for record in ref_data:

            for c in bpy.context.scene.collection.children:
                bpy.context.scene.collection.children.unlink(c)

            name = record[0]
            bpy.ops.import_curve.svg(filepath = indir + name + ".svg")
            print(record)

            x = float(record[1])
            y = float(record[2]) 
            
            main_obj = bpy.context.scene.objects[0]

            main_height = 10
            hole_height = 10.01

            main_obj.name = name
            holes = []
            for obj in bpy.context.scene.objects[1:]:
                obj.parent = main_obj
                holes.append(obj)

            main_obj.select_set(True)
            bpy.context.view_layer.objects.active = main_obj

            bpy.ops.object.origin_set(type = 'ORIGIN_GEOMETRY', center = 'BOUNDS')
            main_obj.dimensions = [x, y, 0]

            convert_and_extrude(main_height)

            #process holes if there are any
            if len(holes) > 0:
                main_obj.select_set(False)
                #main_obj deselected so can't be active, just need one of the selected objects to be marked active so context valid
                bpy.context.view_layer.objects.active = holes[0]

                for i in range(len(holes)):
                    obj = holes[i]
                    obj.name = name + "hole_" + str(i)
                    obj.select_set(True)
                convert_and_extrude(hole_height)


            bpy.ops.export_scene.fbx(filepath = outdir + name + ".fbx")

        #indicate not frozen, note after a directory complete
        print(indir_rel + " processed, results in " + outdir_rel)



