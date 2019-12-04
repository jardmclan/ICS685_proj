using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyToggle : MonoBehaviour
{
    public bool startGravity = true;
    private bool gravity;

    // Start is called before the first frame update
    void Start()
    {
        gravity = startGravity;
        if(gravity) {
                gameObject.GetComponent<OVRPlayerController>().GravityModifier = 1.0f;
            }
            else {
                gameObject.GetComponent<OVRPlayerController>().GravityModifier = 0.0f;
            }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) {
            gravity = !gravity;
            if(gravity) {
                gameObject.GetComponent<OVRPlayerController>().GravityModifier = 1.0f;
            }
            else {
                gameObject.GetComponent<OVRPlayerController>().GravityModifier = 0.0f;
            }
            
        }
    }
}
