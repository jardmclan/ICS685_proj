using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using Wrld.Space;

public class WaterPlaneLoader : MonoBehaviour
{
    public Material waterMaterial;
    public Material holeMaterial;
    public AudioClip ambientAudio;
    public AudioClip[] contactAudio;
    public AudioClip exitAudio;

    // Start is called before the first frame update
    void Start()
    {
        string county = ScenarioController.getCounty();
        SLRTags tags = ScenarioController.getSLRTags();
        float heightm = ScenarioController.getSLRMeters();
        //raise by just over a meter, terrain texturing seems to be +1m for urban, prevents clipping
        float baseHeight = -8.9f;
        float height = baseHeight + heightm;

        string respath = "slr_planes/" + tags.low + "/" + county + "/";

        GameObject wrapper = new GameObject();
        foreach(string record in File.ReadLines("Assets/Resources/" + respath + "/georef.csv")) {
            string[] fields = record.Trim().Split(',');
            string name = fields[0];
            float lng = float.Parse(fields[3]);
            float lat = float.Parse(fields[4]);

            GameObject planePrefab = Resources.Load<GameObject>(respath + name);
            GameObject plane = Instantiate(planePrefab, new Vector3(0, 0, 0), Quaternion.Euler(-90, 0, 180));
            GameObject geoWrapper = new GameObject();
            GeographicTransform geoTransform = geoWrapper.AddComponent<GeographicTransform>();
            //heading seems to be set and overwritten when the geographic transform starts, set to 0 from separate script after initialized
            geoWrapper.AddComponent<HeadingAdjustor>();
            LatLong pos = new LatLong(lat, lng);
            geoTransform.SetPosition(pos);
            geoTransform.SetElevation(height);
            //set planes parent to the geographic positioner
            plane.transform.parent = geoWrapper.transform;

            //set up audio
            AudioSource ambient = plane.AddComponent<AudioSource>();
            ambient.clip = ambientAudio;
            ambient.spatialBlend = 1.0f;
            ambient.maxDistance = 100.0f;
            ambient.rolloffMode = AudioRolloffMode.Logarithmic;
            ambient.loop = true;
            ambient.Play();

            AudioSource splash = plane.AddComponent<AudioSource>();
            splash.volume = 0.25f;
            SplashHandler splashHandler = plane.AddComponent<SplashHandler>();
            splashHandler.source = splash;
            splashHandler.exitAudio = exitAudio;
            splashHandler.contactAudio = contactAudio;


            //set materials in object renderers
            Renderer[] renderers = plane.GetComponentsInChildren<Renderer>();
            foreach(Renderer renderer in renderers) {
                //if main plane renderer apply water material
                if(renderer.gameObject == plane) {
                    renderer.material = waterMaterial;
                }
                //apply hole material to everything else
                else {
                    renderer.material = holeMaterial;
                }
            }
            
        }

        GameObject test = GameObject.Find("Cube (1)");
        Debug.Log(test);
        AudioSource a = test.AddComponent<AudioSource>();
        a.clip = ambientAudio;
        a.spatialBlend = 1.0f;
        a.maxDistance = 100.0f;
        a.rolloffMode = AudioRolloffMode.Logarithmic;
        a.loop = true;
        
        a.Play();

        a = test.AddComponent<AudioSource>();
        SplashHandler s = test.AddComponent<SplashHandler>();
        s.source = a;
        s.exitAudio = exitAudio;
        a.volume = 0.25f;
        s.contactAudio = contactAudio;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
