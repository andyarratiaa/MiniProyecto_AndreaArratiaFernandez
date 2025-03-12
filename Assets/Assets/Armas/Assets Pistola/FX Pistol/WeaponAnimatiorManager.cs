using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimatorManager : MonoBehaviour
{
    Animator weaponAnimator;

    [Header("Weapon FX")]
    public GameObject weaponMuzzleFlashFX; // Efecto de disparo
    public GameObject weaponBulletCaseFX;  // Casquillo de bala expulsado

    [Header("Weapon FX Transforms")]
    public Transform weaponMuzzleFlashTransform; // Posición del efecto de disparo
    public Transform weaponBulletCaseTransform;  // Posición del casquillo de bala

    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
    }

    public void ShootWeapon(PlayerCamera playerCamera, WeaponItem currentWeapon)
    {
        // ANIMACIÓN SEGÚN EL ARMA ACTUAL
        if (currentWeapon != null)
        {
            if (currentWeapon.weaponName == "Rifle") // Usa el nombre del rifle en el script WeaponItem
            {
                weaponAnimator.Play("Rifle");
            }
            else
            {
                weaponAnimator.Play("Shoot"); // Animación por defecto (pistola)
            }
        }

        // INSTANTIATE MUZZLE FLASH FX
        GameObject muzzleFlash = Instantiate(weaponMuzzleFlashFX, weaponMuzzleFlashTransform);
        muzzleFlash.transform.parent = null;

        // INSTANTIATE EMPTY BULLET CASE
        GameObject bulletCase = Instantiate(weaponBulletCaseFX, weaponBulletCaseTransform);
        bulletCase.transform.parent = null;

        // DISPARO Y DETECCIÓN DE COLISIÓN
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.cameraObject.transform.position, playerCamera.cameraObject.transform.forward, out hit))
        {
            Debug.Log(hit.transform.gameObject.name);
        }
    }
}

