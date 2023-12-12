using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum KeyType
{
    Emblem,
    KeyCard,
    Spade,
    Heart
}

[CreateAssetMenu(fileName = "Key Item", menuName = "Item/Create New Key Item")]
public class KeyItem : InventoryItem
{
    public KeyType KeyType;
}
