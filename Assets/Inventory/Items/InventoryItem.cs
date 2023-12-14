using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum InventoryItemType
{
    Weapon,
    Ammo,
    Grenade,
    Key,
    Herbs,
    GunPowder,
    Mixture,
    Treasure
}


[CreateAssetMenu(fileName ="Inventory Item", menuName="Item/Create New Item")]
public class InventoryItem : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite icon;
    public InventoryItemType ItemType;
    public int baseCost;
    public int sellPrice;
}
