using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttuneRedFlame : MonoBehaviour
{
    public GameObject player;
    public GameObject flameParent;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !KillPlayer.hasRedFlame) {
            if (KillPlayer.hasBlueFlame) {
                Transform currentFlame = null;
                Transform[] children = player.GetComponentsInChildren<Transform>();
                foreach (Transform child in children) {
                    if (child.tag == "Flame") {
                        currentFlame = child;
                    }
                }
                currentFlame.transform.parent = flameParent.transform;
                currentFlame.transform.position = this.transform.position;
                StartCoroutine(waitToChangeFlameValue());
            }
            KillPlayer.hasRedFlame = true;
            this.transform.position = player.transform.position;
            this.transform.parent = player.transform;
            this.transform.localPosition = new Vector3(-0.08f, -0.47f, transform.localPosition.z); 
        }
    }
    IEnumerator waitToChangeFlameValue() {
        yield return new WaitForSeconds(1.0f);
        KillPlayer.hasBlueFlame = false;
    }
}
