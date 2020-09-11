using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float acceleration;
    
    private Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get the Cursor Position relative to the player
        Vector3 rel = transform.position - Camera.current.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0)) {
            rigid.AddForce(rel.normalized * acceleration, ForceMode2D.Impulse);
        }
    }
}
