using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaleScript : MonoBehaviour
{
    public int speed;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(player.GetComponent<Rigidbody2D>().velocity);
    }

    void OnTriggerEnter2D() {
        print("hello");
    }
}
