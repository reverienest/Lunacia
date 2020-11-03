using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    // Master Volume control
    public void SetMasterVolume(float volume) {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20);
    }
    
    // Music Volume control
    public void SetMusicVolume(float volume) {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }
    
    // SFX Volume control
    public void SetSFXVolume(float volume) {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
    }    
}
