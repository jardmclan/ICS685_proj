using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrld.Space;

public class HeadingAdjustor : MonoBehaviour
{
    bool firstFrame = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //heading seems to be set and overwritten when the geographic transform starts, set to 0 on first update frame
        if(firstFrame) {
            //gameObject.transform.parent.gameObject.SetActive(false);
            GeographicTransform geoTransform = gameObject.GetComponent<GeographicTransform>();
            geoTransform.SetHeading(0);
            firstFrame = false;
        }
    }
}
