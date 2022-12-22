using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifespan : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Decay());
    }


    IEnumerator Decay()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
