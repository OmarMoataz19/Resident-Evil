using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AssaultRifle,
    ShotGun,
}

[CreateAssetMenu(fileName = "Weapon Item", menuName = "Item/Create New Weapon Item")]
public class WeaponItem : InventoryItem
{
    public WeaponType WeaponType;
    public int Ammo;
}
