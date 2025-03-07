using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    AnimatorManager animatorManager;
    WeaponLoaderSlot weaponLoaderSlot;

    [Header("Current Equipment")]
    public WeaponItem weapon;
    // public SubWeaponItem sunweapon; // Knife, stun grenade etc

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        LoadWeaponLoaderSlots();
    }

    private void Start()
    {
        LoadCurrentWeapon();
    }

    private void LoadWeaponLoaderSlots()
    {
        // Back Slot
        // Hip Slot
        weaponLoaderSlot = GetComponentInChildren<WeaponLoaderSlot>();
    }

    private void LoadCurrentWeapon()
    {
        // LOAD THE WEAPON ONTO OUR PLAYERS HAND
        weaponLoaderSlot.LoadWeaponModel(weapon);
        // CHANGE OUR PLAYERS MOVEMENT/IDLE ANIMATIONS TO THE WEAPONS MOVEMENT/IDLE ANIMATIONS
        animatorManager.animator.runtimeAnimatorController = weapon.weaponAnimator;
    }
}
