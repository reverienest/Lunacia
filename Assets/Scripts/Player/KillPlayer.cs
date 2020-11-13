using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pubsub;

public class KillPlayer : MonoBehaviour {
	public GameObject player;
	public GameObject playerBody;
	public Animator transition;
	public static Vector2 respawnLocation;
	public static bool hasRedFlame = false;
	public static bool hasBlueFlame = false;

	private Parallax ps;

	void Start() {
		MessageBroker.Instance.PlayerDeathTopic += ConsumePlayerDeathEvent;
		player.transform.position = respawnLocation;
		ps = FindObjectOfType<Parallax>();
		//added this so that the background spawns at respawn location
		// for (int i = 0; i < ps.backgrounds.Length; i++) {
		// 	ps.backgrounds[i].transform.position = new Vector3(
		// 		respawnLocation.x * (1 / ps.parallaxScalesX[i]),
		// 		respawnLocation.y * (1 / ps.parallaxScalesY[i]),
		// 		ps.backgrounds[i].position.z);
		// }
	}

	void ConsumePlayerDeathEvent(object sender, PlayerDeathEventArguments death) {
		print(death.deathMessage);
		player.GetComponent<PlayerController>().enabled = false;
		KillPlayer.hasRedFlame = false;
		KillPlayer.hasBlueFlame = false;
		StartCoroutine(waitForDeathAnimation());
	}

	void OnDestroy() {
		MessageBroker.Instance.PlayerDeathTopic -= ConsumePlayerDeathEvent;
	}
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Hazard") {
			MessageBroker.Instance.Raise(new PlayerDeathEventArguments("Player died!"));
		}
	}

	IEnumerator waitForDeathAnimation() {
		yield return new WaitForSeconds(1f);
		Destroy(playerBody);
		yield return new WaitForSeconds(0.5f);
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Debug.Log("Respawned at: " + respawnLocation);
	}

}
