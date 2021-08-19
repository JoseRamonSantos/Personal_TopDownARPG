using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipSlot : MonoBehaviour
{
    [SerializeField]
    private Item_Equipable m_equipedItem = null;
    [SerializeField]
    private E_EQUIP_SLOT m_equipSlot = E_EQUIP_SLOT.HEAD;
    [SerializeField]
    private Image m_slotImg = null;
    [SerializeField]
    private Sprite m_slotEmptySprite = null;


    public E_EQUIP_SLOT EquipSlot { get => m_equipSlot; }
    public Item_Equipable EquipedItem { get => m_equipedItem; }

    private void Awake()
    {
        m_slotImg = GetComponent<Image>();
    }

    public void AddEquip(Item_Equipable equip)
    {
        if (m_equipedItem == null)
        {
            m_equipedItem = equip;
            m_slotImg.sprite = m_equipedItem.ItmIcon;
        }
        else
        {
            RemoveEquip();
            m_equipedItem = equip;
            m_slotImg.sprite = m_equipedItem.ItmIcon;
        }

    }

    public void RemoveEquip()
    {
        if (m_equipedItem != null)
        {
            Inventory.Instance.AddItem(m_equipedItem);
            m_equipedItem = null;
            m_slotImg.sprite = m_slotEmptySprite;
        }
    }
}
