using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class SpeedyCameraScript : MonoBehaviour
{
    private Animator cameraAnimator;
    public bool isActive;

    public float speedThreshold;
    private Rigidbody2D playerRB;

    public float releaseTimer = 0.3f;
    private float releaseTimerCount;
    private bool timerActive;
    void Start()
    {
        cameraAnimator = FindObjectOfType<CinemachineVirtualCamera>().gameObject.GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        // timerActive = false;
        isActive = false;
        timerActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(playerRB.velocity.magnitude);
        if (!isActive && playerRB.velocity.magnitude >= speedThreshold) 
        {
            cameraAnimator.Play("PlayerBoost");
            releaseTimerCount = releaseTimer;
            isActive = true;
        }
        if (isActive && playerRB.velocity.magnitude <= speedThreshold) 
        {
            timerActive = true;
        }
        if (timerActive) 
        {
            releaseTimerCount -= Time.deltaTime;
            if (releaseTimerCount < 0)
            {
                cameraAnimator.Play("PlayerSlow");
                timerActive = false;
                isActive = false;
            }
        }
    }
}
