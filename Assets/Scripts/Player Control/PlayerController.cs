using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float acceleration;
    
    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.getComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get the Cursor Position relative to the player
        Vector2 rel = Camera.ScreenToWorld(Input.mousePosition).subtract(transform.position);

        if (Input.getMouseButton(0)) {
            rigidbody.addForce(rel.normalized * acceleration, ForceMode.Impulse);
        }
    }
}
