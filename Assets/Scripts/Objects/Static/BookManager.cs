using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    [Header("TextObjects")]
    public TextObject[] textObjs; // for updating text speed

    [Header("Settings")]
    public textSpeedEnum textSpeedEditor;
    public enum textSpeedEnum { Fast, Medium, Slow };
    public float fastSpeed = 0f;
    public float mediumSpeed = 0.025f;
    public float slowSpeed = 0.05f;

    [Header("References")]
    public GameObject bookDisplayPanel;
    private GameObject openedBookTextContainer;
    private TextObject[] openedPages;

    public GameObject scrollDisplayPanel;
    private TextObject openedScrollText;
    private GameObject openedScrollTextContainer;


    public Animator bookAnimator;
    public Animator scrollAnimator;

    // book stuff
    private int currentPage = 0; // 0, 2, 4, etc., index starts at 0
    private IEnumerator typeNextPageRoutine;

    void Start()
    {
        foreach (TextObject o in textObjs)
        {
            UpdateTextSpeed(o);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // book
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("Fire1"))
        {
            if (openedPages != null && openedPages[currentPage].gameObject.activeInHierarchy)
            {
                TurnPageLeft();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("Fire2"))
        {
            if (openedPages != null && openedPages[currentPage].gameObject.activeInHierarchy)
            {
                TurnNextPage();
            }
        }
        // scroll
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("Fire1"))
        {
            if (openedScrollText != null && !openedScrollText.TrySkipText())
            {
                CloseScroll();
            }
        }
    }

    #region Book
    // TODO: disable player movement/enable in CloseScroll
    public void OpenBook(TextObject[] pages, GameObject textContainer)
    {
        if (openedScrollText != null)
        {
            Debug.LogWarning("Tried opening scroll while scroll is already open! Tried opening: " + textContainer.name);
            return;
        }
        openedPages = pages;
        bookDisplayPanel.SetActive(true);
        openedBookTextContainer = textContainer;
        openedBookTextContainer.SetActive(true);
        bookAnimator.SetBool("isOpen", true); // requires panel to be active :)

        //  animation stuff or delay here

        currentPage = 0;
        //EnableCurrentPage(); // -> called in animation
        StartCoroutine(EnablePageAfterDelay(2));
    }

    public void CloseBook()
    {
        bookAnimator.SetBool("isOpen", false);
        // DisableBook();
    }

    public void DisableBook()
    {
        scrollDisplayPanel.SetActive(false);
        openedBookTextContainer.SetActive(false);

        openedPages = null;
        openedBookTextContainer = null;
    }


    private IEnumerator EnablePageAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        EnableCurrentPage();
    }

    public void TurnPageLeft()
    {
        if (currentPage <= 0)
        {
            return;
        }
        DisableCurrentPage();
        currentPage -= 2;
        bookAnimator.SetTrigger("turnLeft");
        //EnableCurrentPage();
        StartCoroutine(EnablePageAfterDelay(1.2f));
    }

    public void TurnPageRight()
    {
        int maxPages = openedPages.Length % 2 == 1 ? openedPages.Length - 1 : openedPages.Length - 2;
        if (currentPage >= maxPages)
        {
            return;
        }
        DisableCurrentPage();
        currentPage += 2;
        bookAnimator.SetTrigger("turnRight");
        //EnableCurrentPage();
        StartCoroutine(EnablePageAfterDelay(1.2f));
    }

    public void TurnNextPage()
    {
        // case: odd # pages
        if (currentPage + 1 >= openedPages.Length)
        {
            // odd pages and done
            if (openedPages[currentPage].finishedTyping)
            {
                CloseBook();
            }
            // odd pages not done
            else
            {
                openedPages[currentPage].TrySkipText();
            }
        }
        // case: even pages
        else
        {
            // even pages and done
            if (openedPages[currentPage + 1].finishedTyping)
            {
                // check if book is done
                if (currentPage + 2 >= openedPages.Length)
                {
                    CloseBook();
                }
                else
                {
                    TurnPageRight();
                }
            }
            // even pages not done
            else
            {
                openedPages[currentPage].TrySkipText();
                openedPages[currentPage + 1].StartTyping(); // comment this out to skip 1 page at a time instead of 2
                openedPages[currentPage + 1].TrySkipText();
            }
        }
    }

    private IEnumerator TypeNextPageRoutine()
    {
        // should end coroutine in case of null pointer maybe
        if (currentPage + 1 >= openedPages.Length)
        {
            yield break;
        }
        while (!openedPages[currentPage].finishedTyping)
        {
            yield return null;
        }
        openedPages[currentPage + 1].StartTyping();
        typeNextPageRoutine = null;
    }

    private void DisableCurrentPage()
    {
        openedPages[currentPage].gameObject.SetActive(false);
        if (currentPage + 1 < openedPages.Length)
            openedPages[currentPage + 1].gameObject.SetActive(false);
    }

    public void EnableCurrentPage()
    {
        openedPages[currentPage].gameObject.SetActive(true);
        openedPages[currentPage].StartTyping();
        if (currentPage + 1 < openedPages.Length)
        {
            openedPages[currentPage + 1].gameObject.SetActive(true);
            typeNextPageRoutine = TypeNextPageRoutine();
            StartCoroutine(typeNextPageRoutine);
        }
    }
    #endregion

    #region Scroll
    // TODO: disable player movement/enable in CloseScroll
    public void OpenScroll(TextObject textObj, GameObject textContainer)
    {
        if (openedScrollText != null)
        {
            Debug.LogWarning("Tried opening scroll while scroll is already open! Tried opening: " + textObj.name);
            return;
        }
        openedScrollText = textObj;
        openedScrollText.gameObject.SetActive(true);
        scrollDisplayPanel.SetActive(true);
        openedScrollTextContainer = textContainer;
        openedScrollTextContainer.SetActive(true);
        scrollAnimator.SetBool("isOpen", true); // requires panel to be active :)

        //  animation stuff or delay here

        openedScrollText.StartTyping();
    }

    public void OpenScroll(string _, GameObject gameObj) // probably won't use this, its for event args
    {
        TextObject textObj = gameObj.GetComponent<TextObject>();
        if (textObj != null)
            OpenScroll(textObj, gameObj);
        else
            Debug.LogWarning("Tried opening scroll when text object does not exist!");
    }

    public void CloseScroll()
    {
        scrollAnimator.SetBool("isOpen", false);
        // DisableBook();
    }

    public void DisableScroll()
    {
        scrollDisplayPanel.SetActive(false);
        openedScrollTextContainer.SetActive(false);

        openedScrollText = null;
        openedScrollTextContainer = null;
    }
    #endregion

    public void UpdateTextSpeed(TextObject textObj)
    {
        switch (textSpeedEditor)
        {
            case textSpeedEnum.Fast:
                textObj.UpdateTextSpeed(fastSpeed);
                break;
            case textSpeedEnum.Medium:
                textObj.UpdateTextSpeed(mediumSpeed);
                break;
            case textSpeedEnum.Slow:
                textObj.UpdateTextSpeed(slowSpeed);
                break;
        }
    }
}
