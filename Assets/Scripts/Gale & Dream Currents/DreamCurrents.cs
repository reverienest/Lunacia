using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class DreamCurrents : MonoBehaviour
{
    public float force;
    [HideInInspector]
    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = (Vector2)transform.TransformVector(Vector3.right).normalized;
    }

    void OnTriggerEnter2D(Collider2D other) {
        print(other.tag);
        if (other.tag == "Player") {
            other.GetComponent<PlayerController>().boost += direction * force;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerController>().boost -= direction * force;
        }
    }
}
