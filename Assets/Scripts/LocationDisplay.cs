using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationDisplay : MonoBehaviour
{
    private string location = "Glade";
    public Text locationText;

    void Start()
    {
        setLocation("Intro Glade");
    }

    void setLocation(string place)
    {
        location = place;
        locationText.text = "Location: " + location;
    }
    
    void Update()
    {
    }
}
