using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pubsub;

public class KillPlayer : MonoBehaviour
{
    public GameObject player;
    public static Vector2 respawnLocation; 
    public static bool hasRedFlame = false;
    public static bool hasBlueFlame = false;
   
    void Awake() {
        MessageBroker.Instance.PlayerDeathTopic += consumePlayerDeathEvent;
        player.transform.position = respawnLocation;
    }
    static void consumePlayerDeathEvent(object sender, PlayerDeathEventArguments death) {
		print(death.deathMessage);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Respawned at: " + respawnLocation);
	}
    void OnDestroy() {
        MessageBroker.Instance.PlayerDeathTopic -= consumePlayerDeathEvent;
    }
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Hazard") {
            MessageBroker.Instance.Raise(new PlayerDeathEventArguments("Player died!"));
        }
    }

}
