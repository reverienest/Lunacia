using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool paused = false;
    private GameObject pauseShade;
    private Animator animator;
    public string mainMenuSceneName;
    // animatable representation of the current time scale
    public Slider timeSlider;

    // Start is called before the first frame update
    void Start()
    {
        // get the shade child component
    	pauseShade = gameObject.transform.Find("Shade").gameObject;
    	timeSlider = GetComponentInChildren<Slider>();
        // deactivate the shade on game start
        pauseShade.SetActive(false);
        // setup animator
        animator = pauseShade.GetComponent<Animator>();
        // set "timeScale" to 1 at start
        timeSlider.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        Time.timeScale = timeSlider.value;
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
            pauseShade.SetActive(true);
            paused = true;
        }
    }

    private IEnumerator IResume()
    {
        animator.Play("Exit");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // wait until anim stops - use real time outside of time.timeScale
        yield return StartCoroutine(IWaitForRealSeconds(0.8f));
        // reset time scale to account for rounding errors
        Time.timeScale = 1f; 
        timeSlider.value = 1f;
        pauseShade.SetActive(false);
        paused = false;
    }

    public void Click_Resume()
    {
        TogglePause();
    }

    public void Click_Options()
    {
        Time.timeScale = 1f;
        // implement options menu
    }

    // load main menu scene (title screen)
    public void Click_MainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Quit to Main Menu! Scene \"" + mainMenuSceneName + "\" now loading...");
        TogglePause();
        if (!mainMenuSceneName.Equals("")) 
        {
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