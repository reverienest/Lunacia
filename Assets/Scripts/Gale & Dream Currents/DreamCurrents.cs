using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class DreamCurrents : MonoBehaviour
{
    public float ForceMagnitude;
    private AreaEffector2D force;

    private Transform position;

    // Start is called before the first frame update
    void Start()
    {
        position = GetComponent<Transform>();
        force = GetComponent<AreaEffector2D>();
        force.forceMagnitude = ForceMagnitude;
        force.forceAngle = position.rotation.eulerAngles.z;
    }
}
