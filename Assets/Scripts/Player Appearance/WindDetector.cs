using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindDetector : MonoBehaviour
{
    [HideInInspector]
    public Vector2 wind;

    // Start is called before the first frame update
    void Start()
    {
        wind = new Vector2(0.0f, 0.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DreamCurrents dc = other.GetComponent<DreamCurrents>();
        if (dc != null)
        {
            //the collided object is a dream current
            AreaEffector2D ae2d = other.GetComponent<AreaEffector2D>();
            float ang = (ae2d.forceAngle / 180.0f * Mathf.PI);
            wind += ae2d.forceMagnitude * new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        DreamCurrents dc = other.GetComponent<DreamCurrents>();
        if (dc != null)
        {
            //the collided object is a dream current
            AreaEffector2D ae2d = other.GetComponent<AreaEffector2D>();
            float ang = (ae2d.forceAngle / 180.0f * Mathf.PI);
            wind -= ae2d.forceMagnitude * new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
