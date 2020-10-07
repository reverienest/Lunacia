using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookObject : MonoBehaviour
{
    public BookManager bookManager;
    public TextObject textObject;

    void Start()
    {
        if (bookManager == null)
        {
            Debug.LogWarning("Book Manager is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
                
    }

    public void OpenBook()
    {
        bookManager.OpenBook(textObject);
    }

    public void CloseBook()
    {
        bookManager.CloseBook();
    }
}
