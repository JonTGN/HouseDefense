using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

public class TabMenuController : MonoBehaviour
{

    [SerializeField]
    public GameObject TabMenu;

    public WeaponClass Weapon;
    public FirstPersonController Controller;

    private void Awake()
    {
        TabMenu.SetActive(false);
        Cursor.visible = false;


        Cursor.lockState = CursorLockMode.Locked;
        
    }
    
    private void Update()
    {
        // If TabMenu closed, we should not see cursor.
        if (Cursor.visible && !TabMenu.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log($"Cursor set to: {Cursor.lockState}");
        }
        else if (!Cursor.visible && TabMenu.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Debug.Log($"Cursor set to: {Cursor.lockState}");
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TabMenu.SetActive(!TabMenu.activeSelf);
            Weapon.enabled = !TabMenu.activeSelf;
            Controller.enabled = !TabMenu.activeSelf;
        }
    }
}
