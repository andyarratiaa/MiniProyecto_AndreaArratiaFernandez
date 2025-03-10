using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimatorManager : MonoBehaviour
{
    Animator weaponAnimator;

    [Header("Weapon FX")]
    public GameObject weaponMuzzleFlashFX; // The muzzle flash FX that is instantiated when the weapon is fired
    public GameObject weaponBulletCaseFX;  // The bullet case FX that is ejected from the weapon, when the weapon is fired

    [Header("Weapon FX Transforms")]
    public Transform weaponMuzzleFlashTransform; // The location the muzzle flash FX will instantiate
    public Transform weaponBulletCaseTransform;  // The location the bullet case will instantiate

    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
    }

    public void ShootWeapon(PlayerCamera playerCamera)
    {
        // ANIMATE THE WEAPON
        weaponAnimator.Play("Shoot");

        // INSTANTIATE MUZZLE FLASH FX
        GameObject muzzleFlash = Instantiate(weaponMuzzleFlashFX, weaponMuzzleFlashTransform);
        muzzleFlash.transform.parent = null;

        // INSTANTIATE EMPTY BULLET CASE
        GameObject bulletCase = Instantiate(weaponBulletCaseFX, weaponBulletCaseTransform);
        bulletCase.transform.parent = null;

        // SHOOT SOMETHING
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.cameraObject.transform.position, playerCamera.cameraObject.transform.forward, out hit))
        {
            Debug.Log(hit.transform.gameObject.name);
        }
    }

}

