using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentDisplay : MonoBehaviour
{
    public static PlayerEquipmentDisplay Instance = null;

    [SerializeField]
    private UnityEngine.GameObject m_inventoryPanel = null;

    [SerializeField]
    private List<PlayerEquipSlot> m_pInventorySlots = new List<PlayerEquipSlot>();



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_pInventorySlots.AddRange(m_inventoryPanel.GetComponentsInChildren<PlayerEquipSlot>());
        m_inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryVisibility();
        }
    }

    public void EquipItem(Item_Equipable _itm)
    {
        for (int i = 0; i < m_pInventorySlots.Count; i++)
        {
            if (m_pInventorySlots[i].EquipSlot == _itm.EquipSlot)
            {
                if(m_pInventorySlots[i].EquipedItem != null)
                {
                    DesequipItem(m_pInventorySlots[i]);
                }

                m_pInventorySlots[i].AddEquip(_itm);
                continue;
            }
        }

        Char_Player.Instance.AddEquipedItem(_itm);
    }

    public void DesequipItem(PlayerEquipSlot _slot)
    {
        if(_slot.EquipedItem == null) { return; }

        Item_Equipable itm = _slot.EquipedItem;
        itm.Unequip();

        Char_Player.Instance.RemoveEquipedItem(itm);

        _slot.RemoveEquip();
    }

    public void ToggleInventoryVisibility()
    {
        if (!m_inventoryPanel) { return; }

        bool crntState = m_inventoryPanel.activeSelf;

        m_inventoryPanel.SetActive(!crntState);
    }
}
