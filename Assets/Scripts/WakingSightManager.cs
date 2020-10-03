using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pubsub;

public class WakingSightManager : MonoBehaviour
{
    
    public Image myImageComponent;
    public Sprite WSoff;
    public Sprite WSon;

    void Start()
    {
        MessageBroker.Instance.WakingSightModeTopic += consumeExampleMessage;
    }
    private void consumeExampleMessage(object sender, WakingSightModeEventArgs example)
    {
        if (example.ActiveMode == 0)
        {
            SetImage(WSoff);
        }
        if (example.ActiveMode == 1)
        {
            SetImage(WSon);
        }
    }
    public void SetImage(Sprite newSprite)
    {
        this.GetComponent<Image>().sprite = newSprite;
    }
 
    // Update is called once per frame
    void Update()
    {
    }
}

