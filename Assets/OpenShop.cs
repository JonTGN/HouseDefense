using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenShop : MonoBehaviour, IInteractable
{
    public GameObject WeaponHolder;
    public GameObject ShopUI;

    [Header("Controls")]
    public PlayerInput input;

    public string GetDescription()
    {
          return "Press E to Open Shop";
    }

    public void Interact()
    {
        // either extend functionality here, or in player interaction from ur script
        Debug.Log("OpenShop");
        ShopUI.SetActive(true);
        input.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // also pls hide waypoint when shop inactive :)
    }

    Items IInteractable.GetType()
    {
          return Items.Shop;
    }
}
