using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Box Of Ammo Item")]
public class BoxOfAmmoItem : Item
{
    public AmmoType ammoType;
    public int ammoRemaining = 50;
    public int boxOfAmmoCapacity = 50;
}
