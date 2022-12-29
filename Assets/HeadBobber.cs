using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class HeadBobber : MonoBehaviour
{
    [SerializeField] private Camera cam;

    public float walkingBobbingSpeed = 10f;
    public float runMultiplier = 1.1f;
    public float bobbingAmount = 0.05f;
    public FirstPersonController player;
    //public CharacterController controller;

    bool sprinting = false;
    float defaultPosY = 0;
    float timer = 0;

    private void Start()
    {
        defaultPosY = transform.localPosition.y;
    }

    private void Update()
    {
        if (player._speed > player.MoveSpeed)
            sprinting = true;

        else if (player._speed > 0)
            sprinting = false;

        if (player._speed > 0)
        {
            if (sprinting)
                timer += Time.deltaTime * runMultiplier * walkingBobbingSpeed;
            else
                timer += Time.deltaTime * walkingBobbingSpeed;

            cam.transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
        }

        else
        {
            // Idle
            timer = 0;
            cam.transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), transform.localPosition.z);
        }
    }
}
