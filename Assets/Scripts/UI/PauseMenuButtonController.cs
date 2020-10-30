using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PauseMenuButtonController : MonoBehaviour
{
    //related to hover animations
    private int frame = 0;

    private bool onEntry = true, onExit = false, onHover;

    private int dormantFrame = -20, startingFrame = 3, staticFrame = 35, peakFrame = 59; 
        //starting is set to 3 to avoid dim buttons
        //dormant is set to negative to stop frame progression when entry just began (i.e. the sprites are still transparent)

    private Sprite[] sprites;

    private Transform text;
    public double textSizeMin, textSizeMax;

    UnityEngine.UI.Image renderer_;

    // Start is called before the first frame update
    void Start()
    {
        renderer_ = GetComponent<UnityEngine.UI.Image>();

        //get sprite name prefix
        //ex. for "a_b_1", the prefix is "a_b_"
        int lastI = renderer_.sprite.name.LastIndexOf("_");
        string prefix = renderer_.sprite.name.Substring(0, lastI);
        //print(prefix);
        UnityEngine.Object[] spriteList = Resources.LoadAll(prefix);
        sprites = new Sprite[60];

        //the 0th element is the whole spritesheet and not the actual sprite
        //from 1 and on, the corresponding frame is (i - 1)th frame, which will be appended to
        //the actual list for actual sprites, instead of the object list
        
        //the resources must be sort in order manually because unity doesn't
        //always import them by their naming order
        for (int i = 1; i < spriteList.Length; i++)
        {
            int true_index = Int32.Parse(spriteList[i].name.Substring(
                spriteList[i].name.LastIndexOf("_")+1
            ));
            sprites[true_index] = spriteList[i] as Sprite;
        }

        //replace sprite with overrideSprite, the latter is controled dynamically by this script
        renderer_.overrideSprite = renderer_.sprite;

        frame = dormantFrame;

        text = transform.Find("Text");
    }

    public void StartEntry()
    {
        //print("Start Entry called");
        onEntry = true;
        onExit = false;
        onHover = false;
        frame = dormantFrame;
    }
    public void StartExit()
    {
        //print("exit called");
        onExit = true;
    }

    public void StartHover()
    {
        onHover = true;
    }

    public void EndHover()
    {
        onHover = false;
    }

    // Update is called once per frame
    void Update()
    {
        //print(hovering);
        int img_num = Int16.Parse(
            renderer_.sprite.name.Substring(
                renderer_.overrideSprite.name.LastIndexOf("_")+1
            )
        ); //take the last item in the name (the number)

        //entry animation
        if (onEntry)
        {
            if (frame < staticFrame)
            {
                frame++;
            }
            else
            {
                onEntry = false;
            }
        }
        //exit animation
        else if (onExit)
        {
            if (frame > startingFrame)
            {
                frame--;
            }
            else
            {
                onEntry = false;
            }
        }
        else //not on any major animations
        {
            //hover animation
            if (onHover)
            {
                if (frame < peakFrame)
                    frame++;
            }
            else
            {
                if(frame > staticFrame)
                    frame--;
            }
            //scale button text according to sprite progression
            text.GetComponent<UnityEngine.UI.Text>().fontSize =
                (int)(
                    (textSizeMax - textSizeMin) / (peakFrame * 1.0 - staticFrame)
                    * (frame - staticFrame) + textSizeMin
                );
        }

        //set sprite according to static frame number
        if (img_num != frame)
        {
            if (frame < startingFrame)
            {
                renderer_.overrideSprite = sprites[startingFrame];
            }
            else
            {
                renderer_.overrideSprite = sprites[frame];
            }
        }

    }
}
