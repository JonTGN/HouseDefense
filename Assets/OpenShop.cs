using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour, IInteractable
{
    public GameObject ShopUI;

  public string GetDescription()
  {
    return "Press E to Open Shops";
  }

  public void Interact()
  {
    // either extend functionality here, or in player interaction from ur script
    Debug.Log("OpenShop");

    // also pls hide waypoint when shop inactive :)
  }

  Items IInteractable.GetType()
  {
    return Items.Shop;
  }
}
