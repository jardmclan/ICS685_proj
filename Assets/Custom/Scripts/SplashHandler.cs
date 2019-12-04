using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SplashHandler : MonoBehaviour
{
    private bool contact;

    public AudioClip[] contactAudio;
    public AudioClip exitAudio;
    public AudioSource source;
    public int splashPause = 5;

    private System.Random r = new System.Random();
    private Coroutine splashRoutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider c) {
        contact = true;
        splashRoutine = StartCoroutine(Splash());
    }

    void OnTriggerExit(Collider c) {
        StopCoroutine(splashRoutine);
        source.PlayOneShot(exitAudio);
        contact = false;
    }

    IEnumerator Splash() {
        int i;
        while(contact) {
            i = r.Next(0, contactAudio.Length);
            //play sounds
            source.PlayOneShot(contactAudio[i]);
            yield return new WaitForSeconds(splashPause);
        }
    }
}
