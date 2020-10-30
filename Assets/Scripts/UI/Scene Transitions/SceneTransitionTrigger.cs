using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pubsub;

public class SceneTransitionTrigger : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.0f;
    public Object loadingScreen;
    public Object nextScene;
    public Vector3 newSceneCoordinates;
    
    IEnumerator LoadLevel() 
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        LevelLoader.nextSceneName = nextScene.name;
        KillPlayer.respawnLocation = newSceneCoordinates;
        MessageBroker.Instance.Raise(new SceneTransitionEventArgs("Transitioning to new scene: " + nextScene.name));
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            StartCoroutine(LoadLevel());
        }
    }

}
