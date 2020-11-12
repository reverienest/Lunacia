using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pubsub;
using WakingSightNS;

public class Vine : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D hazardCollider;
    public GameObject flower;

    // Start is called before the first frame update
    void Start()
    {
        MessageBroker.Instance.WakingSightModeTopic += consumeWakingSightActiveEvent;
        hazardCollider = GetComponent<BoxCollider2D>();
    }

    private void consumeWakingSightActiveEvent(object sender, WakingSightModeEventArgs wsArgs)
    {
        print(wsArgs.PickupLevel);
		if (wsArgs.ActiveMode == 0 || wsArgs.PickupLevel < WSPickupLevel.Glade)
        {
            hazardCollider.enabled = true;
            // flower.SetActive(false);
            animator.SetBool("wakingSight", false);
        }
        if (wsArgs.ActiveMode == 1 && wsArgs.PickupLevel >= WSPickupLevel.Glade)
        {
            hazardCollider.enabled = false;
            // flower.SetActive(true);
            animator.SetBool("wakingSight", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            animator.SetBool("nearPlayer", true);
        }
        // print(other.tag);
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            animator.SetBool("nearPlayer", false);
        }
    }

    void OnDestroy() {
        MessageBroker.Instance.WakingSightModeTopic -= consumeWakingSightActiveEvent;
    }
}
