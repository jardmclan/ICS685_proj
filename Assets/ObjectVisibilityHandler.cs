using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisibilityHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameInvisible() {
        Debug.Log("invis");
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    void OnBecameVisible() {
        Debug.Log("vis");
        gameObject.transform.parent.gameObject.SetActive(true);
    }
}
