using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
