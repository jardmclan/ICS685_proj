using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlaneLockToCamera : MonoBehaviour {

    public Camera camera;
    public float clippingDistance = 1000;

    //also need position
    //static height (y axis)

    //x pitch, y yaw, z roll
    //x = 90def straight down
    //-90 - 90deg have ground view at some distance
    //get lowe
    //forward +z, back -z
    //left -x, right +x
    //up +y, down -y


    // Start is called before the first frame update
    void Start() {
        //for now, maximum
        // camera.farClipPlane
        // transform.localScale = new Vector3();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
