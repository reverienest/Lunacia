using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pubsub;
using UnityEditorInternal;
using System.Net;

public class NullZoneScript : MonoBehaviour
{
    private BoxCollider2D zone;
    public Rigidbody2D player;
    public int WSmode;
    public bool inNZ = false;


    // Start is called before the first frame update
    void Start()
    {
        zone = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        MessageBroker.Instance.WakingSightModeTopic += consumeExampleMessage;
    }

    private void consumeExampleMessage(object sender, WakingSightModeEventArgs example)
    {
        if (example.ActiveMode == 0)
        {
            WSmode = 0;
        }
        if (example.ActiveMode == 1)
        {
            WSmode = 1;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            inNZ = true;
            print("hi");
        }  
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inNZ = false;
            print("bye");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (inNZ == true)
        {
            if (WSmode == 1)
            {
                print("ws is on");
                MessageBroker.Instance.Raise(new WakingSightModeEventArgs(0));
            }
            OnTriggerExit2D(zone);
        }
    }
}
