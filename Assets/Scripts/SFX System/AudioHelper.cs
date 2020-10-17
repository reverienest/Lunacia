using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//each scene has an AudioHelper that contains a list of sounds that are used in the scene
//any two sounds that are different from each other MUST have different names
public class AudioHelper : MonoBehaviour
{
    private AudioManager audioManager;
    public List<Sound> sceneSoundLibrary;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogWarning("Audio Manager not found");
            return;
        }

        //adds the sound library in this audio helper to the Audio Manager's library
        audioManager.AddSceneSounds(sceneSoundLibrary);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
