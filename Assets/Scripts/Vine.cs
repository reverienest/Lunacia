using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pubsub;


public class Vine : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D hazardCollider;
    public GameObject flower;

    // Start is called before the first frame update
    void Start()
    {
        MessageBroker.Instance.WakingSightModeTopic += consumeExampleMessage;
        hazardCollider = GetComponent<BoxCollider2D>();
    }

    private void consumeExampleMessage(object sender, WakingSightModeEventArgs example)
    {
        if (example.ActiveMode == 0)
        {
            hazardCollider.enabled = true;
            // flower.SetActive(false);
            animator.SetBool("wakingSight", false);
        }
        if (example.ActiveMode == 1)
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
        print(other.tag);
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            animator.SetBool("nearPlayer", false);
        }
    }
}
