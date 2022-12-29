using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponItemUI : MonoBehaviour
{
    public string name;
    public string price;

    public TextMeshProUGUI weaponNameLabel;
    public TextMeshProUGUI weaponPriceLabel;

    [Range(1,7)]
    public int damage;
    [Range(1, 7)]
    public int ammo;
    [Range(1, 7)]
    public int range;
    [Range(1, 7)]
    public int weight;

    
    private void OnEnable()
    {
        
    }


    public void WeaponSelected()
    {

    }
}
