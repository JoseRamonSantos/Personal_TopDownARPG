using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Loot : MonoBehaviour, IDisplayInfo
{
    [SerializeField]
    private ItemData m_lootData = null;

    public ItemData LootData { get => m_lootData; }



    public void SetLootData(ItemData _item)
    {
        m_lootData = _item;
    }

    public void StartHover()
    {
        //Debug.Log("Start hover: " + transform.name);
    }

    public void EndHover()
    {
        //Debug.Log("End hover: " + transform.name);
    }
}
