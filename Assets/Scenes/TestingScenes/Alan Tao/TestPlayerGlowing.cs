using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pubsub;

public class TestPlayerGlowing : MonoBehaviour
{
    float timer;
    private bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3.0f && !triggered)
        {
            print("player dead");
            MessageBroker.Instance.Raise(new PlayerDeathEventArguments(""));
            triggered = true;
        }
    }
}
