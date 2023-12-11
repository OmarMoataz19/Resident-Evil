using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TreasureType
{
    GoldBar,
    Ruby,
    Emerald
}

[CreateAssetMenu(fileName = "Treasure Item", menuName = "Item/Create New Treasure Item")]
public class TreasureItem : InventoryItem
{
    public TreasureType TreasureType;
}
