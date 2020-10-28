using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Pubsub;

public class LevelLoader : MonoBehaviour
{
	public GameObject loadingScreen;
	public Slider loadingBar;
	public Text progressPercentage;
	public static string nextSceneName;
	public GameObject player;

	void Awake() 
	{
		MessageBroker.Instance.SceneTransitionTopic += consumeSceneTransitionEvent;
		player.transform.position = KillPlayer.respawnLocation;
	}

	void OnDestroy() 
	{
        MessageBroker.Instance.SceneTransitionTopic -= consumeSceneTransitionEvent;
    }

    IEnumerator LoadAsynchronously(string nextSceneName) 
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);

    	loadingScreen.SetActive(true);

    	while (!operation.isDone) {
    		float progress = Mathf.Clamp01(operation.progress / .9f);
    		loadingBar.value = progress;
    		progressPercentage.text = progress * 100f + "%";

    		yield return null;
    	}

		loadingScreen.SetActive(false);
    }

	void consumeSceneTransitionEvent(object sender, SceneTransitionEventArgs transition) 
	{
		print(transition.sceneTransitionMessage);
        StartCoroutine(LoadAsynchronously(nextSceneName));
	}

}
