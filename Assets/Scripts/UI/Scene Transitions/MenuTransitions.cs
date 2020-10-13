using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransitions : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.0f;
    public Object nextScene;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            StartCoroutine(LoadLevel());
        }
    }
    IEnumerator LoadLevel() 
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(nextScene.name);
    }
}