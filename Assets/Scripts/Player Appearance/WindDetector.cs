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
            wind += dc.direction * dc.force;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        DreamCurrents dc = other.GetComponent<DreamCurrents>();
        if (dc != null)
        {
            wind -= dc.direction * dc.force;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
