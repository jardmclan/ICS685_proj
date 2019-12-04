using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrld.Space;

public class TestPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GeographicTransform test = gameObject.GetComponent<GeographicTransform>();
        test.SetElevation(-8);
        //LatLong pos = new LatLong(21.5, -157.83);
        //test.SetPosition(pos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
