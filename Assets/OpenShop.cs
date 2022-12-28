using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour, IInteractable
{
  public string GetDescription()
  {
    return "Open Shop";
  }

  public void Interact()
  {
    // either extend functionality here, or in player interaction from ur script
    Debug.Log("Make it snappy, I gotta take a craaaaaaap! haha 7dtd");

    // also pls hide waypoint when shop inactive :)
  }

  Items IInteractable.GetType()
  {
    return Items.Shop;
  }
}
