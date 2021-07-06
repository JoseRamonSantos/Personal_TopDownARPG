using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item_Base
{
    [SerializeField]
    protected string m_itmName = "";
    [SerializeField]
    protected Sprite m_itmIcon = null;

    public string ItmName { get => m_itmName; }
    public Sprite ItmIcon { get => m_itmIcon; }


    protected Item_Base(ItemData _data)
    {
        m_itmName = _data.m_itmName;
        m_itmIcon = _data.m_itmIcon;
    }


    public abstract void Action();
}

