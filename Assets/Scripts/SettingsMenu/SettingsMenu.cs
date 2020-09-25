﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    
    public AudioMixer audioMixer;
    
    Resolution[] resolutions;
    
    void Start() {
        resolutions = Screen.resolutions;
        
    }
    
    
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
    
    
    // TEST CODE -- WILL BE DELETED
    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }
    
}
