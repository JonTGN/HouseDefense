using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponItemUI : MonoBehaviour
{
    [Header("Weapon Info")]
    public string name;
    public int price;

    [Header("Weapon Item")]
    public TextMeshProUGUI weaponNameLabel;

    [Header("Weapon Showcase")]
    public TextMeshProUGUI weaponNameDisplayLabel;
    //public TextMeshProUGUI weaponPriceLabel;

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
        updateDisplayLabels();
    }

    private void updateDisplayLabels()
    {
        weaponNameDisplayLabel.text = name;
        //weaponPriceLabel.text = $"${price}";

        DisplayWeapon();
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
