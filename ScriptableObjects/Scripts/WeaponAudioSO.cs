using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Add Weapon Audio...")]
public class WeaponAudioSO : ScriptableObject
{
    [Header("Misc Options")]
    [Range(0, 1f)]
    public float Volume = 0.4f;

    [Header("Bullet Type Weapon Sounds")]
    public AudioClip ShootingClip;
    public AudioClip ReloadClip;
    public AudioClip DryFireClip;
    public AudioClip LastBulletClip;
    public AudioClip RackingClip;
    public AudioClip CasingClip;

    [Header("Rocket Weapon Sounds")]
    public AudioClip RocketFireClip;
    public AudioClip RocketReloadClip;
    public AudioClip RocketDryFireClip;
    public AudioClip RocketCasingClip;
    public AudioClip RocketExplosionClip;

    [Header("Mini Gun Weapon Sounds")]
    public AudioClip MiniGunFireStopClip;
    public AudioClip MiniGunFireLoopClip;
    public AudioClip MiniGunSpinLoopClip;
    public AudioClip MiniGunWindownClip;
    public AudioClip MiniGunWindupClip;
    public AudioClip MiniGunCasingClip;
    public AudioClip MiniGunDryFireClip;
    public AudioClip MiniGunReloadClip;

    [Header("Knife Sounds")]
    public AudioClip KnifeStabClip;
    public AudioClip KnifeSliceClip;
    public AudioClip KnifeUnsheatheClip;
    public AudioClip KnifeSheatheClip;
    public AudioClip KnifeSharpeningClip;


    public void PlayShootingClip(AudioSource audioSource)
    {
        if (ShootingClip != null)
        {
            audioSource.PlayOneShot(ShootingClip, Volume);
        }
    }

    public void PlayLastBulletClip(AudioSource audioSource)
    {
        if (LastBulletClip != null)
        {
            audioSource.PlayOneShot(LastBulletClip, Volume);
        }
    }

    public void PlayReloadClip(AudioSource audioSource)
    {
        if (ReloadClip != null)
        {
            audioSource.PlayOneShot(ReloadClip, Volume);
        }
    }

    public void PlayDryFireClip(AudioSource audioSource)
    {
        if (DryFireClip != null)
        {
            audioSource.PlayOneShot(DryFireClip, Volume);
        }
    }

    public void PlayRackingClip(AudioSource audioSource)
    {
        if (RackingClip != null)
        {
            audioSource.PlayOneShot(RackingClip, Volume);
        }
    }

    public void PlayCasingClip(AudioSource audioSource)
    {
        if (CasingClip != null)
        {
            audioSource.PlayOneShot(CasingClip, Volume);
        }
    }

    public void PlayRocketFireClip(AudioSource audioSource)
    {
        if (RocketFireClip != null)
        {
            audioSource.PlayOneShot(RocketFireClip, Volume);
        }
    }

    public void PlayRocketReloadClip(AudioSource audioSource)
    {
        if (RocketReloadClip != null)
        {
            audioSource.PlayOneShot(RocketReloadClip, Volume);
        }
    }

    public void PlayRocketDryFireClip(AudioSource audioSource)
    {
        if (RocketDryFireClip != null)
        {
            audioSource.PlayOneShot(RocketDryFireClip, Volume);
        }
    }

    public void PlayRocketCasingClip(AudioSource audioSource)
    {
        if (RocketCasingClip != null)
        {
            audioSource.PlayOneShot(RocketCasingClip, Volume);
        }
    }

    public void PlayRocketExplosionClip(AudioSource audioSource)
    {
        if (RocketExplosionClip != null)
        {
            audioSource.PlayOneShot(RocketExplosionClip, Volume);
        }
    }

    public void PlayMiniGunFireLoopClip(AudioSource audioSource)
    {
        if (MiniGunFireLoopClip != null)
        {
            audioSource.clip = MiniGunFireLoopClip;
            audioSource.volume = Volume;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayMiniGunFireStopClip(AudioSource audioSource)
    {
        if (MiniGunFireStopClip != null)
        {
            audioSource.clip = MiniGunFireStopClip;
            audioSource.volume = Volume;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void PlayMiniGunWindDownClip(AudioSource audioSource)
    {
        if (MiniGunWindownClip != null)
        {
            audioSource.clip = MiniGunWindownClip;
            audioSource.volume = Volume;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void PlayMiniGunWindupClip(AudioSource audioSource)
    {
        if (MiniGunWindupClip != null)
        {
            audioSource.clip = MiniGunWindupClip;
            audioSource.volume = Volume;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void PlayMiniGunSpinLoopClip(AudioSource audioSource)
    {
        if (MiniGunSpinLoopClip != null)
        {
            audioSource.clip = MiniGunSpinLoopClip;
            audioSource.volume = Volume;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayMiniGunReloadClip(AudioSource audioSource)
    {
        if (MiniGunReloadClip != null)
        {
            audioSource.clip = MiniGunReloadClip;
            audioSource.volume = Volume;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void PlayMiniGunDryFireClip(AudioSource audioSource)
    {
        if (MiniGunDryFireClip != null)
        {
            audioSource.clip = MiniGunDryFireClip;
            audioSource.volume = Volume;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void PlayMiniGunCasingClip(AudioSource audioSource)
    {
        if (MiniGunCasingClip != null)
        {
            audioSource.clip = MiniGunCasingClip;
            audioSource.volume = Volume;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void PlayKnifeStabClip(AudioSource audioSource)
    {
        if (KnifeStabClip != null)
        {
            audioSource.PlayOneShot(KnifeStabClip, Volume);
        }
    }

    public void PlayKnifeSliceClip(AudioSource audioSource)
    {
        if (KnifeSliceClip != null)
        {
            audioSource.PlayOneShot(KnifeSliceClip, Volume);
        }
    }

    public void PlayKnifeUnsheatheClip(AudioSource audioSource)
    {
        if (KnifeUnsheatheClip != null)
        {
            audioSource.PlayOneShot(KnifeUnsheatheClip, Volume);
        }
    }

    public void PlayKnifeSheatheClip(AudioSource audioSource)
    {
        if (KnifeSheatheClip != null)
        {
            audioSource.PlayOneShot(KnifeSheatheClip, Volume);
        }
    }

    public void PlayKnifeSharpeningClip(AudioSource audioSource)
    {
        if (KnifeSharpeningClip != null)
        {
            audioSource.PlayOneShot(KnifeSharpeningClip, Volume);
        }
    }
}
