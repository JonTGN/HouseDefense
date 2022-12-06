using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunCam : MonoBehaviour
{

    [Header("Gun Information")]
    public float viewAngle;
    public float fireAngle;
    public float sensX;
    public float sensY;

    [Header("Camera Information")]
    private float xRotation;
    private float yRotation;

    [Header("Targeting Information")]
    public float aimDistance;
    public Image crosshair;
    public GameObject barrel;
    //public MountedGunController gunController;


    private void OnEnable()
    {
        Debug.Log("Camera Enabled");
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ShootRaycast();

        // get mouse input 
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;


        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -fireAngle, fireAngle);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -viewAngle, viewAngle);

        //rotate cam and orientation
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        //transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void ShootRaycast()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 2;

        layerMask = ~layerMask;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, aimDistance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            //Debug.Log($"Did Hit {hit.transform}");
            crosshair.color = new Color(1f, 0f, 0f, .25f);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * aimDistance, Color.green);
            //Debug.Log("Did not Hit");
            crosshair.color = new Color(0f, 0f, 0f, .25f);
        }

        Vector3 TargetPos = transform.TransformDirection(Vector3.forward) * aimDistance;
        //gunController.Target = TargetPos;
        //barrel.transform.LookAt(transform.TransformDirection(Vector3.forward) * aimDistance);
    }

    public void RemoveGunCamControl()
    {
        Destroy(gameObject.GetComponent<GunCam>());
    }
}
