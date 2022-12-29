using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Shopkeeper : MonoBehaviour
{
    [Header("Inventory")]
    public InventoryManager inventory;
    [Header("Controls")]
    public PlayerInput input;
    [Header("Director")]
    public Director levelManager;


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

    public TextMeshProUGUI weaponNameDisplayLabel;
    public TextMeshProUGUI weaponPriceLabel;

    [Header("Stats List")]
    public TextMeshProUGUI walletLabel;
    public List<GameObject> damageBlocks = new List<GameObject>();
    public List<GameObject> ammoBlocks = new List<GameObject>();
    public List<GameObject> rangeBlocks = new List<GameObject>();
    public List<GameObject> weightBlocks = new List<GameObject>();

    public List<GameObject> inventoryWeightBlocks = new List<GameObject>();


    private void OnEnable()
    {
        weaponNameDisplayLabel.text = "None";
        weaponPriceLabel.text = "$0";
        RetrieveValues();
        UpdateDisplay();
        UpdateInventoryWeightBlocks();
        Debug.Log($"Grabbed current gun: {inventory.currentGun}");
        Debug.Log($"Grabbed current gun script: {inventory.EnumToWeapon(inventory.currentGun).GetComponent<WeaponClass>()}");
        inventory.EnumToWeapon(inventory.currentGun).GetComponent<WeaponClass>().readyToShoot = false;
        Debug.Log("OnEnable()");
    }

    private void RetrieveValues()
    {
        inventory = GameObject.Find("Test Player").GetComponent<InventoryManager>();
        
        damage = 0;
        ammo = 0;
        range = 0;

        if (inventory.primaryGun != Guns.None)
            weight = (int)EnumToWeight(inventory.primaryGun);
        else
            weight = 0;

        // Take from director
        Wallet = levelManager.wallet;
    }

    private int EnumToWeight(Guns weapon)
    {
        switch (weapon)
        {
            case (Guns.AR):
                return 4;

            case (Guns.Shotgun):
                return 4;
            default:
                return 0;
        }
    }

    private Guns NameToEnum(string name)
    {
        Debug.Log(name);
        switch (name)
        {
            case ("Assault Rifle"):
                return Guns.AR;
            case ("Shotgun"):
                return Guns.Shotgun;
            default:
                return Guns.None;
        }
    }

    public void NewItemToDisplay(WeaponItemUI weapon)
    {
        weaponNameDisplayLabel.text = weapon.name;
        weaponPriceLabel.text = $"${weapon.price}";

        // Update Values
        damage = weapon.damage;
        ammo = weapon.ammo;
        range = weapon.range;
        weight = weapon.weight;

        // Update UI
        UpdateDisplay();

        DisableButtons();
    }

    private void DisableButtons()
    {
        //Disable all buy/sell buttons 
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void BuyItem(WeaponItemUI weapon)
    {
        if(inventory.primaryGun == Guns.None)
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
            UpdateInventoryWeightBlocks();
            inventory.primaryGun = NameToEnum(weapon.name);
            // Add primary
            // ask jon
        }

    }
    public void SellItem(WeaponItemUI weapon)
    {
        // Remove primary
        //inventory.EnumToWeapon(inventory.primaryGun).PutGunAway();
        if (inventory.EnumToWeapon(inventory.primaryGun).PutGunAway())
        {
            inventory.EnumToWeapon(inventory.currentGun).GetComponent<WeaponClass>().readyToShoot = true;
            // Update Values
            damage = weapon.damage;
            ammo = weapon.ammo;
            range = weapon.range;
            weight = weapon.weight;

            // Add (*Cost* /2)
            Wallet += ((int)weapon.price / 2);

            // Update UI
            //UpdateDisplay();
            UpdateWallet();
            weight = 0;
            UpdateInventoryWeightBlocks();

            StartCoroutine(inventory.EquipNewWeapon(inventory.EnumToWeapon(Guns.Pistol), 0.5f));
            inventory.primaryGun = Guns.None;
            inventory.currentGun = Guns.Pistol;
        } else
        {
            Debug.Log("Can't sell gun!");
        }
    }

    private void UpdateDisplay()
    {
        UpdateDamageBlocks();
        UpdateAmmoBlocks();
        UpdateRangeBlocks();
        UpdateWeightBlocks();
        //UpdateInventoryWeightBlocks();
            // ask jon
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
            if (i < damage)
                damageBlocks[i].SetActive(true);
            else
                damageBlocks[i].SetActive(false);
        }
    }
    private void UpdateAmmoBlocks()
    {
        for (int i = 0; i < ammoBlocks.Count; i++)
        {
            if (i < ammo)
                ammoBlocks[i].SetActive(true);
            else
                ammoBlocks[i].SetActive(false);
        }
    }
    private void UpdateRangeBlocks()
    {
        for (int i = 0; i < rangeBlocks.Count; i++)
        {
            if (i < range)
                rangeBlocks[i].SetActive(true);
            else
                rangeBlocks[i].SetActive(false);
        }
    }
    private void UpdateWeightBlocks()
    {
        for (int i = 0; i < weightBlocks.Count; i++)
        {
            if (i < weight)
                weightBlocks[i].SetActive(true);
            else
                weightBlocks[i].SetActive(false);
        }
    }

    private void UpdateInventoryWeightBlocks()
    {
        for (int i = 0; i < inventoryWeightBlocks.Count; i++)
        {
            if (i < weight)
                inventoryWeightBlocks[i].SetActive(true);
            else
                inventoryWeightBlocks[i].SetActive(false);
        }
    }

    public void Close()
    {
        DisableButtons();
        input.enabled = true;
        gameObject.SetActive(false);
        inventory.EnumToWeapon(inventory.currentGun).GetComponent<WeaponClass>().readyToShoot = true;
        Cursor.lockState = CursorLockMode.Locked;

        // Take from director
        levelManager.wallet = Wallet;
    }


}
