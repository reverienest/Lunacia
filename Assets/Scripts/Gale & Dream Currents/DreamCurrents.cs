using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class DreamCurrents : MonoBehaviour
{
    public string direction;
    private Animator cameraAnimator;
    private PlayerController player;
    private AreaEffector2D force;

    public float releaseTimer = 0.3f;
    private float releaseTimerCount;
    private bool timerActive;
    // Start is called before the first frame update
    void Start()
    {
        cameraAnimator = FindObjectOfType<CinemachineVirtualCamera>().gameObject.GetComponent<Animator>();
        force = GetComponent<AreaEffector2D>();
        if (direction.Equals("right")) {
            force.forceAngle = 0;
        } else if (direction.Equals("up")) {
            force.forceAngle = 90;
        } else if (direction.Equals("left")) {
            force.forceAngle = 180;
        } else if (direction.Equals("down")) {
            force.forceAngle = 270;
        }
        timerActive = false;
         
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive) {
            releaseTimerCount -= Time.deltaTime;
            if (releaseTimerCount < 0) {
                cameraAnimator.Play("PlayerSlow");
                timerActive = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            cameraAnimator.Play("PlayerBoost");
            releaseTimerCount = releaseTimer;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timerActive = true;
        }
    }
}
