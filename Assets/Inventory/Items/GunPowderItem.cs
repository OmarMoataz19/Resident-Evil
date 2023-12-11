using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GunPowderType
{
    Normal,
    HighGrade
}

[CreateAssetMenu(fileName = "Gun Powder Item", menuName = "Item/Create New Gun Powder Item")]
public class GunPowderItem : InventoryItem
{
    public GunPowderType GunPowderType;
}
