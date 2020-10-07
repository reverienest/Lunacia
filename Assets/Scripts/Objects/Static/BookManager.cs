using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    [Header("Books")]
    public TextObject[] books;

    [Header("Settings")]
    public textSpeedEnum textSpeedEditor;
    public enum textSpeedEnum { Fast, Medium, Slow };
    public float fastSpeed = 0f;
    public float mediumSpeed = 0.025f;
    public float slowSpeed = 0.05f;

    public GameObject bookDisplayPanel;
    private TextObject openedBookText;
    public Animator animator;

    void Start()
    {
        foreach (TextObject o in books)
        {
            UpdateTextSpeed(o);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))// || Input.GetButtonDown("Fire1"))
        {
            if (openedBookText != null && !openedBookText.TrySkipText())
            {
                CloseBook();
            }
        }
    }

    public void OpenBook(TextObject textObj)
    {
        if (openedBookText != null)
        {
            Debug.LogWarning("Tried opening book while book is already open!");
            return;
        }
        openedBookText = textObj;
        bookDisplayPanel.SetActive(true);
        openedBookText.textContainer.SetActive(true);
        animator.SetBool("isOpen", true); // requires panel to be active :)

        //  animation stuff or delay here

        openedBookText.StartTyping();
    }

    public void OpenBook(string _, GameObject gameObj)
    {
        TextObject textObj = gameObj.GetComponent<TextObject>();
        if (textObj != null)
            OpenBook(textObj);
        else
            Debug.LogWarning("Tried opening book when text object does not exist!");
    }

    public void CloseBook()
    {
        animator.SetBool("isOpen", false);
        // DisableBook();
    }

    public void DisableBook()
    {
        bookDisplayPanel.SetActive(false);
        openedBookText.textContainer.SetActive(false);

        openedBookText = null;
    }

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
