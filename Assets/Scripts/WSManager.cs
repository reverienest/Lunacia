using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSManager : MonoBehaviour
{
    public Sprite WSoff;
    public Sprite WSon;

    //Sprite spriteoff = Resources.Load<Sprite>("WSoff"); idk if this will work better?
    //Sprite spriteon = Resources.Load<Sprite>("WSon");

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = WSon;
            //alphaLevel += .5f;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = WSoff;
            //alphaLevel -= .5f;
        }
        //GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alphaLevel);
    }
}
