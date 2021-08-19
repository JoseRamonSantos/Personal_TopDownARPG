using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent (typeof(TextMeshProUGUI))]
public class PlayerCrntHpPotions : MonoBehaviour
{
    private TextMeshProUGUI m_txtCrntHpPotions;


    private void Awake()
    {
        m_txtCrntHpPotions = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        Inventory.Instance.OnUpdateHpPotions += OnUpdateCrntHpPotions;
        OnUpdateCrntHpPotions(Inventory.Instance.GetCrntHpPotions());
    }

    private void OnUpdateCrntHpPotions(int _crntHpPotions)
    {
        Debug.Log("ON UPDATE CRNT POTIONS");
        m_txtCrntHpPotions.text = _crntHpPotions.ToString();
    }
}
