using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BloodEffectController : MonoBehaviour
{
    [SerializeField] private VisualEffect bloodVisualEffect;

    public void PlayBloodEffect()
    {
        if (bloodVisualEffect != null)
        {
            bloodVisualEffect.Play();
        }
    }

    public void StopBloodEffect()
    {
        if (bloodVisualEffect != null)
        {
            bloodVisualEffect.Stop();
        }
    }

    public void DestroyBloodEffect()
    {
        if (bloodVisualEffect != null)
        {
            Destroy(bloodVisualEffect.gameObject);
        }
    }
}