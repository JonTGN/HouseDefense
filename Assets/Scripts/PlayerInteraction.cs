using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public Camera mainCam;
    public float interactionDistance = 2f;

    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;

    // local gameobjects player has
    private bool hasMatches;
    private bool alreadyLitFireplace;

    private void Update()
    {
        InteractionRay();
    }

    void InteractionRay()
    {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one/2f);
        RaycastHit hit;

        bool hitSomething = false;

        Vector3 direction = mainCam.transform.forward;
        Debug.DrawRay(mainCam.transform.position, direction *  2f);

        if (Physics.Raycast(ray, out hit, interactionDistance)) 
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // check if has matches or has already lit it before interacting w/ fireplace
                if (interactable.GetType() == Items.Fireplace)
                {
                    if (!hasMatches || alreadyLitFireplace)
                        return;
                }

                hitSomething = true;
                interactionText.text = interactable.GetDescription();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (interactable.GetType() == Items.Matches)
                        hasMatches = true;

                    // will only get here if has matches, so safe to set to true
                    if (interactable.GetType() == Items.Fireplace)
                    {
                        alreadyLitFireplace = true;

                        // make interaction text disappear after lighting it
                        hitSomething = false;
                    }
                        

                    interactable.Interact();
                }
            }
        }

        interactionUI.SetActive(hitSomething);

    }
}
