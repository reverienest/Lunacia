using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pubsub;

public class KillPlayer : MonoBehaviour
{
    GameObject player;
    GameObject[] hazards;

    static KillPlayer()
    {
        MessageBroker.Instance.playerDeath += consumePlayerDeathEvent;
    }

    static void consumePlayerDeathEvent(object sender, PlayerDeathEvent death) {
		print(death.deathMessage);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hazards = GameObject.FindGameObjectsWithTag("Hazard");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Hazard")
        {
            MessageBroker.Instance.Raise(new PlayerDeathEvent("Player died!"));
        }
    }

}
