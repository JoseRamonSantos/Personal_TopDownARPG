using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance = null;

    //SLOTS
    [SerializeField]
    private UnityEngine.GameObject m_headParent = null;
    [SerializeField]
    private UnityEngine.GameObject m_chestParent = null;
    [SerializeField]
    private UnityEngine.GameObject m_pantsParent = null;
    [SerializeField]
    private UnityEngine.GameObject m_bootsParent = null;
    [SerializeField]
    private UnityEngine.GameObject m_mainHandParent = null;
    [SerializeField]
    private UnityEngine.GameObject m_offHandParent = null;

    //ITEMS REFS
    private UnityEngine.GameObject m_headItem = null;
    private UnityEngine.GameObject m_chestItem = null;
    private UnityEngine.GameObject m_pantsItem = null;
    private UnityEngine.GameObject m_bootsItem = null;
    private UnityEngine.GameObject m_mainHandItem = null;
    private UnityEngine.GameObject m_offHandItem = null;


    private void Awake()
    {
        Instance = this;
    }

    public void ShowItem(E_EQUIP_SLOT _slot, UnityEngine.GameObject _item)
    {
        switch (_slot)
        {
            case E_EQUIP_SLOT.HEAD:
                if (HasInvalidParent(ref m_headParent, "Head")) { return; }

                if (m_headItem) { Destroy(m_headItem); }

                m_headItem = Instantiate(_item, m_headParent.transform);
                break;
            case E_EQUIP_SLOT.CHEST:
                if (HasInvalidParent(ref m_chestParent, "Chest")) { return; }

                if (m_chestItem) { Destroy(m_chestItem); }
                
                m_chestItem = Instantiate(_item, m_chestParent.transform);
                break;
            case E_EQUIP_SLOT.PANTS:
                if (HasInvalidParent(ref m_pantsParent, "Pants")) { return; }

                if (m_pantsItem) { Destroy(m_pantsItem); }

                m_pantsItem = Instantiate(_item, m_pantsParent.transform);
                break;
            case E_EQUIP_SLOT.BOOTS:
                if (HasInvalidParent(ref m_bootsParent, "Boots")) { return; }

                if (m_bootsItem) { Destroy(m_bootsItem); }

                m_bootsItem = Instantiate(_item, m_bootsParent.transform);
                break;
            case E_EQUIP_SLOT.MAINHAND:
                if (HasInvalidParent(ref m_mainHandParent, "MainHand")) { return; }

                if (m_mainHandItem) { Destroy(m_mainHandItem); }

                m_mainHandItem = Instantiate(_item, m_mainHandParent.transform);
                break;
            case E_EQUIP_SLOT.OFFHAND:
                if (HasInvalidParent(ref m_offHandParent, "OffHand")) { return; }

                if (m_offHandItem) { Destroy(m_offHandItem); }

                m_offHandItem = Instantiate(_item, m_offHandParent.transform);
                break;
        }
    }

    private bool HasInvalidParent(ref UnityEngine.GameObject _go, string _name = "")
    {
        if (!_go)
        {
            Debug.LogWarning(transform.name + "(PlayerEquipment.cs): " + _name + " parent is never asigned)");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HideItem(E_EQUIP_SLOT _slot)
    {
        switch (_slot)
        {
            case E_EQUIP_SLOT.HEAD:
                if (m_headItem) { Destroy(m_headItem); }

                m_headItem = null;
                break;
            case E_EQUIP_SLOT.CHEST:
                if (m_chestItem) { Destroy(m_chestItem); }

                m_chestItem = null;
                break;
            case E_EQUIP_SLOT.PANTS:
                if (m_pantsItem) { Destroy(m_pantsItem); }

                m_pantsItem = null;
                break;
            case E_EQUIP_SLOT.BOOTS:
                if (m_bootsItem) { Destroy(m_bootsItem); }

                m_bootsItem = null;
                break;
            case E_EQUIP_SLOT.MAINHAND:
                if (m_mainHandItem) { Destroy(m_mainHandItem); }

                m_mainHandItem = null;
                break;
            case E_EQUIP_SLOT.OFFHAND:
                if (m_offHandItem) { Destroy(m_offHandItem); }

                m_offHandItem = null;
                break;
        }
    }
}
