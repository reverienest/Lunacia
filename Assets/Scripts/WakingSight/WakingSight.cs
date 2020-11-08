using System.Collections;
using System.Collections.Generic;
using Pubsub;
using UnityEngine;

public class WakingSight : MonoBehaviour {
	public int activeMode = 0;
	public float maxScale = 10f;
	private bool changingMode = false;
	[SerializeField]
	private Animator circleAnimator;
	public bool inNZ = false;



	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		if (!changingMode) {
			if (Input.GetButtonDown("Fire2")) {
				if (inNZ == false) {
					// Toggle through modes
					if (activeMode == 0) {
						print("a");
						changeMode(1);
					} else if (activeMode == 1) {
						print("b");
						changeMode(0);
					}
				}
			} else if (inNZ == true && activeMode == 1) {
				print(":)");
				changeMode(0);
			} else if (inNZ == true && activeMode == 0) {
				// Trying to activate ws in a nz, do nothing. time permitting, add an "you can't do that!!" vfx. #TODO
			} else {
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "NullZone") {
			inNZ = true;
			print("hi");
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "NullZone") {
			inNZ = false;
			print("bye");
		}
	}

	void changeMode(int mode) {
		if (mode != activeMode && !changingMode) {
			changingMode = true;
			// Flag mode changing so coroutine can't be called twice simultaneously
			MessageBroker.Instance.Raise(new WakingSightModeEventArgs(mode));
			activeMode = mode;
			float scalarStep = maxScale / 30;
			if (mode == 0) {

				circleAnimator.SetBool("WakingSightOn", false);

				// Set to max scale and step down
				// Vector3 scale = new Vector3(maxScale, maxScale, 0);
				// Vector3 step = new Vector3(-maxScale/30, -maxScale/30, 0);
				// while (transform.localScale.x > 0) {
				//     scale += step;
				//     transform.localScale = scale;
				//     yield return new WaitForSeconds(0.1f);
				// }
				// transform.localScale = Vector3.zero;
			} else if (mode == 1) {

				circleAnimator.SetBool("WakingSightOn", true);

				// Set scale to zero and step up
				// Vector3 scale = Vector3.zero;
				// Vector3 step = new Vector3(maxScale/30, maxScale/30, 0);
				// while (transform.localScale.x < maxScale) {
				//     scale += step;
				//     transform.localScale = scale;
				//     yield return new WaitForSeconds(0.1f);
				// }
				// transform.localScale = new Vector3(maxScale, maxScale, 0);
			}
			changingMode = false;
		}
	}

	//    void OnTriggerEnter2D(Collider2D other) {
	//       
	//   }
}
