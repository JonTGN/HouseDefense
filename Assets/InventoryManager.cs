using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public WeaponClass[] guns;
    private WeaponClass equippedGun, newGun;


    // Update is called once per frame
    void Update()
    {
        // equip primary
        if (Input.GetKeyDown(KeyCode.Alpha1))
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
        if (Input.GetKeyDown(KeyCode.Alpha2))
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
