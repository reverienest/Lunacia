using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pubsub;

public class TestBlackholeRespondance : MonoBehaviour
{
    private float timer = 0.0f;
    private bool broadcasted = false, broadcasted2 = false;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (!broadcasted && timer > 5.0f)
        {
            print("waking sight on");
            MessageBroker.Instance.Raise(new WakingSightModeEventArgs(1));
            broadcasted = true;
        }
        if (!broadcasted2 && timer > 10.0f)
        {
            print("waking sight off");
            MessageBroker.Instance.Raise(new WakingSightModeEventArgs(0));
            broadcasted2 = true;
        }
    }

}