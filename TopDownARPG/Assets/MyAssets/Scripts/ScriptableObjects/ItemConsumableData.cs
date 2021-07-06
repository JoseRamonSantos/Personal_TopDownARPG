using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable/New Consumable", order = 2)]
public class ItemConsumableData : ItemData
{
    public int m_healAmount = 10;
}
