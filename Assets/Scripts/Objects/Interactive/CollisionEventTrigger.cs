using UnityEngine;
using UnityEngine.Events;

using System;
using System.Collections;

[Serializable]
public class MyEvent : UnityEvent<string, GameObject> { }

public class CollisionEventTrigger : MonoBehaviour
{
    public bool disableAfterCollision = true;
    public string myStr;
    public GameObject myGameObj;
    public MyEvent myEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);
        if (collision.tag == "Player")
        {
            myEvent.Invoke(myStr, myGameObj);
            // can be used to send dynamic stuff like .SendMessage()
            // use myEvent.Invoke("ApplyDamage", gameObject) and config to .SendMessage()
            // to run .SendMessage("ApplyDamage", gameObject) when invoked.
            if (disableAfterCollision)
                gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Collided with non-player object!");
        }
    }

    public void TestPrint(string s, GameObject g)
    {
        if (g != null)
            Debug.Log(g.name + " says: " + s);
    }

    public void InvokeEvent()
    {
        myEvent.Invoke(myStr, myGameObj);
    }
}