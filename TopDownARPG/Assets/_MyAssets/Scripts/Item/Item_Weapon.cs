using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon : Item_Equipable
{
    protected SAttackStats m_statsMod;
    protected float m_attackDistance = 2.5f;


    public SAttackStats StatsMod { get => m_statsMod; }
    public float AttackDistance { get => m_attackDistance; set => m_attackDistance = value; }

    public Item_Weapon(ItemWeaponData _data) : base(_data)
    {
        m_statsMod = _data.m_statsMod;
        m_attackDistance = _data.m_attackDistance;
    }
}
