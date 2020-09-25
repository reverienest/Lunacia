using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakingSightManager : MonoBehaviour
{
    
    public Image myImageComponent;
    public Sprite WSoff;
    public Sprite WSon;
    public WakingSight wakingSight;

    public void SetImage(Sprite x)
    {
        this.GetComponent<Image>().sprite = x;
    }
 
    // Update is called once per frame
    void Update()
    {
        if (wakingSight.activeMode == 0)
        {
            SetImage(WSoff);
        }
        if (wakingSight.activeMode == 1)
        {
            SetImage(WSon);
        }
    }
}

