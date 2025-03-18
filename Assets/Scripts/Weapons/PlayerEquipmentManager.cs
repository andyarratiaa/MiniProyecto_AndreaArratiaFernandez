using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    PlayerManager playerManager;
    WeaponLoaderSlot weaponLoaderSlot;

    [Header("Current Equipment")]
    public WeaponItem primaryWeapon; // Pistola
    public WeaponItem secondaryWeapon; // Rifle
    public WeaponItem currentWeapon;
    public int pistolAmmoInventory = 12; // Máximo de 12 para la pistola
    public int rifleAmmoInventory = 6;  // Máximo de 6 para el rifle
    public WeaponAnimatorManager weaponAnimator;
    RightHandIKTarget rightHandIK;
    LeftHandIKTarget leftHandIK;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        LoadWeaponLoaderSlots();
    }

    private void Start()
    {
        currentWeapon = primaryWeapon; // Inicia con la pistola
        LoadCurrentWeapon();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return; // ✅ Evita cambiar de arma en pausa

        if (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0)
        {
            SwitchWeapon();
        }
    }

    private void LoadWeaponLoaderSlots()
    {
        weaponLoaderSlot = GetComponentInChildren<WeaponLoaderSlot>();
    }

    private void LoadCurrentWeapon()
    {
        if (currentWeapon == null) return;

        weaponLoaderSlot.LoadWeaponModel(currentWeapon);
        playerManager.animatorManager.animator.runtimeAnimatorController = currentWeapon.weaponAnimator;

        weaponAnimator = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<WeaponAnimatorManager>();
        rightHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
        leftHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();

        playerManager.animatorManager.AssignHandIK(rightHandIK, leftHandIK);
        playerManager.playerUIManager.currentAmmoCountText.text = currentWeapon.remainingAmmo.ToString();

        // Actualizar el icono del arma en la UI
        playerManager.playerUIManager.UpdateWeaponUI(currentWeapon.weaponIcon, currentWeapon.weaponIconSize);

        if (playerManager.playerInventoryManager.currentAmmoInInventory != null)
        {
            if (playerManager.playerInventoryManager.currentAmmoInInventory.ammoType == currentWeapon.ammoType)
            {
                playerManager.playerUIManager.reservedAmmoCountText.text = playerManager.playerInventoryManager.currentAmmoInInventory.ammoRemaining.ToString();
            }
        }
    }

    public void SwitchWeapon()
    {
        currentWeapon = (currentWeapon == primaryWeapon) ? secondaryWeapon : primaryWeapon;
        LoadCurrentWeapon();
    }
}



