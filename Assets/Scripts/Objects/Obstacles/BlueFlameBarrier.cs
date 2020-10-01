using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFlameBarrier : MonoBehaviour
{
    public GameObject player;

    void Update() {
        if (KillPlayer.hasBlueFlame) {
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        } else {
            this.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
