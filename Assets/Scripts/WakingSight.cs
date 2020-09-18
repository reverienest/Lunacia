using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakingSight : MonoBehaviour
{
    public int activeMode = 0;
    public float maxScale = 10f;
    private bool changingMode = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire2") && !changingMode) {
            // Toggle through modes
            if (activeMode == 0) {
                StartCoroutine(changeMode(1));
            } else if (activeMode == 1) {
                StartCoroutine(changeMode(0));
            }
        }
    }

    IEnumerator changeMode(int mode) {
        if (mode != activeMode && !changingMode) {
            // Flag mode changing so coroutine can't be called twice simultaneously
            changingMode = true;
            activeMode = mode;
            if (mode == 0) {
                // Set to max scale and step down
                Vector3 scale = new Vector3(maxScale, maxScale, 0);
                Vector3 step = new Vector3(-maxScale/30, -maxScale/30, 0);
                while (transform.localScale.x > 0) {
                    scale += step;
                    transform.localScale = scale;
                    yield return new WaitForSeconds(0.1f);
                }
                transform.localScale = Vector3.zero;
            } else if (mode == 1) {
                // Set scale to zero and step up
                Vector3 scale = Vector3.zero;
                Vector3 step = new Vector3(maxScale/30, maxScale/30, 0);
                while (transform.localScale.x < maxScale) {
                    scale += step;
                    transform.localScale = scale;
                    yield return new WaitForSeconds(0.1f);
                }
                transform.localScale = new Vector3(maxScale, maxScale, 0);
            }
            changingMode = false;
        }
    }
}
