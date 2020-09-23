﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PauseMenuButtonController : MonoBehaviour
{
    //related to hover animations
    private int frame = 0;
    private string prefix = "";

    private bool onEntry = true, onExit = false, onHover;

    private int startingFrame = 3, staticFrame = 35, peakFrame = 59; //starting is set to 3 to avoid dim buttons
    private Sprite[] sprites;

    UnityEngine.UI.Image renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<UnityEngine.UI.Image>();

        //get sprite name prefix
        //ex. for "a_b_1", the prefix is "a_b_"
        int lastI = renderer.sprite.name.LastIndexOf("_");
        string prefix = renderer.sprite.name.Substring(0, lastI);
        //print(prefix);
        UnityEngine.Object[] spriteList = Resources.LoadAll(prefix);
        sprites = new Sprite[60];

        //the 0th element is the whole spritesheet and not the actual sprite
        //from 1 and on, the corresponding frame is (i - 1)th frame, which will be appended to
        //the actual list for actual sprites, instead of the object list
        for (int i = 1; i < spriteList.Length; i++)
        {
            sprites[i - 1] = spriteList[i] as Sprite;
        }

        //replace sprite with overrideSprite, the latter is controled dynamically by this script
        renderer.overrideSprite = renderer.sprite;

        
    }

    public void StartEntry()
    {
        //print("Start Entry called");
        onEntry = true;
        onExit = false;
        onHover = false;
        frame = startingFrame;
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
            renderer.sprite.name.Substring(
                renderer.overrideSprite.name.LastIndexOf("_")+1
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
            if (onHover)
            {
                if (frame < peakFrame)
                {
                    frame++;
                }
            }
            else
            {
                if(frame > staticFrame)
                {
                    frame--;
                }
            }
        }

        //print(frame + "onEntry:" + onEntry);
        //set sprite according to static frame number
        if (img_num != frame)
        {
            renderer.overrideSprite = sprites[frame];
        }
    }
}
