using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
	// Start is called before the first frame update

	public Image backgroundImage;
	public Sprite[] slides;
	public int currentSlide = 0;
	public string nextScene = "GladeMap1";

	// Update is called once per frame
	void Update() {
		if (Input.GetButtonDown("Fire1")) {
			currentSlide++;
			if (currentSlide >= slides.Length) {
				SceneManager.LoadScene(nextScene);
			} else {
				updateSlide(currentSlide);
			}
		}
		if (Input.GetButtonDown("Fire2")) {
			if (currentSlide > 0) {
				currentSlide--;
				updateSlide(currentSlide);
			}

		}
	}

	void updateSlide(int newSlide) {
		backgroundImage.sprite = slides[newSlide];
	}
}
