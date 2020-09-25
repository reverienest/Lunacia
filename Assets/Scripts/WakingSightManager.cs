using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakingSightManager : MonoBehaviour
{
    // Drag image component here in the inspector window
    public Image myImageComponent;

    // Drag first sprite here in the inspector window
    public Sprite WSoff;

    // Drag second sprite here in the inspector window
    public Sprite WSon;

    public void SetImage(Sprite x)
    {
        this.GetComponent<Image>().sprite = x;
    }
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetImage(WSoff);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetImage(WSon);
        }
    }
}

