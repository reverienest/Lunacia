using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlameBarrier : MonoBehaviour
{
    public GameObject player;

    void Update() {
        if (KillPlayer.hasRedFlame) {
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        } else {
            this.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
    
}
