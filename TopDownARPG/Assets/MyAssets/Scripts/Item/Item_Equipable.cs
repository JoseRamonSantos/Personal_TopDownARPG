using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equipable : Item_Base, IEquipable
{
    private E_EQUIP_SLOT m_equipSlot = E_EQUIP_SLOT.HEAD;
    private UnityEngine.GameObject m_model = null;

    public E_EQUIP_SLOT EquipSlot { get => m_equipSlot; }


    public Item_Equipable(ItemEquipableData _data) : base(_data)
    {
        m_equipSlot = _data.m_equipSlot;
        m_model = _data.m_model;
    }


    public virtual void Equip()
    {
        PlayerEquipmentDisplay.Instance.EquipItem(this);
        Inventory.Instance.RemoveItem(this);
        if (m_model)
        {
            PlayerEquipment.Instance.ShowItem(m_equipSlot, m_model);
        }
        Debug.Log("Player: " + m_itmName + " has been equiped.");
    }

    public virtual void Unequip()
    {
        if (m_model)
        {
            PlayerEquipment.Instance.HideItem(m_equipSlot);
        }
        Debug.Log("Player: " + m_itmName + " has been unequiped.");
    }

    public override void Action()
    {
        Equip();
    }
}
