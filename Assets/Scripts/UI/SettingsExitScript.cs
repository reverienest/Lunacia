using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsExitScript : MonoBehaviour
{
    private AudioListener previousSceneAudioListener;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Obsolete]
    public void ExitFromSettings()
    {
        int sCount = SceneManager.sceneCount;

        if (sCount != 2)
        {
            Debug.Log("there are more or less than 2 scenes, which shouldn't be happening.");
            return;
        }

        //there are 2 scenes correctly loaded in the scene manager
        //one of them is the active scene, which is this setting menu

        Scene s1 = SceneManager.GetSceneAt(0);
        Scene s2 = SceneManager.GetSceneAt(1);

        Scene settingsScene = SceneManager.GetSceneByName("SettingsMenu 1");

        Scene previousScene = s1 == settingsScene ? s2 : s1;


        GameObject[] mCams = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject mCam in mCams)
        {
            if(!mCam.scene.name.Equals(settingsScene.name))
            {
                previousSceneAudioListener = mCam.GetComponent<AudioListener>();
            }
            //Debug.Log(mCam.scene.name);
        }

        SceneManager.sceneUnloaded += SettingsSceneUnloaded;
        if (settingsScene != null)
        {
            SceneManager.UnloadScene(settingsScene);
        }
    }

    //this is only run when the settings scene is unloaded, after running, it removes itself from SceneManager
    private void SettingsSceneUnloaded(Scene s)
    {
        if (previousSceneAudioListener != null)
        {
            previousSceneAudioListener.enabled = true;
        }

        previousSceneAudioListener = null;
        SceneManager.sceneUnloaded -= SettingsSceneUnloaded;
    }
}
