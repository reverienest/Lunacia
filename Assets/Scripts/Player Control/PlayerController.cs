using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float acceleration = 1;
    
    private Rigidbody2D rigid;

    [HideInInspector]
    public bool intentionalForce; //as opposed to unintentional force like gales & black holes

    [HideInInspector]
    public Vector2 boost;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get the Cursor Position relative to the player
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetMouseButton(0)) {
            moveDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            moveDirection.z = 0f;
        } else {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        }

        intentionalForce = !moveDirection.Equals(Vector3.zero);

        Vector2 force = (Vector2)(moveDirection.normalized * acceleration);
        force += Mathf.Clamp(Vector2.Dot(force.normalized, boost), 0, Mathf.Infinity) * force.normalized;

        rigid.AddForce(force * Time.deltaTime, ForceMode2D.Impulse);
        // print(rigid.velocity.magnitude);
    }
}
