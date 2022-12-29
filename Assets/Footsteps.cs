using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Footsteps : MonoBehaviour
{
    public FirstPersonController player;
    public AudioSource footstepPlayer;
    public AudioClip[] woodSurface;
    public AudioClip[] carpetSurface;
    public float walkWait, runWait;
    private float soundWait;
    private bool isPlayingWalkSound;

    void Update()
    {
        if (!isPlayingWalkSound && player._speed > 0)
            PlayFootsteps();
    }

    public void PlayFootsteps()
    {
		RaycastHit hit = new RaycastHit();
		string floortag;

        if(player.Grounded)
		{
		    if(Physics.Raycast(transform.position, Vector3.down, out hit))
            {
		        floortag = hit.collider.gameObject.tag;

                // if running set faster wait time
		        if (player._speed > player.MoveSpeed)
                    soundWait = runWait;
                
                // player is walking
                else
                    soundWait = walkWait;

                if (floortag == "Wood")
		        {
                    var selectedFootstep = woodSurface[Random.Range(0, woodSurface.Length)];
		            StartCoroutine(playFootstepSound(selectedFootstep));
		        }

		        else if (floortag == "Carpet")
		        {
                    footstepPlayer.clip = carpetSurface[Random.Range(0, carpetSurface.Length)];
		            footstepPlayer.Play();
		        }
		    }
		}
    }

    IEnumerator playFootstepSound(AudioClip audio_file)
    {
        isPlayingWalkSound = true;
        footstepPlayer.clip = audio_file;
        footstepPlayer.Play();
        yield return new WaitForSeconds(footstepPlayer.clip.length + soundWait);
        isPlayingWalkSound = false;
    }
}
