using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public bool startGravity = true;
    public Vector3 spawnAnchor = new Vector3(0, 200, 0);

    private bool gravity;


    void Start()
    {
        gravity = startGravity;
        if(gravity) {
            gameObject.GetComponent<OVRPlayerController>().GravityModifier = 1.0f;
            gameObject.GetComponent<OVRPlayerController>().enabled = true;
        }
        else {
            gameObject.GetComponent<OVRPlayerController>().GravityModifier = 0.0f;
            gameObject.GetComponent<OVRPlayerController>().enabled = false;
        }
    }

    void Update()
    {
        flyToggle();
        testReset();
        positionReset();
        flight();
        rotate();
        farPlane();
    }

    void flyToggle() {
        if(Input.GetKeyDown(KeyCode.T))  {
            gravity = !gravity;
            if(gravity) {
                gameObject.GetComponent<OVRPlayerController>().GravityModifier = 1.0f;
                gameObject.GetComponent<OVRPlayerController>().enabled = true;
            }
            else {
                gameObject.GetComponent<OVRPlayerController>().GravityModifier = 0.0f;
                gameObject.GetComponent<OVRPlayerController>().enabled = false;
            }
            
        }
    }

    void testReset() {
        if(Input.GetKeyDown(KeyCode.N)) {
            ScenarioController.setSLR(2);
            SceneLoader.loadSLR();
        }
    }

    void positionReset() {
        
        if(Input.GetKeyDown(KeyCode.R)) {
            //Transform parent = gameObject.transform.parent;
            //need to disable ovr controller before moving to prevent movement adjustments
            bool ovrControllerState = gameObject.GetComponent<OVRPlayerController>().enabled;
            gameObject.GetComponent<OVRPlayerController>().enabled = false;
            gameObject.transform.position = new Vector3(spawnAnchor.x, spawnAnchor.y, spawnAnchor.z);
            gameObject.GetComponent<OVRPlayerController>().enabled = ovrControllerState;
        }
    }


    private bool flying = false;
    private Vector3 startPos;
    void flight() {
        //can only fly if there's no gravity
        if(!gravity) {
            if(Input.GetMouseButtonDown(0) && !flying) {
                flying = true;
                startPos = Input.mousePosition;
            }
            else if(Input.GetMouseButtonUp(0)) {
                flying = false;
            }

            if(flying) {
                Vector3 dir = Input.mousePosition - startPos;
                gameObject.transform.Translate(dir);
                Debug.Log("!");
            }
        }
    }

    public float rotation = 20;
    void rotate() {
        if(!gravity) {
            if(Input.GetKeyDown(KeyCode.Q)) {
                Debug.Log("Q");
                gameObject.transform.Rotate(new Vector3(0, -rotation, 0));
            }
            if(Input.GetKeyDown(KeyCode.E)) {
                gameObject.transform.Rotate(new Vector3(0, rotation, 0));
            }
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            Debug.Log("W");
            gameObject.transform.Rotate(new Vector3(-rotation, 0, 0));
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            gameObject.transform.Rotate(new Vector3(rotation, 0,  0));
        }
        
        
    }

    public float maxAngle = 85.0f;
    public float minFarPlane = 1000.0f;
    public float maxFarPlane = 30000.0f;
    public Camera mainCam;
    void farPlane() {
        float pitch = gameObject.transform.eulerAngles.x;

        //Debug.Log();
        float upperAngle = 90 - adjustDegreePlane(gameObject.transform.eulerAngles.x) + (mainCam.fieldOfView / 2.0f);
        upperAngle = Mathf.Abs(upperAngle);
        if(upperAngle > maxAngle) {
            upperAngle = maxAngle;
        }
        Debug.Log(upperAngle);
        float far = (1.0f / Mathf.Cos(Mathf.Deg2Rad * upperAngle)) * gameObject.transform.position.y;
        if(far < minFarPlane) {
            far = minFarPlane;
        }
        else if(far > maxFarPlane) {
            far = maxFarPlane;
        }
        mainCam.farClipPlane = far;
    }

    private float adjustDegreePlane(float angle) {
        float adjusted = angle;
        if(angle > 180) {
            adjusted -= 360;
        }
        return adjusted;
    }
}
