using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    public Vector2 respawnPoint;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        KillPlayer.respawnLocation = respawnPoint;
        Debug.Log("Respawn location changed to " + respawnPoint);
    }
}
