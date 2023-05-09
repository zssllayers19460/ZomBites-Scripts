using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Add Melee Audio...")]
public class MeleeAudioSO : ScriptableObject
{
    [Header("Misc Options")]
    [Range(0, 1f)]
    public float Volume = 1f;

    [Header("Frag Grenades Sounds")]
    public AudioClip ExplosionSound;
    public AudioClip PullPinSound;
    public AudioClip ThrowPinSound;
    public AudioClip HoldGrenadeSound;
    public AudioClip AirResistanceSound;
    public AudioClip UnequipGrenadeSound;

    public void PlayExplosionSound(AudioSource audioSource)
    {
        if (ExplosionSound != null)
        {
            audioSource.PlayOneShot(ExplosionSound, Volume);
        }
    }
    public void PlayPullPinSound(AudioSource audioSource)
    {
        if (PullPinSound != null)
        {
            audioSource.PlayOneShot(PullPinSound, Volume);
        }
    }
    public void PlayThrowPinSound(AudioSource audioSource)
    {
        if (ThrowPinSound != null)
        {
            audioSource.PlayOneShot(ThrowPinSound, Volume);
        }
    }
    public void PlayHoldGrenadeSound(AudioSource audioSource)
    {
        if (HoldGrenadeSound != null)
        {
            audioSource.PlayOneShot(HoldGrenadeSound, Volume);
        }
    }
    public void PlayAirResistanceSound(AudioSource audioSource)
    {
        if (AirResistanceSound != null)
        {
            audioSource.PlayOneShot(AirResistanceSound, Volume);
        }
    }
    public void PlayUnequipGrenadeSound(AudioSource audioSource)
    {
        if (UnequipGrenadeSound != null)
        {
            audioSource.PlayOneShot(UnequipGrenadeSound, Volume);
        }
    }
}
