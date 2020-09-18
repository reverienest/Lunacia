using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Pubsub;

public class KillPlayer : MonoBehaviour
{
    GameObject player;
    GameObject[] hazards;
    Scene currentScene;

    bool sceneSwitched = false;
    
    void Awake() {
        MessageBroker.Instance.playerDeath += consumePlayerDeathEvent;
    }

    private void consumePlayerDeathEvent(object sender, PlayerDeathEvent death) {
		print(death.deathMessage);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        sceneSwitched = true;
	}

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hazards = GameObject.FindGameObjectsWithTag("Hazard");
        currentScene = SceneManager.GetActiveScene();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Hazard" && !sceneSwitched)
        {
            MessageBroker.Instance.Raise(new PlayerDeathEvent("Player died!"));
        }
    }

}
