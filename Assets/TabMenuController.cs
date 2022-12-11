using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TabMenuController : MonoBehaviour
{

    [SerializeField]
    public GameObject TabMenu;

    private void Awake()
    {
        TabMenu.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            TabMenu.SetActive(!TabMenu.activeSelf);
        }
    }
}
