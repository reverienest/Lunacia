using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlayerFlame : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && (KillPlayer.hasBlueFlame || KillPlayer.hasRedFlame)) {
            Destroy(col.gameObject.transform.GetChild(2).gameObject);
            KillPlayer.hasBlueFlame = false;  
            KillPlayer.hasRedFlame = false;  
        }
    }
}
