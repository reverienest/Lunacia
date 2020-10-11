using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public bool facingLeft = false;

    private Rigidbody2D rigid;
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentInParent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = rigid.velocity;

        if (vel.x != 0)
        {
            facingLeft = vel.x < 0;
            renderer.flipX = vel.x < 0;
            facingLeft = vel.x < 0.0f;
        }
        else
        {

        }
    }
}
