using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinigunAudio : MonoBehaviour
{
    [SerializeField] private WeaponAudioSO weaponAudioSO;
    private AudioSource source;
    private EquipmentManager manager;
    private Inventory inventory;

    private bool isAiming = false;
    private bool isShooting = false;
    private bool isPlayingSpinLoop = false;
    private bool isPlayingShootLoop = false;


    public UnityEvent OnStartAiming;
    public UnityEvent OnStopAiming;
    public UnityEvent OnStartShooting;
    public UnityEvent OnStopShooting;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        manager = GetComponentInParent<EquipmentManager>();
        inventory = GetComponentInParent<Inventory>();
    }

    private void Update()
    {
        Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

        // Aiming
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            weaponAudioSO.PlayMiniGunWindupClip(source);
            OnStartAiming.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            weaponAudioSO.PlayMiniGunWindDownClip(source);
            OnStopAiming.Invoke();
        }

        // Shooting
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weaponAudioSO.PlayMiniGunWindupClip(source);
            OnStartShooting.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            
            weaponAudioSO.PlayMiniGunFireStopClip(source);
            OnStopShooting.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (isAiming && !isPlayingSpinLoop)
        {
            weaponAudioSO.PlayMiniGunSpinLoopClip(source);
            isPlayingSpinLoop = true;
        }
        else if (!isAiming && isPlayingSpinLoop)
        {
            weaponAudioSO.PlayMiniGunWindDownClip(source);
            isPlayingSpinLoop = false;
        }

        if (isShooting && !isPlayingShootLoop)
        {
            weaponAudioSO.PlayMiniGunFireLoopClip(source);
            isPlayingShootLoop = true;
        }
        else if (!isShooting && isPlayingShootLoop)
        {
            weaponAudioSO.PlayMiniGunFireStopClip(source);
            isPlayingShootLoop = false;
        }
    }

    public void StartAiming()
    {
        isAiming = true;
    }

    public void StopAiming()
    {
        isAiming = false;
    }

    public void StartShooting()
    {
        isShooting = true;
    }

    public void StopShooting()
    {
        isShooting = false;
    }
}