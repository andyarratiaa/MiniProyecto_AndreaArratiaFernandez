//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerEquipmentManager : MonoBehaviour
//{
//    AnimatorManager animatorManager;
//    WeaponLoaderSlot weaponLoaderSlot;

//    [Header("Current Equipment")]
//    public WeaponItem weapon;
//    public WeaponAnimatorManager weaponAnimator;
//    RightHandIKTarget rightHandIK;
//    LeftHandIKTarget leftHandIK;
//    // public SubWeaponItem sunweapon; // Knife, stun grenade etc

//    private void Awake()
//    {
//        animatorManager = GetComponent<AnimatorManager>();
//        LoadWeaponLoaderSlots();
//    }

//    private void Start()
//    {
//        LoadCurrentWeapon();
//    }

//    private void LoadWeaponLoaderSlots()
//    {
//        // Back Slot
//        // Hip Slot
//        weaponLoaderSlot = GetComponentInChildren<WeaponLoaderSlot>();
//    }

//    private void LoadCurrentWeapon()
//    {
//        // LOAD THE WEAPON ONTO OUR PLAYERS HAND
//        weaponLoaderSlot.LoadWeaponModel(weapon);
//        // CHANGE OUR PLAYERS MOVEMENT/IDLE ANIMATIONS TO THE WEAPONS MOVEMENT/IDLE ANIMATIONS
//        animatorManager.animator.runtimeAnimatorController = weapon.weaponAnimator;
//        weaponAnimator = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<WeaponAnimatorManager>();
//        rightHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
//        leftHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();

//        animatorManager.AssignHandIK(rightHandIK, leftHandIK);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    AnimatorManager animatorManager;
    WeaponLoaderSlot weaponLoaderSlot;

    [Header("Current Equipment")]
    public WeaponItem primaryWeapon; // Pistola
    public WeaponItem secondaryWeapon; // Rifle
    public WeaponItem currentWeapon;
    public WeaponAnimatorManager weaponAnimator;
    RightHandIKTarget rightHandIK;
    LeftHandIKTarget leftHandIK;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        LoadWeaponLoaderSlots();
    }

    private void Start()
    {
        currentWeapon = primaryWeapon; // Inicia con la pistola
        LoadCurrentWeapon();
    }

    private void Update()
    {
        // Si se presiona la tecla "E", cambia de arma
        if (Input.GetKeyDown(KeyCode.T))
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

        // Cargar el arma en la mano del jugador
        weaponLoaderSlot.LoadWeaponModel(currentWeapon);

        // Asignar el Animator correspondiente al arma
        animatorManager.animator.runtimeAnimatorController = currentWeapon.weaponAnimator;

        weaponAnimator = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<WeaponAnimatorManager>();
        rightHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
        leftHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();

        animatorManager.AssignHandIK(rightHandIK, leftHandIK);
    }

    public void SwitchWeapon()
    {
        // Alternar entre la pistola y el rifle
        currentWeapon = (currentWeapon == primaryWeapon) ? secondaryWeapon : primaryWeapon;
        LoadCurrentWeapon();
    }

    public void ShootWeapon(PlayerCamera playerCamera)
    {
        if (weaponAnimator != null && currentWeapon != null)
        {
            weaponAnimator.ShootWeapon(playerCamera, currentWeapon);
        }
    }
}



