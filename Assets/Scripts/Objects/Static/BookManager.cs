using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("TextAnimation")]
    public AnimationCurve alphaSpectrum = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    public AnimationCurve colorSpectrum = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    public Color visibleColor;
    public Color fadeOutColor;

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

        //  disable player movement here

        currentPage = 0;
        //EnableCurrentPage(); // -> called in animation
        StartCoroutine(EnablePageAfterDelay(2));
    }

    public void CloseBook()
    {
        bookAnimator.SetBool("isOpen", false);
        //  enable player movement here
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
        if (openedPages[currentPage].finishedTyping)
            StartCoroutine(FadeTextOutIn(false));
        else
            SetTextDefault(); // temporary
        EnableCurrentPage();
    }

    private IEnumerator DisablePageAfterDelay(float seconds)
    {
        int cp = currentPage;
        yield return new WaitForSeconds(seconds);
        openedPages[cp].gameObject.SetActive(false);
        if (cp + 1 < openedPages.Length)
            openedPages[cp + 1].gameObject.SetActive(false);
    }

    private IEnumerator TriggerAnimatorAfterDelay(string val, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        bookAnimator.SetTrigger(val);
    }

    private IEnumerator FadeTextOutIn(bool fadeTextOut)
    {
        bool hasSecondPage = currentPage + 1 < openedPages.Length;
        TextMeshProUGUI firstPage = openedPages[currentPage].GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI secondPage = hasSecondPage ? openedPages[currentPage + 1].GetComponent<TextMeshProUGUI>() : null;
        Color c;

        float t = 0;
        float endTime = alphaSpectrum.keys[1].time;
        if (!fadeTextOut)
        {
            t = alphaSpectrum.keys[1].time;
            endTime = 0;
        }

        while ((fadeTextOut && t < endTime) || (!fadeTextOut && t > endTime))
        {
            if (fadeTextOut)
                t += Time.deltaTime;
            else
                t -= Time.deltaTime;

            c = visibleColor * colorSpectrum.Evaluate(t) + fadeOutColor * (1 - colorSpectrum.Evaluate(t));
            c.a = alphaSpectrum.Evaluate(t);
            firstPage.color = c;
            if (hasSecondPage)
                secondPage.color = c;

            yield return null;
        }

        if (fadeTextOut)
            t = endTime;
        else
            t = 0;

        c = visibleColor * colorSpectrum.Evaluate(t) + fadeOutColor * (1 - colorSpectrum.Evaluate(t));
        c.a = alphaSpectrum.Evaluate(t);
        firstPage.color = c;
        if (hasSecondPage)
            secondPage.color = c;
    }

    private void SetTextDefault()
    {
        bool hasSecondPage = currentPage + 1 < openedPages.Length;
        TextMeshProUGUI firstPage = openedPages[currentPage].GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI secondPage = hasSecondPage ? openedPages[currentPage + 1].GetComponent<TextMeshProUGUI>() : null;

        Color c = visibleColor * colorSpectrum.Evaluate(0) + fadeOutColor * (1 - colorSpectrum.Evaluate(0));
        c.a = alphaSpectrum.Evaluate(0);
        firstPage.color = c;
        if (hasSecondPage)
            secondPage.color = c;
    }

    public void TurnPageLeft()
    {
        if (currentPage <= 0)
        {
            return;
        }
        //DisableCurrentPage();
        StartCoroutine(DisablePageAfterDelay(alphaSpectrum[1].time));
        StartCoroutine(FadeTextOutIn(true));
        currentPage -= 2;
        SetTextDefault();
        //bookAnimator.SetTrigger("turnLeft");
        StartCoroutine(TriggerAnimatorAfterDelay("turnLeft", .2f));
        //EnableCurrentPage();
        StartCoroutine(EnablePageAfterDelay(1f));
    }

    public void TurnPageRight()
    {
        int maxPages = openedPages.Length % 2 == 1 ? openedPages.Length - 1 : openedPages.Length - 2;
        if (currentPage >= maxPages)
        {
            return;
        }
        //DisableCurrentPage();
        StartCoroutine(DisablePageAfterDelay(alphaSpectrum[1].time));
        StartCoroutine(FadeTextOutIn(true));
        currentPage += 2;
        //bookAnimator.SetTrigger("turnRight");
        StartCoroutine(TriggerAnimatorAfterDelay("turnRight", .2f));
        //EnableCurrentPage();
        StartCoroutine(EnablePageAfterDelay(1f));
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
