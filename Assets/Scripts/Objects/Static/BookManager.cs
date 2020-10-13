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
    private GameObject openedTextContainer;
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

    // TODO: disable player movement/enable in CloseBook
    public void OpenBook(TextObject textObj, GameObject textContainer)
    {
        if (openedBookText != null)
        {
            Debug.LogWarning("Tried opening book while book is already open! Tried opening: " + textObj.name);
            return;
        }
        openedBookText = textObj;
        bookDisplayPanel.SetActive(true);
        openedTextContainer = textContainer;
        openedTextContainer.SetActive(true);
        animator.SetBool("isOpen", true); // requires panel to be active :)

        //  animation stuff or delay here

        openedBookText.StartTyping();
    }

    public void OpenBook(string _, GameObject gameObj) // probably won't use this, its for event args
    {
        TextObject textObj = gameObj.GetComponent<TextObject>();
        if (textObj != null)
            OpenBook(textObj, gameObj);
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
        openedTextContainer.SetActive(false);

        openedBookText = null;
        openedTextContainer = null;
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
