using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MixtureType
{
    GreenGreen,
    GreenRed,
}

[CreateAssetMenu(fileName = "Mixture Item", menuName = "Item/Create New Mixture Item")]
public class MixtureItem : InventoryItem
{
    public MixtureType MixtureType;
}
