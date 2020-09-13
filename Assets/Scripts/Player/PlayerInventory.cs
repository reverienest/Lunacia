using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // store collected in bool[]?
    public int strawberryCount; // temp
    public int book1Count;
    public int book2Count;

    void Start()
    {
        
    }

    public void CollectItem(string item)
    {
        Debug.Log("Collected " + item + "!");
        switch (item)
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
    }
}
