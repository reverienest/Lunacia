using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaigaBlockBarrier : MonoBehaviour
{
    // dirty fix that prevents player from going through flame barriers in first part of taiga until waking sight upgrade is picked up
    
    public GameObject barrier;
    public GameObject priorFlameBarrier;
    public GameObject afterFlameBarrier;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            Destroy(barrier);
            priorFlameBarrier.SetActive(false);
            afterFlameBarrier.SetActive(true);
        }
    }
}
