using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public Rigidbody2D rb;

    public float speed = 10.0f;

    public Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        MovePlayer(movement);
    }

    private void MovePlayer(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
}
