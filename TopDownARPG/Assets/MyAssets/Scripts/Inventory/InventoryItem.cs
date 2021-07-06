using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    private Item_Base m_item = null;

    [SerializeField]
    private Image m_imgIcon = null;


    private void Awake()
    {
        m_imgIcon = GetComponent<Image>();
    }

    private void Start()
    {
        m_imgIcon.sprite = m_item.ItmIcon;
    }

    public void SetItem(Item_Base _item)
    {
        m_item = _item;
    }

    public void LeftClick()
    {
        m_item.Action();
        Destroy(this.gameObject);
    }
}
