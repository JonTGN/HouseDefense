using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponClass : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform playerGunningPosition;

    [Header("Gun Stats")]
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    [Header("Bools")]
    bool shooting, readyToShoot, reloading;

    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public AudioSource audioSource;

    [Header("Graphics")]
    public GameObject muzzleFlash, bulletsHoleGraphic;
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;


    private bool IsControlled;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        //if (!(PlayerController is null))
        //{
        //    //tText
        //    text.SetText(bulletsLeft + " / " + magazineSize);
        //}

        //Debug.Log("Subscribe() Called");

        //// Subscribe to all events


        if (allowButtonHold && Input.GetKey(KeyCode.Mouse0))
            // Can hold fire
            Shoot();
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            // Manual fire
            Shoot();

        if (Input.GetKey(KeyCode.R))
            // Reload
            Reload();
    }

    /*
     * 
     */
    private void Reload()
    {
        if (bulletsLeft < magazineSize && !reloading)
        {
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }
    }

    private void Shoot()
    {
        if (readyToShoot && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;

            readyToShoot = false;

            //Spread 
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //Calculate Direction with Spread
            Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

            //Raycast
            if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
            {
                Debug.Log(rayHit.collider.name);

                //if (rayHit.collider.CompareTag("Enemy"))
                //Modify this!
                //rayHit.collider.GetComponent<ArmorHealth>().takeDamage(damage);

            }

            //Audio 

            //ShakeCamera
            StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));

            //Graphics
            Instantiate(bulletsHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
            Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);

            bulletsLeft--;
            bulletsShot--;

            Invoke("ResetShot", timeBetweenShooting);

            if (bulletsShot > 0 && bulletsLeft > 0)
                Invoke("Shoot", timeBetweenShots);

        }

    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

}

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {

        Vector3 orignialPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-.25f, .25f) * magnitude;
            float y = Random.Range(-.25f, .25f) * magnitude;

            transform.localPosition = transform.localPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            //Debug.Log($"OriginalPos: {orignialPos} || New Position: {transform.localPosition}");
            yield return null;
        }

        transform.localPosition = orignialPos;
    }
}
