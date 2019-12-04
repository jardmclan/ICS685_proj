using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TetsLoader : MonoBehaviour
{

    public Material[] waterMaterials;

    // Start is called before the first frame update
    void Start()
    {
        //AudioSource splashSounds = gameObject.AddComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) {
            ScenarioController.setSLR(2);
            SceneLoader.loadSLR();
        }
    }

    
}
