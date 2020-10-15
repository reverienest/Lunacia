using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    private Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = (Vector2)transform.TransformVector(Vector3.up);
        print(direction);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            float impact = Vector2.Dot(rb.velocity, direction);
            if (impact < 0) {
                rb.velocity -= 2 * impact * direction;
            }
        }
    }
}
