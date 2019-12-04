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

for c in bpy.context.scene.collection.children:
    bpy.context.scene.collection.children.unlink(c)

name = "test"
bpy.ops.import_curve.svg(filepath = cd + "/2.80/scripts/slr_planes/test_input/10ft_low_oahu_126.svg")

main_obj = bpy.context.scene.objects[0]

main_height = 10
hole_height = 10.01

x = 100
y = 100

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

main_obj.select_set(False)
#main_obj deselected so can't be active, just need one of the selected objects to be marked active so context valid
bpy.context.view_layer.objects.active = holes[0]


for obj in holes:
    obj.select_set(True)
convert_and_extrude(hole_height)


bpy.ops.export_scene.fbx(filepath = cd + "/2.80/scripts/slr_planes/test_output/" + name + ".fbx")

    #obj.scale = scale



    
    #bpy.ops.object.origin_set(type = 'ORIGIN_GEOMETRY', center = 'BOUNDS')
    #obj.dimensions = [x, y, 0]
#     bpy.context.scene.cursor.location = obj.matrix_world.translation
# print(bpy.context.scene.cursor.location)


#     bpy.ops.object.convert(target = 'MESH')


#     bpy.ops.object.mode_set(mode = 'EDIT')
#     bpy.ops.mesh.select_mode(type = 'FACE')
#     bpy.ops.mesh.select_all(action = 'SELECT')

#     bpy.ops.mesh.extrude_region_move(
#         TRANSFORM_OT_translate = {"value": (0, 0, 10)}
#     )

#     bpy.ops.object.mode_set(mode = 'OBJECT')

# #

# def setup_object(obj, scale_x, scale_y, height):



