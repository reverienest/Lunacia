using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterTrigger : MonoBehaviour
{
    [SerializeField]
    private string param1Name;
    [SerializeField]
    private float param1Val;
    [SerializeField]
    private string param2Name;
    [SerializeField]
    private float param2Val;
    [SerializeField]
    private GameObject fManager;
    private FMODUnity.StudioEventEmitter emitter;


    // Start is called before the first frame update
    void Start()
    {
        fManager = GameObject.Find("TrackManager");
        emitter = fManager.GetComponent<FMODUnity.StudioEventEmitter>();

        if (fManager)
        {
            Debug.Log(fManager.name);
        }
        else
        {
            Debug.Log("No TrackManager found!");
        }

        if (emitter == null)
        {
            Debug.LogWarning("No fmod emitter found, audio will not play.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SetParameter(emitter.EventInstance, param1Name, param1Val);
            SetParameter(emitter.EventInstance, param2Name, param2Val);
        }
    }
    void SetParameter(FMOD.Studio.EventInstance e, string name, float value)
    {
        e.setParameterByName(name, value);
    }
}
