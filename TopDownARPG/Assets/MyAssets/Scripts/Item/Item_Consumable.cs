using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Consumable : Item_Base, IConsumable
{
    protected int m_healAmount = 1;


    public int HealAmount { get => m_healAmount; }


    public Item_Consumable(ItemConsumableData _data) : base(_data)
    {
        m_healAmount = _data.m_healAmount;
    }


    public override void Action()
    {
        Consume();
    }

    public void Consume()
    {
        Debug.Log(ItmName + " has been consumed.");
        Char_Player.Instance.Heal(m_healAmount);
        Inventory.Instance.RemoveItem(this);
    }
}
