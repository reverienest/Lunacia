using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDreamDraft : MonoBehaviour
{

    private Rigidbody2D playerRB;
    private TestPlayer player;
    
    [SerializeField]
    private float speedBoost = 20.0f;

    private float prevSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == "Player")
        {
            playerRB = entity.gameObject.GetComponent<Rigidbody2D>();
            player = entity.gameObject.GetComponent<TestPlayer>();
            prevSpeed = player.maxSpeed;
        }
    }

    private void OnTriggerStay2D(Collider2D entity)
    {
        player.maxSpeed = speedBoost;
    }

    private void OnTriggerExit2D(Collider2D entity)
    {
        player.maxSpeed = prevSpeed;
    }
}
