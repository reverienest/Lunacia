using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class VelocityBarrier : MonoBehaviour
{

    private BoxCollider2D collider;
    private Vector2 direction;
    public Rigidbody2D playerRB;
    public float velocityThreshold = 10;
    public float recoil = 10;
    public ParticleSystem burst;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        direction = (Vector2)transform.TransformVector(Vector2.right);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Vector2.Dot(playerRB.velocity, direction)) > velocityThreshold) {
            collider.isTrigger = true;
        } else if (collider.isTrigger) {
            collider.isTrigger = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            float dotProd = Vector2.Dot(playerRB.velocity,direction);
            playerRB.AddForce(-recoil * direction * Mathf.Sign(dotProd), ForceMode2D.Impulse);
            var main = burst.main;
            if (dotProd > 0) {
                main.startSpeed = 5f;
            } else {
                main.startSpeed = -5f;
            }
            burst.Play();
        }
    }
}
