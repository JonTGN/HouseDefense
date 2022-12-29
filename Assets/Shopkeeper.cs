using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shopkeeper : MonoBehaviour
{
    [Header("Weapon StatBlocks")]
    public int Wallet;
    [Range(0, 7)]
    public int damage;
    [Range(0, 7)]
    public int ammo;
    [Range(0, 7)]
    public int range;
    [Range(0, 7)]
    public int weight;


    [Header("GameObjects List")]
    public TextMeshProUGUI walletLabel;
    public List<GameObject> damageBlocks = new List<GameObject>();
    public List<GameObject> ammoBlocks = new List<GameObject>();
    public List<GameObject> rangeBlocks = new List<GameObject>();
    public List<GameObject> weightBlocks = new List<GameObject>();

    public List<GameObject> inventoryWeightBlocks = new List<GameObject>();


    private void Enabled()
    {
        RetrieveValues();
        UpdateDisplay();
    }

    private void RetrieveValues()
    {
        damage = 0;
        ammo = 0;
        range = 0;
        weight = 0;
        Wallet = 10000;
    }

    public void NewItemToDisplay(WeaponItemUI weapon)
    {
        // Update Values
        damage = weapon.damage;
        ammo = weapon.ammo;
        range = weapon.range;
        weight = weapon.weight;

        // Update UI
        UpdateDisplay();
    }

    public void BuyItem(WeaponItemUI weapon)
    {
        // Update Values
        damage = weapon.damage;
        ammo = weapon.ammo;
        range = weapon.range;
        weight = weapon.weight;


        // Funds Check
            // Subtract (*Cost*)
        if (Wallet >= weapon.price)
        {
            Wallet -= weapon.price;
        }


        // Update UI
        UpdateDisplay();
    }
    public void SellItem(WeaponItemUI weapon)
    {
        // Update Values
        damage = weapon.damage;
        ammo = weapon.ammo;
        range = weapon.range;
        weight = weapon.weight;

        // Add (*Cost* /2)
        Wallet += (weapon.price);

        // Update UI
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        UpdateDamageBlocks();
        UpdateAmmoBlocks();
        UpdateRangeBlocks();
        UpdateWeightBlocks();
        UpdateWallet();
    }

    private void UpdateWallet()
    {
        walletLabel.text = $"${Wallet}";
    }

    private void UpdateDamageBlocks()
    {
        for (int i = 0; i < damageBlocks.Count; i++)
        {
            if (i <= damage)
                damageBlocks[i].SetActive(true);
            else
                damageBlocks[i].SetActive(false);
        }
    }
    private void UpdateAmmoBlocks()
    {
        for (int i = 0; i < ammoBlocks.Count; i++)
        {
            if (i <= ammo)
                ammoBlocks[i].SetActive(true);
            else
                ammoBlocks[i].SetActive(false);
        }
    }
    private void UpdateRangeBlocks()
    {
        for (int i = 0; i < rangeBlocks.Count; i++)
        {
            if (i <= range)
                rangeBlocks[i].SetActive(true);
            else
                rangeBlocks[i].SetActive(false);
        }
    }
    private void UpdateWeightBlocks()
    {
        for (int i = 0; i < weightBlocks.Count; i++)
        {
            if (i <= weight)
                weightBlocks[i].SetActive(true);
            else
                weightBlocks[i].SetActive(false);
        }
    }

}
