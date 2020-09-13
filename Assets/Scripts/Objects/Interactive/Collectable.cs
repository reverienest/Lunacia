using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string CollectableType;
    public GameObject parentCollectable;
    // Strawberry, Book1, Book2;

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
        playerInv.CollectItem(CollectableType);

        if (parentCollectable == null)
            Destroy(gameObject);
        else
            Destroy(parentCollectable);
    }
}
