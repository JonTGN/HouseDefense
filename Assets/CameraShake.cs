using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float freq = 0.5f;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;
    private CinemachineBasicMultiChannelPerlin mcp;

    private void Awake()
    {
        //mcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float duration, float magnitude)
    {
        mcp.m_AmplitudeGain = magnitude;
        mcp.m_FrequencyGain = freq;
        startingIntensity = magnitude;
        shakeTimer = duration;
        shakeTimerTotal = duration;
    }

    private void Update()
    {
        if (shakeTimer > 0) 
        {
            shakeTimer -= Time.deltaTime;

            mcp.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, ( 1 - (shakeTimer / shakeTimerTotal)));
            mcp.m_FrequencyGain = Mathf.Lerp(freq, 0f, ( 1 - (shakeTimer / shakeTimerTotal)));
        }
    }
}
