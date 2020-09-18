using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // store collected in bool[]?
    public int strawberryCount; // temp
    public int book1Count;
    public int book2Count;

    private List<GameObject> inactiveItems = new List<GameObject>();

    void Start()
    {
        
    }

    public void CollectItem(string itemName, GameObject item)
    {
        Debug.Log("Collected " + itemName + "!");
        switch (itemName)
        {
            case "Strawberry":
                strawberryCount += 1;
                break;
            case "Book1":
                book1Count += 1;
                break;
            case "Book2":
                book2Count += 1;
                break;
        }
        inactiveItems.Add(item);
        item.SetActive(false);
    }
}
