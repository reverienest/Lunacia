using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool paused = false;
    private GameObject pauseShade;
    private Animator animator;
    public string mainMenuSceneName;

    // Start is called before the first frame update
    void Start()
    {
        // get the shade child component
    	pauseShade = gameObject.transform.Find("Shade").gameObject;
        // deactivate the shade on game start
        pauseShade.SetActive(false);
        // setup animator
        animator = pauseShade.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause() 
    {
        if (paused)
        {
            StartCoroutine(IResume());
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            pauseShade.SetActive(true);
            paused = true;
        }
    }

    private IEnumerator IResume()
    {
        animator.Play("Exit");
        // wait until anim stops - use actual time outside of time.timeScale
        yield return StartCoroutine(IWaitForRealSeconds(0.55f));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        pauseShade.SetActive(false);
        paused = false;
    }

    public void Click_Resume()
    {
        TogglePause();
    }

    public void Click_Options()
    {
        // TODO: implement
    }

    // load main menu scene (title screen)
    public void Click_MainMenu()
    {
        Debug.Log("Quit to Main Menu! Scene \"" + mainMenuSceneName + "\" now loading...");
        TogglePause();
        if (!mainMenuSceneName.Equals("")) {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    private static IEnumerator IWaitForRealSeconds(float seconds)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + seconds)
        {
            yield return null;
        }
    }
}