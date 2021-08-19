using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Armor : Item_Equipable
{
    protected int m_armor = 1;

    public int Armor { get => m_armor; }

    public Item_Armor(ItemArmorData _data) : base(_data)
    {
        m_armor = _data.m_armor;
    }

}
