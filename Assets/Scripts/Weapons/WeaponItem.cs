using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    [Header("Weapon Info")]
    public string weaponName; // ← Ahora cada arma tendrá un nombre identificable

    [Header("Weapon Animation")]
    public AnimatorOverrideController weaponAnimator;

    [Header("Weapon Damage")]
    public int damage = 20;

    [Header("Ammo")]
    public int remainingAmmo;
    public int maxAmmo = 12; // Capacidad máxima del cargador

    public AmmoType ammoType;

    [Header("Weapon UI")]
    public Sprite weaponIcon;
    public Vector2 weaponIconSize = new Vector2(100, 100); // Tamaño por defecto
}

