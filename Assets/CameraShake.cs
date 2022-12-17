using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
