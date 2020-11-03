using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pubsub;

public class DynamicFlameBarrier : MonoBehaviour
{
   private bool isRedFlame;
   private bool isBlueFlame;
   public GameObject redFlame;
   public GameObject blueFlame;

   void Awake() {
        MessageBroker.Instance.WakingSightModeTopic += consumeWakingSightActiveEvent;
        isRedFlame = true;
        isBlueFlame = false;
    }
   void consumeWakingSightActiveEvent(object sender, WakingSightModeEventArgs wakingSightState) {
       print(wakingSightState.ActiveMode);
       if (wakingSightState.ActiveMode == 0) {
           isRedFlame = false;
           isBlueFlame = true;
       } else if (wakingSightState.ActiveMode == 1) {
           isRedFlame = true;
           isBlueFlame = false;
       }
   }
   void Update() {
       if (isRedFlame) {
           redFlame.GetComponent<BoxCollider2D>().enabled = true;
           blueFlame.GetComponent<BoxCollider2D>().enabled = false;
           if (KillPlayer.hasRedFlame) {
                redFlame.GetComponent<BoxCollider2D>().isTrigger = true;
            } else {
                redFlame.GetComponent<BoxCollider2D>().isTrigger = false;
            }
       } else if (isBlueFlame) {
            redFlame.GetComponent<BoxCollider2D>().enabled = false;
            blueFlame.GetComponent<BoxCollider2D>().enabled = true;
            if (KillPlayer.hasBlueFlame) {
                blueFlame.GetComponent<BoxCollider2D>().isTrigger = true;
            } else {
                blueFlame.GetComponent<BoxCollider2D>().isTrigger = false;
            }
       }
   }
}
