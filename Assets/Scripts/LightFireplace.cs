using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFireplace : MonoBehaviour, IInteractable
{
    public GameObject activateFireplace;
    public AudioSource audio_match;
    public AudioSource audio_fireplace;

    public string GetDescription()
    {
        return "Light Fireplace";
    }

    public void Interact()
    {
        // play light sound
        audio_match.Play();
        Invoke("PlayFireplaceAmbience", 0.4f);

        activateFireplace.SetActive(true);
    }

    private void PlayFireplaceAmbience()
    {
        audio_fireplace.Play();
    }

    Items IInteractable.GetType()
    {
        return Items.Fireplace;
    }
}
