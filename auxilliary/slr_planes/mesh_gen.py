import bpy
import os

ref_data = []
cd = os.getcwd()
with open(cd + "/2.80/scripts/slr_planes/input/georef.csv") as f:
    for line in f:
        record = line.rstrip()
        ref_data.append(record.split(','))

bpy.ops.import_curve.svg(filepath=cd + "/2.80/scripts/slr_planes/input/" + ref_data[0][0] + ".svg")
#for record in ref_data:
