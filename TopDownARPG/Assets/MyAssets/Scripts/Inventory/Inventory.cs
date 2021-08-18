using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance = null;

    public delegate void StatsHandler(int _crntHpPotions);
    public event StatsHandler OnUpdateHpPotions;

    [SerializeField]
    private E_DISPLAY_FILTER m_displayFilter = E_DISPLAY_FILTER.ALL;

    [SerializeField]
    private List<ItemData> m_initialItemsList = new List<ItemData>();

    [SerializeField]
    private Transform m_content;

    [SerializeField]
    private GameObject m_itemBtnPref;

    [SerializeField]
    private List<Item_Base> m_inventory = new List<Item_Base>();
    [SerializeField]
    private List<GameObject> m_displayedItems = new List<GameObject>();


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < m_initialItemsList.Count; i++)
        {
            AddItem(m_initialItemsList[i]);
        }

        UpdateDisplayedItems();
    }

    //Items managment
    public void AddItem(ItemData _data)
    {
        Item_Base newItem = null;

        if (_data is ItemConsumableData)
        {
            newItem = new Item_Consumable((ItemConsumableData)_data);
        }
        else if (_data is ItemArmorData)
        {
            newItem = new Item_Armor((ItemArmorData)_data);
        }
        else if (_data is ItemWeaponData)
        {
            newItem = new Item_Weapon((ItemWeaponData)_data);
        }

        if (newItem != null)
        {
            AddItem(newItem);
        }
    }

    public void AddItem(Item_Base _item)
    {
        m_inventory.Add(_item);
        UpdateDisplayedItems();

        if (_item is Item_Consumable)
        {
            OnUpdateHpPotions?.Invoke(GetCrntHpPotions());
        }
    }

    public void RemoveItem(Item_Base _item)
    {

        m_inventory.Remove(_item);
        UpdateDisplayedItems();
        
        if (_item is Item_Consumable)
        {
            OnUpdateHpPotions?.Invoke(GetCrntHpPotions());
        }
    }

    public void ShowItem(Item_Base _item)
    {
        UnityEngine.GameObject go = Instantiate(m_itemBtnPref, m_content);
        m_displayedItems.Add(go);
        go.GetComponent<InventoryItem>().SetItem(_item);
    }

    //Quick Use
    public void QuickUseHpPotion()
    {
        if (Char_Player.Instance.CrntHp == Char_Player.Instance.MaxHp) { return; }

        for (int i = m_inventory.Count - 1; i >= 0; i--)
        {
            if (m_inventory[i] is Item_Consumable)
            {
                (m_inventory[i] as Item_Consumable).Consume();
                return;
            }
        }
    }

    //Display
    private void UpdateDisplayedItems()
    {
        EmptyAllDisplay();

        switch (m_displayFilter)
        {
            case E_DISPLAY_FILTER.ALL:
                for (int i = 0; i < m_inventory.Count; i++)
                {
                    ShowItem(m_inventory[i]);
                }
                break;
            case E_DISPLAY_FILTER.CONSUMABLES:
                for (int i = 0; i < m_inventory.Count; i++)
                {
                    if (m_inventory[i] is Item_Consumable)
                    {
                        ShowItem(m_inventory[i]);
                    }
                }
                break;
            case E_DISPLAY_FILTER.EQUIPABLES:
                for (int i = 0; i < m_inventory.Count; i++)
                {
                    if (m_inventory[i] is Item_Equipable)
                    {
                        ShowItem(m_inventory[i]);
                    }
                }
                break;
        }
    }

    public void ShowAllItems()
    {
        m_displayFilter = E_DISPLAY_FILTER.ALL;
        UpdateDisplayedItems();
    }
    public void ShowConsumableItems()
    {
        m_displayFilter = E_DISPLAY_FILTER.CONSUMABLES;
        UpdateDisplayedItems();
    }
    public void ShowEquipableItems()
    {
        m_displayFilter = E_DISPLAY_FILTER.EQUIPABLES;
        UpdateDisplayedItems();
    }

    private void EmptyAllDisplay()
    {
        for (int i = 0; i < m_displayedItems.Count; i++)
        {
            Destroy(m_displayedItems[i].gameObject);
        }

        m_displayedItems.Clear();
    }

    public int GetCrntHpPotions()
    {
        int x = 0;

        for (int i = 0; i < m_inventory.Count; i++)
        {
            if(m_inventory[i] is Item_Consumable)
            {
                x++;
            }
        }

        Debug.Log("CRNT POTIONS: " + x);
        return x;
    }
}
