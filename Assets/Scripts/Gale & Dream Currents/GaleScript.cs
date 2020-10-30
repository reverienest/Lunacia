using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaleScript : MonoBehaviour
{
    public ParticleSystem burst;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            burst.Play();
        }
    }
}
