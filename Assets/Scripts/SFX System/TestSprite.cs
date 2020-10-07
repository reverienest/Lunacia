using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSprite : MonoBehaviour 
{
    private AudioManager audioManager;
    
    // Start is called before the first frame update
    void Start() {
        audioManager = AudioManager.instance;
        //to play a sound, call the Play method of the AudioManager, specifying the name of the sound and the gameObj to play it from
        audioManager.Play("Spawn", gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            audioManager.Play("Spawn", gameObject);
        } else if (Input.GetKeyDown(KeyCode.Tab)) {
            audioManager.Play("Spawn");
        }
    }

}
