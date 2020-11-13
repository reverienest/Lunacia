using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pubsub;

public class FakeWall : MonoBehaviour
{
    void Awake() 
    {
        MessageBroker.Instance.WakingSightModeTopic += consumeWakingSightActiveEvent;
    }

    void consumeWakingSightActiveEvent(object sender, WakingSightModeEventArgs wakingSightState) {
        print(wakingSightState.ActiveMode);
        if (wakingSightState.ActiveMode == 1) {
            GetComponent<TilemapCollider2D>().enabled = false;
        } else if (wakingSightState.ActiveMode == 0) {
            GetComponent<TilemapCollider2D>().enabled = true;
        }
    }
}
