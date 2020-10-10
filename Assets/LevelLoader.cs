using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	public GameObject loadingScreen;
	public Slider loadingBar;
	public Text progressPercentage;

    public void LoadLevel (int sceneIndex) {
    	StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex) {
    	AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

    	loadingScreen.SetActive(true);

    	while (!operation.isDone) {
    		float progress = Mathf.Clamp01(operation.progress / .9f);
    		loadingBar.value = progress;
    		progressPercentage.text = progress * 100f + "%";

    		yield return null;
    	}
    }
}
