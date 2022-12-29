using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponItemUI : MonoBehaviour
{
    [Header("Weapon Info")]
    public string name;
    public int price;

    [Header("Weapon Item")]
    public TextMeshProUGUI weaponNameLabel;

    [Header("Buttons")]
    public Button BuyButton;
    public Button SellButton;

    [Header("Weapon Stats")]
    [Range(0, 7)]
    public int damage;
    [Range(0, 7)]
    public int ammo;
    [Range(0, 7)]
    public int range;
    [Range(0, 7)]
    public int weight;

    
    private void Awake()
    {
        weaponNameLabel.text = name;

        // Test
        //DisplayWeapon();
    }

    public void WeaponSelected()
    {
        Debug.Log("button selected");
        DisplayWeapon();
        //Turn on my buttons after all turned off
        BuyButton.interactable = true;
        SellButton.interactable = true;
        Debug.Log("Buttons Enabled");
    }

    public void DisplayWeapon()
    {
        try
        {
            Shopkeeper keeper = GameObject.Find("ShopUI").GetComponent<Shopkeeper>();

            keeper.NewItemToDisplay(this);

        }
        catch (System.Exception)
        {

            Debug.Log("ERROR: NO KEEPER FOUND IN SCENE");
        }
    }

    public void BuyWeapon()
    {
        try
        {
            Shopkeeper keeper = GameObject.Find("ShopUI").GetComponent<Shopkeeper>();

            keeper.BuyItem(this);

        }
        catch (System.Exception)
        {

            Debug.Log("ERROR: NO KEEPER FOUND IN SCENE");
        }
    }

    public void SellWeapon()
    {
        try
        {
            Shopkeeper keeper = GameObject.Find("ShopUI").GetComponent<Shopkeeper>();

            keeper.SellItem(this);

        }
        catch (System.Exception)
        {

            Debug.Log("ERROR: NO KEEPER FOUND IN SCENE");
        }
    }
}
