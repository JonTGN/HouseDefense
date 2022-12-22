using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cinemachine;

public class WeaponClass : MonoBehaviour
{

    [Header("Gun Stats")]
    public int damage;
    public float timeBetweenShooting, spread, aimSpread, range, reloadTime, timeBetweenShots;
    public int magazineSize, ammoReserve, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    bool readyToShoot, reloading, aiming;
    
    // expose for shell eject script
    public bool shooting;

    [Header("Movement")]
    public int normalFOV;
    public int zoom;
    public float zoomSmooth;

    [Header("Reference")]
    public Animator anim;
    public CinemachineVirtualCamera fpsCam;
    public Camera rayCam;  // for some reason fpsCam shoots raycast in front at all times?
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public CameraShake cameraShake;
    public AudioSource audioSource;
    public AudioClip audio_fire;

    [Header("Graphics")]
    public GameObject muzzleFlash1;
    public GameObject muzzleFlash2, bulletsHoleGraphic;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        fpsCam.m_Lens.FieldOfView = normalFOV;
    }

    private void Update()
    {
        // DEBUG > set to find optimal gun pos while aming, comment both elses out
        //aiming = true;
        //Aim();

        text.SetText(bulletsLeft + " / " + ammoReserve);

        // if reloading
        if (Input.GetKey(KeyCode.R))
            // Reload
            Reload();

        // if aiming
        else if (Input.GetKey(KeyCode.Mouse1) && !reloading)
        {
            aiming = true;
            Aim();
        }

        // if idling
        else
        {
            aiming = false;

            // reset camera fov after done aiming
            fpsCam.m_Lens.FieldOfView = Mathf.Lerp(fpsCam.m_Lens.FieldOfView, normalFOV, Time.deltaTime * zoomSmooth);

        }

        // if shooting automatic weapon with bullets in clip
        if (allowButtonHold && Input.GetKey(KeyCode.Mouse0) && !reloading)
        {
            if (bulletsLeft > 0)
            {
            
                if (aiming)
                {
                    PlayAnim("ZoomFire");
                }

                else
                {
                    // if cont. fire do not play idle anim in the middle of cooldown
                    PlayAnim("Fire");
                }

                shooting = true;
                Shoot();
            }

            // no bullets left, dry fire
            // TODO: FIX BUG: When plr holds mouse down dry fire anim is held and idle is not so gun is static
            // Introduce wait time for duration of dry fire anim, when anim done goto idle/aim anim
            else if (bulletsLeft <= 0)
            {
                shooting = false;
                DryFire();
                //Reload();
            }
            
        }

        // if shooting semi-auto weapon with bullets in clip
        else if (Input.GetKeyDown(KeyCode.Mouse0) && !reloading)
        {
            if (bulletsLeft > 0)
            {
            
                if (aiming)
                {
                    PlayAnim("ZoomFire");
                }

                else
                {
                    // if cont. fire do not play idle anim in the middle of cooldown
                    PlayAnim("Fire");
                }

                shooting = true;
                Shoot();
            }

            // no bullets left, dry fire
            else if (bulletsLeft <= 0)
            {
                shooting = false;
                DryFire();
            }
        }

        // if not aiming and not reloading
        else if (!aiming && !reloading)
        {
            shooting = false;
            ResetShot();
        }

        // if aiming and not shooting
        else
        {
            shooting = false;
        }

    }

    /*
     * 
     */
    private void Aim()
    {
        PlayAnim("ZoomIdle");
        fpsCam.m_Lens.FieldOfView = Mathf.Lerp(fpsCam.m_Lens.FieldOfView, zoom, Time.deltaTime * zoomSmooth);
    }

    private void Reload()
    {
        if (bulletsLeft < magazineSize && !reloading && ammoReserve > 0)
        {
            // clear any potential anim resets
            CancelInvoke();

            // if aiming clear any anims and don't allow
            aiming = false;

            // reset camera fov after done aiming
            fpsCam.m_Lens.FieldOfView = Mathf.Lerp(fpsCam.m_Lens.FieldOfView, normalFOV, Time.deltaTime * zoomSmooth);

            PlayAnim("Reload");
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }
    }

    private void Shoot()
    {
        if (readyToShoot && !reloading && bulletsLeft > 0)
        {
            PlayShotSound();

            bulletsShot = bulletsPerTap;

            readyToShoot = false;

            //Spread 
            float x, y;
            if (aiming)
            {
                x = Random.Range(-aimSpread, aimSpread);
                y = Random.Range(-aimSpread, aimSpread);
            }

            else
            {
                x = Random.Range(-spread, spread);
                y = Random.Range(-spread, spread);
            }
            

            //Calculate Direction with Spread
            Vector3 direction = rayCam.transform.forward + new Vector3(x, y, 0);

            Debug.DrawRay(rayCam.transform.position, direction *  range);
            //Raycast
            if (Physics.Raycast(rayCam.transform.position, direction, out rayHit, range, whatIsEnemy))
            {
                Debug.Log(rayHit.collider.name);

                if(rayHit.collider.CompareTag("Enemy"))
                {
                    // Modify this!
                    rayHit.collider.gameObject.GetComponentInParent<EnemyClass>().Damage(damage);

                    //Graphics
                    //Instantiate(bulletsHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(rayCam.transform.position, direction, out hit, range))
            {
			    //DestructibleObject target = hit.transform.GetComponent<DestructibleObject>();
			    GameObject colObject = hit.collider.gameObject;
    
			    //GameObject impactObject = Instantiate(Impact, hit.point, Quaternion.LookRotation(hit.normal));
			    GameObject holeObject = Instantiate(bulletsHoleGraphic, hit.point + new Vector3(0f, 0f, -.02f), Quaternion.FromToRotation(Vector3.up, hit.normal));
			    holeObject.transform.SetParent(colObject.transform);
			    //Destroy(impactObject, 2f);
			    Destroy(holeObject, 4f);
		    }

            //Audio 

            //ShakeCamera
            cameraShake.Shake(camShakeDuration, camShakeMagnitude);
            //camShake.Shake(camShakeDuration, camShakeMagnitude);

            //Graphics
            //Instantiate(bulletsHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
            //Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);

            bulletsLeft--;
            bulletsShot--;

            Invoke("ResetShot", timeBetweenShooting);

            if (bulletsShot > 0 && bulletsLeft > 0)
                Invoke("Shoot", timeBetweenShots);

        }

        // should never hit this...
        else if (bulletsLeft <= 0)
        {
            PlayAnim("Idle");
        }

    }

    private void PlayShotSound()
    {
        audioSource.Play();
    }

    private void ResetShot()
    {
        if (!shooting) 
        {
            PlayAnim("Idle");
        }
        readyToShoot = true;
    }

    private void ReloadFinished()
    {
        int amountNeededToResupply = magazineSize - bulletsLeft;

        // can do a full reload
        if (ammoReserve - amountNeededToResupply >= 0 )
        {
            bulletsLeft = magazineSize;
            ammoReserve -= amountNeededToResupply;
        }

        // can only partly reload
        else
        {
            bulletsLeft += ammoReserve;
            ammoReserve = 0;
        }
        
        // tell shoot its okay to shoot
        reloading = false;
        readyToShoot = true;
    }

    private void DryFire()
    {
        CancelInvoke();

        // lazy coding, no path from fire to dry fire, can't get bool on zoom fire? always false??
        if (anim.GetBool("Fire"))
            PlayAnim("Idle");  
        
        else if (aiming && !shooting)
        {
            PlayAnim("ZoomIdle");

            // still play dry fire sound
        }
             
        else
        {
            PlayAnim("DryFire");
        } 
    }
    
    private void PlayAnim(string animToSetToTrue)
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Fire", false);
        anim.SetBool("ZoomIdle", false);
        anim.SetBool("ZoomFire", false);
        anim.SetBool("Reload", false);
        anim.SetBool("DryFire", false);

        // -- todo: -- //
        anim.SetBool("Move", false);
		anim.SetBool("ZoomMove", false);
		anim.SetBool("FastMove", false);
		anim.SetBool("ReloadLoop", false);
		anim.SetBool("EndReload", false);
		anim.SetBool("EmptyReload", false);
		anim.SetBool("MeleeAttack", false);
		anim.SetBool("Crouch", false);
		anim.SetBool("ZoomCrouch", false);
		anim.SetBool("Jump", false);
		anim.SetBool("GrenadeThrow", false);
		anim.SetBool("Select", false);

        anim.SetBool(animToSetToTrue, true);
    }

}

