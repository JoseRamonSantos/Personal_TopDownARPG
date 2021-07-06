using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Equip/New Weapon", order = 2)]
public class ItemWeaponData : ItemEquipableData
{
    public SAttackStats m_statsMod;
    public float m_attackDistance = 3;
}
