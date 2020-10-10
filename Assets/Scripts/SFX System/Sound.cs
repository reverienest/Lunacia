using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
//hold the basic audio source information of a given sound
public class Sound 
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    [Range(0f, 1f)]
    public float spatialBlend;
    public bool loop;
    [HideInInspector]
    public AudioSource currentAudioSource;
}
