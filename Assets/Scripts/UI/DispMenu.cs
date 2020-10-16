using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DispMenu : MonoBehaviour {

    public TMP_Dropdown resDropdown;

    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currResInd = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height) {

                currResInd = i;
            }
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currResInd;
        resDropdown.RefreshShownValue();
    }

    public void SetResolution(int resInd) {
        Resolution resolution = resolutions[resInd];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(int resInd) {
        if (resInd == 0) {
            Screen.fullScreen = true;
        } else {
            Screen.fullScreen = false;
        }
    }

}
