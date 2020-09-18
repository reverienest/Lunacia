using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string CollectableType;
    // Strawberry, Book1, Book2;
    //public GameObject parentCollectable;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);
        if (collision.tag == "Player")
        {
            PlayerInventory pInv = collision.gameObject.GetComponent<PlayerInventory>();
            if (pInv != null)
                Collect(pInv);
            else
                Debug.LogWarning("Player doesn't have a PlayerInventory script attatched");
        }
    }

    private void Collect(PlayerInventory playerInv)
    {
        playerInv.CollectItem(CollectableType, gameObject);

        //if (parentCollectable == null)
        //    Destroy(gameObject);
        //else
        //    Destroy(parentCollectable);
    }
}
