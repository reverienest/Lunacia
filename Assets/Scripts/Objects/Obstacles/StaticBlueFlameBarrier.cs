using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBlueFlameBarrier : MonoBehaviour
{
    void Update()
    {
        if (KillPlayer.hasBlueFlame) {
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        } else {
            this.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
