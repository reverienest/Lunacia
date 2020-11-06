using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pubsub;

public class BookObject : MonoBehaviour
{
    public BookManager bookManager;
    public GameObject normalTextContainer;
    public TextObject[] textObjectNormal;
    public GameObject WSTextContainer;
    public TextObject[] textObjectWS;

    private SpriteRenderer sprite;
    public Sprite spriteWSOn;
    public Sprite spriteWSOff;

    private bool isWSEnabled;

    void Start()
    {
        if (bookManager == null)
        {
            Debug.LogWarning("Book Manager is null!");
        }
        sprite = GetComponent<SpriteRenderer>();
        if (sprite == null)
        {
            Debug.LogWarning("Book Sprite is null!"); // consider making public sprite
        }

        MessageBroker.Instance.WakingSightModeTopic += consumeExampleMessage;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Fire2"))
        //{
        //    SetWakingSight(!isWSEnabled);
        //}
    }

    public void OpenBook()
    {
        if (isWSEnabled)
            bookManager.OpenBook(textObjectWS, WSTextContainer);
        else
            bookManager.OpenBook(textObjectNormal, normalTextContainer);
    }

    public void CloseBook()
    {
        bookManager.CloseScroll();
    }

    public void SetWakingSight(bool isEnabled)
    {
        isWSEnabled = isEnabled;
        if (isEnabled)
        {
            sprite.sprite = spriteWSOn;
        }
        else
        {
            sprite.sprite = spriteWSOff;
        }
    }

    private void consumeExampleMessage(object sender, WakingSightModeEventArgs example)
    {
        SetWakingSight(example.ActiveMode == 1);
    }
}
