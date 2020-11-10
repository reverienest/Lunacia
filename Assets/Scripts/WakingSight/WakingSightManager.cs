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

    [SerializeField]
    private FMODUnity.StudioEventEmitter emitter;
    FMOD.Studio.PARAMETER_ID filterParameterId;

    void Start()
    {
        MessageBroker.Instance.WakingSightModeTopic += consumeExampleMessage;
    }
    private void consumeExampleMessage(object sender, WakingSightModeEventArgs example)
    {
        if (example.ActiveMode == 0)
        {
            SetImage(WSoff);
            SetParameter(emitter.EventInstance, "Waking Sight", 0.0f);

        }
        if (example.ActiveMode == 1)
        {
            SetImage(WSon);
            SetParameter(emitter.EventInstance, "Waking Sight", 1.0f);
        }
    }
    public void SetImage(Sprite newSprite)
    {
        this.GetComponent<Image>().sprite = newSprite;
    }

    void SetParameter(FMOD.Studio.EventInstance e, string name, float value)
    {
        e.setParameterByName(name, value);
    }

    // Update is called once per frame
    void Update()
    {
    }
}

