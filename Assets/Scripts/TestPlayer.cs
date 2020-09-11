using System.Collections;
using UnityEngine;

/*
The player movement class for testing purposes
Includes acceleration and deceleration
*/
public class TestPlayer : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField]
    private float speed = 0f; //do not change

    public float maxSpeed = 10.0f; //max speed the player can reach

    [SerializeField]
    private float accel = 10.0f; //rate at which the player gains speed

    [SerializeField]
    private float decel = 8.0f; // rate at which player loses speed

    private Vector2 movement;
    private Vector2 pastMovement; //Last known direction the player was headed

    private float moveX;
    private float moveY;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        pastMovement = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveX = movement.x;
        moveY = movement.y;

        //checks for input and speed, applies acceleration and records pastMovement
        if ((moveX != 0 || moveY != 0) && speed < maxSpeed)
        {
            speed = speed + accel * Time.deltaTime;
            if (pastMovement != movement)
            {
                pastMovement = movement;
            }
        }
        //if there is no input, decelerate the player to an eventual stop and clear pastMovement
        else
        {
            if (speed > decel * Time.deltaTime)
            {
                speed = speed - decel * Time.deltaTime;
            }
            else
            {
                speed = 0f;
                pastMovement = new Vector2();
            }
        }
    }

    void FixedUpdate()
    {
        if (moveX != 0 || moveY != 0)
        {
            MovePlayer(movement);
        }
        else 
        {
            MovePlayer(pastMovement);
        }
    }

    //moves the player's position
    private void MovePlayer(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
}
