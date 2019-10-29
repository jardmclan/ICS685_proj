using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterModifier : MonoBehaviour {
    private bool first = true;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(first) {
            foreach(Transform child in transform) {
                Debug.Log(child.name + "\n");
                //"Water - 0"
            }
            first = false;
        }
        
    }
}
