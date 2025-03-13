using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimatorManager : MonoBehaviour
{
    PlayerManager player;
    Animator weaponAnimator;

    [Header("Weapon FX")]
    public GameObject weaponMuzzleFlashFX; // Efecto de disparo
    public GameObject weaponBulletCaseFX;  // Casquillo de bala expulsado

    [Header("Weapon FX Transforms")]
    public Transform weaponMuzzleFlashTransform; // Posición del efecto de disparo
    public Transform weaponBulletCaseTransform;  // Posición del casquillo de bala

    [Header("Weapon Bullet Range")]
    public float bulletRange = 200f;

    [Header("Shootable Layers")]
    public LayerMask shootableLayers;



    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
        player = GetComponentInParent<PlayerManager>();
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
        if (Physics.Raycast(playerCamera.cameraObject.transform.position, playerCamera.cameraObject.transform.forward, out hit, bulletRange, shootableLayers))
        {
            Debug.Log(hit.collider.gameObject.layer);
            ZombieEffectManager zombie = hit.collider.gameObject.GetComponentInParent<ZombieEffectManager>();

            if (zombie != null)
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    zombie.DamageZombieHead(player.playerEquipmentManager.currentWeapon.damage);
                }
                else if (hit.collider.gameObject.layer == 9)
                {
                    zombie.DamageZombieTorso(player.playerEquipmentManager.currentWeapon.damage);
                }
                else if (hit.collider.gameObject.layer == 10)
                {
                    zombie.DamageZombieRightArm(player.playerEquipmentManager.currentWeapon.damage);
                }
                else if (hit.collider.gameObject.layer == 11)
                {
                    zombie.DamageZombieLeftArm(player.playerEquipmentManager.currentWeapon.damage);
                }
                else if (hit.collider.gameObject.layer == 12)
                {
                    zombie.DamageZombieRightLeg(player.playerEquipmentManager.currentWeapon.damage);
                }
                else if (hit.collider.gameObject.layer == 13)
                {
                    zombie.DamageZombieLeftLeg(player.playerEquipmentManager.currentWeapon.damage);
                }
            }
        }
    }
}

