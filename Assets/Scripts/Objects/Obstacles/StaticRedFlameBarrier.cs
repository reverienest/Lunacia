using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticRedFlameBarrier : MonoBehaviour
{
    void Update()
    {
        if (KillPlayer.hasRedFlame) {
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        } else {
            this.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
