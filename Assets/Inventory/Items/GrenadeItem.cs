using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GrenadeType
{
    Hand,
    Flash
}

[CreateAssetMenu(fileName = "Grenade Item", menuName = "Item/Create New Grenade Item")]
public class GrenadeItem : InventoryItem
{
    public GrenadeType GrenadeType;

}
