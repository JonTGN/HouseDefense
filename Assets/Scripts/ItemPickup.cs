using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public string description;
    public Items itemType;

    public string GetDescription()
    {
        return description;
    }

    public void Interact()
    {
        // play pickup sound 

        gameObject.SetActive(false);


    }

    public Items GetType()
    {
        return itemType;
    }
}
