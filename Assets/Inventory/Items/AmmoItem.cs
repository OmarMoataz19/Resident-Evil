
using UnityEngine;


public enum AmmoType
{
    Pistol,
    Revolver,
    AssaultRifle,
    ShotGun
}

[CreateAssetMenu(fileName = "Ammo Item", menuName = "Item/Create New Ammo Item")]
public class AmmoItem : InventoryItem
{
    public AmmoType AmmoType;
    public int Amount;
}
