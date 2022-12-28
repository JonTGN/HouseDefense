using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class HeadBobber : MonoBehaviour
{
    [SerializeField] private Camera cam;

    public float walkingBobbingSpeed = 14f;
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = true;
        }

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            //Player is moving

            if (sprinting)
                timer += Time.deltaTime * 1.5f * walkingBobbingSpeed;
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
