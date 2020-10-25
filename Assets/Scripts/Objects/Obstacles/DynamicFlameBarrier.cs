using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pubsub;

public class DynamicFlameBarrier : MonoBehaviour
{
   private bool isRedFlame;
   private bool isBlueFlame;
   void Awake() {
        MessageBroker.Instance.WakingSightModeTopic += consumeWakingSightActiveEvent;
        isRedFlame = true;
        isBlueFlame = false;
    }
   void consumeWakingSightActiveEvent(object sender, WakingSightModeEventArgs wakingSightState) {
       print(wakingSightState.ActiveMode);
       if (wakingSightState.ActiveMode == 1) {
           isRedFlame = false;
           isBlueFlame = true;
           this.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
       } else if (wakingSightState.ActiveMode == 0) {
           isRedFlame = true;
           isBlueFlame = false;
           this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
       }
   }
   void Update() {
       if (isRedFlame) {
           if (KillPlayer.hasRedFlame) {
                this.GetComponent<BoxCollider2D>().isTrigger = true;
           } else {
                this.GetComponent<BoxCollider2D>().isTrigger = false;
           }
       } else if (isBlueFlame) {
           if (KillPlayer.hasBlueFlame) {
                this.GetComponent<BoxCollider2D>().isTrigger = true;
           } else {
                this.GetComponent<BoxCollider2D>().isTrigger = false;
           }
       }
   }
}
