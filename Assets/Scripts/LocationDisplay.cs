using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationDisplay : MonoBehaviour
{
    private string location = "Glade";
    public Text locationText;

    void setLocation(string place)
    {
        location = place;
    }
    
    void Update()
    {
        locationText.text = "Location: " + location;
    }
}
