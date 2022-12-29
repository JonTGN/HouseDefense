using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class WeaponClass : MonoBehaviour
{
    [Header("Inv Mngmt")]
    public bool isGunEquipped;
    public bool isGunPrimary;
    public bool isGunOwned;
    
    [Header("Gun Stats")]
    public int damage;
    public float timeBetweenShooting, spread, aimSpread, range, reloadTime, timeBetweenShots, knifeTime;
    public int magazineSize, ammoReserve, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    bool readyToShoot, reloading, aiming, running, inShotgunLoop, playingDryFireAnim, playedAimSound, knifing, puttingGunAway, wantsToCancelShotgunReload, altForm;
    //bool readyToMoveAfterSemi;
    
    // expose for shell eject script
    public bool shooting, isShotgun;

    [Header("Movement")]
    public int normalFOV;
    public int zoom;
    public float zoomSmooth;

    [Header("Reference")]
    public Animator anim;
    public Camera fpsCam;
    public Camera rayCam;  // for some reason fpsCam shoots raycast in front at all times?
    public RaycastHit rayHit;
    public FirstPersonController plr;
    public LayerMask whatIsEnemy;
    public CameraShake cameraShake;
    public AudioSource audio_fire;
    public AudioSource audio_reload;
    public AudioSource audio_aim;
    public AudioSource audio_dry_fire;
    public AudioSource audio_knife;
    public AudioSource audio_hit_marker;
    public AudioSource audio_slide;


    [Header("Graphics")]
    public GameObject muzzleFlash1;
    public GameObject muzzleFlash2, bulletsHoleGraphic, bloodSplatter;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;
    public GameObject CustomMuzzleFlash;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        fpsCam.fieldOfView = normalFOV;

        //CustomMuzzleFlash = GameObject.Find("MuzzleFlash");
        //DeactivateMuzzleFlash();
    }

    private void Update()
    {
        // DEBUG > uncomment to set new gun's pos
        //aiming = true;
        //Aim();
        //return;

        if (puttingGunAway)
            return;

        text.SetText(bulletsLeft + " / " + ammoReserve);

        // KMS
        //if (Input.GetKey(KeyCode.Mouse0) && isShotgun && !wantsToCancelShotgunReload && reloading)
        //{
            //wantsToCancelShotgunReload = true;
        //}

        // if reloading
        if (Input.GetKey(KeyCode.R) && !knifing)
            // Reload
            Reload();

        else if (Input.GetKey(KeyCode.C) && !reloading && !knifing && !shooting && !aiming && !running && !anim.GetBool("MeleeAttack"))
        {
            //Knife();
        }

        // if aiming
        else if (Input.GetKey(KeyCode.Mouse1) && !reloading && !running && !knifing)
        {
            if (!playedAimSound)
            {
                audio_aim.Play();
                playedAimSound = true;
            }
            aiming = true;
            Aim();
        }

        // if idling
        else
        {
            aiming = false;
            playedAimSound = false;

            // reset camera fov after done aiming
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, normalFOV, Time.deltaTime * zoomSmooth);

        }

        // if shooting automatic weapon with bullets in clip
        if (allowButtonHold && Input.GetKey(KeyCode.Mouse0) && !reloading && !running && !knifing)
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
        else if (Input.GetKeyDown(KeyCode.Mouse0) && !reloading && !running && readyToShoot && !knifing)
        {
            if (bulletsLeft > 0)
            {
            
                if (aiming)
                {
                    PlayAnim("AltZoomFire");
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

        // play jump anim if not already reloading
        else if (plr._input.jump && !reloading && !plr.Grounded && !knifing)
        {
            PlayAnim("Jump");
        }

        // Is sprinting -> +1 because there is acceleration
        else if (plr._speed > plr.MoveSpeed + 1 && !reloading && plr.Grounded && !knifing && !shooting)
        {
            // play run anim
            PlayAnim("FastMove");

            running = true;

            // while you can shoot and move, both anims will not play
            shooting = false;
        }

        // Movement anims
        else if (plr._speed > 0 && !reloading && plr.Grounded && !knifing && !shooting && !inShotgunLoop)
        {
            if (aiming)
                PlayAnim("ZoomMove");
            
            // move anim broke for semi, dont want to fix :( use idle instead bc lazy
            else if (allowButtonHold)
                PlayAnim("Move");

            else 
                PlayAnim("Idle");

            running = false;

            shooting = false;
        }

        // if not aiming and not reloading be idle
        else if (!aiming && !reloading && !knifing && !inShotgunLoop)
        {
            shooting = false;
            //ResetShot();
            PlayAnim("Idle");

            running = false;
        }

        // if aiming and not shooting
        else
        {
            shooting = false;
        }

    }

    private void Aim()
    {
        PlayAnim("ZoomIdle");
        fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, zoom, Time.deltaTime * zoomSmooth);
    }

    private void Reload()
    {
        if (bulletsLeft < magazineSize && !reloading && ammoReserve > 0)
        {
            shooting = false;

            // sometimes this cancel invoke will clear this value out and keep it true, preventing dry fire anims/sounds
            playingDryFireAnim = false;

            // clear any potential anim resets
            CancelInvoke();

            // if aiming clear any anims and don't allow
            aiming = false;

            if (!isShotgun)
                audio_reload.Play();

            // reset camera fov after done aiming
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, normalFOV, Time.deltaTime * zoomSmooth);

            PlayAnim("Reload");
            reloading = true;

            if (isShotgun)
            {
                // .433 is len of relaod anim for shotgun
                Invoke("ShotgunReload", 0.433f);
                //Invoke("Reload", 0);
            }

            else
                Invoke("ReloadFinished", reloadTime);

            
        }

        else if (isShotgun && (bulletsLeft == magazineSize || ammoReserve == 0))
        {
            PlayAnim("EndReload");
            Invoke("PlaySlideSound", 0.266f);
            Invoke("EndReload", 0.5f);
        }
    }

    private void PlaySlideSound()
    {
        if (!audio_slide.isPlaying)
            audio_slide.Play();
    }

    private void EndReload()
    {
        reloading = false;
        readyToShoot = true;
        inShotgunLoop = false;
        wantsToCancelShotgunReload = false;
    }

    private void Knife()
    {
        knifing = true;
        PlayAnim("MeleeAttack");
        audio_knife.Play();

        Invoke("CompleteKnife", knifeTime);
    }

    private void CompleteKnife()
    {
        // if for some reason plr holds kinfe down anim will not complete properly but still play, make sure melee attack anim not active
        if (anim.GetBool("MeleeAttack"))
            PlayAnim("Idle");
        
        knifing = false;
    }

    private void Shoot()
    {
        if (readyToShoot && !reloading && bulletsLeft > 0)
        {
            //readyToMoveAfterSemi = false;
            if (!inShotgunLoop)
            {
                PlayShotSound();

                bulletsShot = bulletsPerTap;
            }

            if(isShotgun && !inShotgunLoop)
                Invoke("PlaySlideSound", 0.2f);

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
                //Debug.Log(rayHit.collider.name);

                if(rayHit.collider.CompareTag("Enemy"))
                {
                    //audio_hit_marker.Play();
                    
                    // Modify this!
                    rayHit.collider.gameObject.GetComponentInParent<EnemyClass>().Damage(damage);

                    //Graphics
                    Instantiate(bloodSplatter, rayHit.point + new Vector3(0f, 0f, -.02f), Quaternion.FromToRotation(Vector3.up, rayHit.normal));
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(rayCam.transform.position, direction, out hit, range))
            {
			    //DestructibleObject target = hit.transform.GetComponent<DestructibleObject>();
			    GameObject colObject = hit.collider.gameObject;
    
			    //GameObject impactObject = Instantiate(Impact, hit.point, Quaternion.LookRotation(hit.normal));
                if (!hit.collider.CompareTag("Enemy"))
                {
			        GameObject holeObject = Instantiate(bulletsHoleGraphic, hit.point + new Vector3(0f, 0f, -.02f), Quaternion.FromToRotation(Vector3.up, hit.normal));
			        holeObject.transform.SetParent(colObject.transform);
			        //Destroy(impactObject, 2f);
			        Destroy(holeObject, 4f);
                }
		    }

            //Audio 

            //ShakeCamera
            StartCoroutine(cameraShake.Shake(camShakeDuration, camShakeMagnitude));
            //camShake.Shake(camShakeDuration, camShakeMagnitude);

            //Graphics
            //Instantiate(bulletsHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
            //Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);

            if (bulletsShot <= 1)
                bulletsLeft--;

            bulletsShot--;        

            if (bulletsShot > 0 && bulletsLeft > 0)
            {
                readyToShoot = true;

                // will not have burst weapon so lazy way is ok for this
                inShotgunLoop = true;
                Invoke("Shoot", timeBetweenShots);
            }
                
            else
                Invoke("ResetShot", timeBetweenShooting);

        }

        // should never hit this...
        else if (bulletsLeft <= 0)
        {
            //PlayAnim("Idle");
        }

    }

    private void PlayShotSound()
    {
        audio_fire.Play();
        ActivateMuzzleFlash();
        Invoke("DeactivateMuzzleFlash", 0.1f);

    }

    private void ActivateMuzzleFlash()
    {
        CustomMuzzleFlash.SetActive(true);
    }

    private void DeactivateMuzzleFlash()
    {
        CustomMuzzleFlash.SetActive(false);
    }

    private void ResetShot()
    {
        if (!allowButtonHold)
        {
            if (!aiming && !inShotgunLoop && !reloading)
                PlayAnim("Idle");

            // make move anim wait for this var if using semi
            Invoke("SemiShootingCooldown", timeBetweenShooting + 1);
            
        }

        else if (!shooting && !inShotgunLoop) 
        {
            PlayAnim("Idle");
        }
        readyToShoot = true;
        inShotgunLoop = false;
    }

    // idea behind this method is because move anim plays too quick after shooting, cutting into the shoot anim while using semi auto gun
    // make the move anim wait for <shootingcooldown> var too before activating again
    private void SemiShootingCooldown()
    {
        //readyToMoveAfterSemi = true;
    }

    private void ShotgunReload()
    {
        if (wantsToCancelShotgunReload)
        {
            CancelInvoke();
            PlayAnim("EndReload");
            Invoke("EndReload", reloadTime);

        }
        if (ammoReserve > 0 && bulletsLeft < magazineSize)
        {
            PlayAnim("ReloadLoop");
            // reload anim happens at .266
            Invoke("ShotgunReloadSound", 0.266f);
            Invoke("ShotgunReload", reloadTime);
        }

        else
            Reload();
    }

    private void ShotgunReloadSound()
    {
        audio_reload.Play();
        bulletsLeft++;
        ammoReserve--;
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
    
    // bug: movement can get stuck in dry fire
    private void DryFire()
    {
        // No path from fire to dry fire, can't get bool on zoom fire? always false??
        if (anim.GetBool("Fire"))
            PlayAnim("Idle");  
        
        else if (aiming && !shooting && !playingDryFireAnim)
        {
            PlayAnim("ZoomIdle");

            // still play dry fire sound; anim bool is just timer
            playingDryFireAnim = true;
            audio_dry_fire.Play();
            Invoke("ResetDryFireTimer", 0.4f);
        }
             
        else if (!playingDryFireAnim)
        {
            PlayAnim("DryFire");

            playingDryFireAnim = true;
            audio_dry_fire.Play();
            Invoke("ResetDryFireTimer", 0.4f);
        } 

        // since aim uses same counter, we can't idle when counter is active
        else if (!aiming)
        {
            PlayAnim("Idle");
        }
    }

    private void ResetDryFireTimer()
    {
        playingDryFireAnim = false;
    }
    
    public bool PutGunAway()
    {
        if (!reloading && !knifing && !shooting && !running)
        {
            Debug.Log("putting gun away");
            CancelInvoke();

            puttingGunAway = true;
            PlayAnim("PutAway");

            Invoke("PutAwayFinish", 0.5f);

            return true;
        }

        else
            return false;
    }

    private void PutAwayFinish()
    {
        puttingGunAway = false;
        readyToShoot = true;

        gameObject.SetActive(false);
    }

    private void PlayAnim(string animToSetToTrue)
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Fire", false);
        anim.SetBool("ZoomIdle", false);
        anim.SetBool("ZoomFire", false);
        anim.SetBool("Reload", false);
        anim.SetBool("DryFire", false);
        anim.SetBool("Move", false);
        anim.SetBool("FastMove", false);
        anim.SetBool("ZoomMove", false);
		anim.SetBool("Jump", false);
        anim.SetBool("AltFire", false);
        anim.SetBool("AltZoomFire", false);
        anim.SetBool("MeleeAttack", false);
        anim.SetBool("PutAway", false);
        anim.SetBool("ReloadLoop", false);
        anim.SetBool("EndReload", false);
        anim.SetBool("AlternateTo", false);

        // -- todo: -- //
		anim.SetBool("EmptyReload", false);
		anim.SetBool("Crouch", false);
		anim.SetBool("ZoomCrouch", false);
		anim.SetBool("GrenadeThrow", false);
		anim.SetBool("Select", false);

        anim.SetBool(animToSetToTrue, true);
    }

}

