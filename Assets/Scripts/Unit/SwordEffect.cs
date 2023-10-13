using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.VFX;

public class SwordEffect : MonoBehaviour
{
    public VisualEffect[] VEeffects;
    public ParticleSystem[] PSeffects;
    public GameObject startPoint;

    public void Start()
    {
        VEeffects = GetComponentsInChildren<VisualEffect>();
        PSeffects = GetComponentsInChildren<ParticleSystem>();
    }

    public void PlayEffects()
    {
        transform.rotation = startPoint.transform.rotation;
        foreach (var eff in VEeffects)
        {
            eff.Play();
        }
        foreach (var eff in PSeffects)
        {
            eff.Play();
        }
    }
}
