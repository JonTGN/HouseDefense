using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnClick : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject.Find("Test Enemy").GetComponent<DeathCube>().damage(10);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameObject.Find("Test Player").GetComponent<DeathCube>().damage(10);
        }
    }
}
