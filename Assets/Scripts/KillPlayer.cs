using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pubsub;

public class KillPlayer : MonoBehaviour
{
    public GameObject player;

    private static bool addedMessageBroker = false;

    void Awake() {
        if (!addedMessageBroker) {
            MessageBroker.Instance.playerDeath += consumePlayerDeathEvent;
            addedMessageBroker = true;
        }
    }

    static void consumePlayerDeathEvent(object sender, PlayerDeathEventArguments death) {
		print(death.deathMessage);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Hazard")
        {
            MessageBroker.Instance.Raise(new PlayerDeathEventArguments("Player died!"));
        }
    }

}
