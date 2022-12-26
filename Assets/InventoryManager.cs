using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public WeaponClass[] guns;

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

        guns[0].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // switch to glock
        if(Input.GetKeyDown(KeyCode.Alpha1) && guns[0].gameObject.activeSelf == false)
        {
            // put rifle away 
            guns[1].PutGunAway();

            StartCoroutine(EquipNewWeapon(guns[0].gameObject, 0.5f));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"Conditions failed! (Switch to glock!)\n Conditions: {Input.GetKeyDown(KeyCode.Alpha1)} | {guns[1].gameObject.activeSelf == false}");
        }

        // switch to rifle
        if (Input.GetKeyDown(KeyCode.Alpha2) && ownRifle && guns[1].gameObject.activeSelf == false)
        {
            // put rifle away 
            guns[0].PutGunAway();

            StartCoroutine(EquipNewWeapon(guns[1].gameObject, 0.5f));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log($"Conditions failed! (Switch to rifle!)\n Conditions: {Input.GetKeyDown(KeyCode.Alpha2)} | {ownRifle} | {guns[1].gameObject.activeSelf == false}");
        }

        //// equip primary
        //if (input.getkeydown(keycode.alpha1))
        //{
        //    equippedgun = getcurrentgun();
        //    newgun = getnewgun();

        //    // return; primary already equipped
        //    if (equippedgun.isgunprimary)
        //        return;

        //    equippedgun.putgunaway();
        //    equippedgun.isgunequipped = false;

        //    // wait for put gun away func to finish
        //    startcoroutine(Equipewweapon(newgun.gameobject, 0.5f));
        //}

        //// equip secondary
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    equippedGun = GetCurrentGun();
        //    newGun = GetNewGun();

        //    // return; secondary already equipped
        //    if (!equippedGun.isGunPrimary)
        //        return;

        //    equippedGun.PutGunAway();
        //    equippedGun.isGunEquipped = false;

        //    // wait for put gun away func to finish
        //    StartCoroutine(EquipNewWeapon(newGun.gameObject, 0.5f));
        //}
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

    IEnumerator EquipNewWeapon(GameObject weaponToEquip, float delay)
    {
        yield return new WaitForSeconds(delay);

        // activate GO
        weaponToEquip.SetActive(true);

        // play equip anim in script
        weaponToEquip.GetComponent<WeaponClass>().isGunEquipped = true;
    } 

}
