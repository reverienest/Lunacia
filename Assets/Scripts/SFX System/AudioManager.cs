using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour 
{
    public List<Sound> currentSoundLibrary;
    public static AudioManager instance;
    void Awake() 
    {
        //ensures there is only one instance of AudioManager and is not destroyed when loading a new scene
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start () 
    {
        //you can play sounds from within the audio manager using Play method
        Play("Music");
    }

    //called by the AudioHelper in a scene
    //adds the given sounds list to the currentSoundLibrary of the AudioManager
    public void AddSceneSounds(List<Sound> sounds)
    {
        foreach (Sound s in sounds)
        {
            //only adds sounds that do not share a name with a sound in the currentSoundLibrary
            if (!currentSoundLibrary.Exists(sound => sound.name == s.name))
            {
                currentSoundLibrary.Add(s);
            }
        }

    }

    //plays the desired sound from the GameObject you specify
    //name is the name of the sound you want to play
    //gameObj is the GameObject you want the AudioSource to belong to and play from
    //if no gameObj is specified the AudioSource will play from the AudioManager itself
    public void Play (string name, GameObject gameObj = null) 
    {
        Sound s = currentSoundLibrary.Find(sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound " + name + " not found in current sound library");
            return;
        }

        //sets the currentAudioSource to the correct Audio Source or creates an Audio Source if needed
        setCurrentAudioSource(s, gameObj);

        s.currentAudioSource.Play();
    }

    //helper method for Play
    //sets the currentAudioSource of s to the correct AudioSource
    //ensures the gameObj does not have multiple audio sources for the same sound
    private void setCurrentAudioSource(Sound s, GameObject gameObj)
    {
        AudioSource tempAudioSource = null;
        AudioSource[] gameObjectAudioSources;

        if (gameObj != null) {
            gameObjectAudioSources = gameObj.GetComponents<AudioSource>();
        } else {
            gameObjectAudioSources = gameObject.GetComponents<AudioSource>();
        }

        if (gameObjectAudioSources != null) {
            tempAudioSource = Array.Find(gameObjectAudioSources, audioSource => audioSource.clip == s.clip);
        }

        if (tempAudioSource == null) {
            InitAudioSource(s, gameObj);
        } else {
            s.currentAudioSource = tempAudioSource;
        }
    }

    //creates an Audio Source in the specified Game Object for the given sound
    //helper method for the setCurrentAudioSource method
    private void InitAudioSource(Sound s, GameObject gameObj)
    {
        //audio source added to the AudioManager itself if gameObj is null
        if (gameObj == null)
        {
            s.currentAudioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            s.currentAudioSource = gameObj.AddComponent<AudioSource>();
        }

        //uses properties of Sound to create AudioSource with same properties
        s.currentAudioSource.clip = s.clip;
        s.currentAudioSource.volume = s.volume;
        s.currentAudioSource.pitch = s.pitch;
        s.currentAudioSource.loop = s.loop;
        s.currentAudioSource.spatialBlend = s.spatialBlend;
    }

}
