using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter mesh = gameObject.GetComponent<MeshFilter>();
        Vector3[] vertices = mesh.mesh.vertices;
        for(int i = 0; i < vertices.Length; i++) {
            Debug.Log(vertices[i].x);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
