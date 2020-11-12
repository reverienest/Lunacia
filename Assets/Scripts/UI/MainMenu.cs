using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public string startGameScene = "GladeMap1";
	public string creditsScene = "Credits";
	public string settingsMenu = "SettingsMenu 1";

	// load save (or new game)
	public void Play_Game() {
		Debug.Log("Start game! Scene \"" + startGameScene + "\" now loading...");
		if (!startGameScene.Equals("")) {
			SceneManager.LoadScene(startGameScene);
		}
	}

	public void Click_Settings() {
		if (!settingsMenu.Equals("")) {
			SceneManager.LoadScene(settingsMenu);
		}
	}

	// load save (or new game)
	public void Click_Credits() {
		if (!creditsScene.Equals("")) {
			SceneManager.LoadScene(creditsScene);
		}
	}

	public void Quit_Game() {
#if UNITY_EDITOR
		// Application.Quit() does not work in the editor so
		// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
		UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
	}
}
