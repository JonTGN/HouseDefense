using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    public WeaponClass[] guns;
    [SerializeField] private Guns currentGun = Guns.None;
    [SerializeField] private Guns primaryGun = Guns.None;

    private bool ownRifle = true;

    public bool OwnRifle
    {
        get => ownRifle;
        set
        { 
            ownRifle = value;
        }
    }

    private WeaponClass equippedGun, newGun;


    private void Awake()
    {
        // Guns start in a disabled state
        // Look for owned weapons
        // Equip first weapon (Glock 100% of the time)
        // Take Glock out

        currentGun = Guns.Pistol;
    }

    // Update is called once per frame
    void Update()
    {
        // switch to glock
        if(Input.GetKeyDown(KeyCode.Alpha1) && currentGun != Guns.Pistol)
        {
            // get gun from enum
            if (EnumToWeapon(currentGun).PutGunAway())
                StartCoroutine(EquipNewWeapon(EnumToWeapon(Guns.Pistol), 0.5f));
            
            currentGun = Guns.Pistol;
        }

        // switch to primary
        if (Input.GetKeyDown(KeyCode.Alpha2) && currentGun == Guns.Pistol)
        {
            // put pistol away 
            if (EnumToWeapon(currentGun).PutGunAway())
                StartCoroutine(EquipNewWeapon(EnumToWeapon(primaryGun), 0.5f));

            currentGun = primaryGun;
        }

       
        /*
        // equip primary
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            equippedGun = GetCurrentGun();
            newGun = GetNewGun();

            // return; primary already equipped
            if (equippedGun.isGunPrimary)
                return;

            equippedGun.PutGunAway();
            equippedGun.isGunEquipped = false;

            // wait for put gun away func to finish
            StartCoroutine(EquipNewWeapon(newGun.gameObject, 0.5f));
        }

        // equip secondary
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            equippedGun = GetCurrentGun();
            newGun = GetNewGun();

            // return; secondary already equipped
            if (!equippedGun.isGunPrimary)
                return;

            equippedGun.PutGunAway();
            equippedGun.isGunEquipped = false;

            // wait for put gun away func to finish
            StartCoroutine(EquipNewWeapon(newGun.gameObject, 0.5f));
        }
        */
    }

  private WeaponClass EnumToWeapon(Guns gunType)
  {
        switch (gunType)
        {
            case (Guns.Pistol) :
                return guns[0];
            
            case (Guns.AR) :
                return guns[1];
            
            case (Guns.Shotgun) : 
                return guns[2];

            default :
                return null;
        }
  }

  private WeaponClass GetCurrentGun()
    {
        // get currently equipped weapon
        foreach (var gun in guns)
        {
            if (gun.isGunEquipped)
                return gun;
        }

        // should not ever hit this 
        return new WeaponClass();
    }

    private WeaponClass GetNewGun()
    {
        // get first gun in list where it is not primary (always pistol for now)
        if (equippedGun.isGunPrimary)
            return guns.Where(o => !o.isGunPrimary).FirstOrDefault();

        // get first gun that is owned that is primary (shop will prevent owning 2 primaries at a time)
        return guns.Where(o => o.isGunOwned && o.isGunPrimary).FirstOrDefault();
    }

    IEnumerator EquipNewWeapon(WeaponClass weaponToEquip, float delay)
    {
        yield return new WaitForSeconds(delay);

        // activate GO
        weaponToEquip.gameObject.SetActive(true);

        // play equip anim in script
        weaponToEquip.GetComponent<WeaponClass>().isGunEquipped = true;
    } 

}
