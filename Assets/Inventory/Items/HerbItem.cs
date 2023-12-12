using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HerbType
{
    Red,
    Green,
}

[CreateAssetMenu(fileName = "Herb Item", menuName = "Item/Create New Herb Item")]
public class HerbItem : InventoryItem
{
    public HerbType HerbType;
}
