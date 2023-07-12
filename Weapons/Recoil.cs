using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private Inventory inventory;
    private EquipmentManager manager;
    private AimDownSights aimDownSights;

    /*[Header("Non-Aiming Recoil")]     // these wont be used since i have made them in the Weapon Scriptable Object script instead of here
    [Tooltip("The amount of recoil applied on the X-axis when the weapon is not aimed")]
    public float recoilX;

    [Tooltip("The amount of recoil applied on the Y-axis when the weapon is not aimed")]
    public float recoilY;

    [Tooltip("The amount of recoil applied on the Z-axis when the weapon is not aimed")]
    public float recoilZ;

    [Header("Aiming Recoil")]
    [Tooltip("The amount of recoil applied on the X-axis when the weapon is aimed")]
    public float aimRecoilX;

    [Tooltip("The amount of recoil applied on the Y-axis when the weapon is aimed")]
    public float aimRecoilY;

    [Tooltip("The amount of recoil applied on the Z-axis when the weapon is aimed")]
    public float aimRecoilZ;*/

    [Header("Other Recoil Settings")]
    [Tooltip("The speed at which the recoil snaps back to zero rotation")]
    [SerializeField] private float snappiness;

    [Tooltip("The speed at which the weapon's rotation returns to its original position")]
    [SerializeField] private float returnSpeed;

    public bool isAiming = false;

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
        //RecoilFire(recoilX, recoilY, recoilZ);    /// im not 100% sure if that supposed to be here
    }

    public void RecoilFire()
    {
        inventory = GetComponentInParent<Inventory>();
        manager = GetComponentInParent<EquipmentManager>();
        aimDownSights = GetComponentInChildren<AimDownSights>();

        Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

        if (aimDownSights.isAiming)
        {
            targetRotation += new Vector3(currentWeapon.aimingRecoilX, Random.Range(-currentWeapon.aimingRecoilY, currentWeapon.aimingRecoilY), Random.Range(-currentWeapon.aimingRecoilZ, currentWeapon.aimingRecoilZ));
        }
        else
        {
            targetRotation += new Vector3(currentWeapon.recoilX, Random.Range(-currentWeapon.recoilY, currentWeapon.recoilY), Random.Range(-currentWeapon.recoilZ, currentWeapon.recoilZ));
        }
    }
}