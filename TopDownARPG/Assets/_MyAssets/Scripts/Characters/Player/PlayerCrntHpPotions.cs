using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PlayerCrntHpPotions : MonoBehaviour
{
    #region VARIABLES
    private TextMeshProUGUI m_txtCrntHpPotions;
    #endregion


    #region METHODS
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
        m_txtCrntHpPotions.text = _crntHpPotions.ToString();
    }
    #endregion
}
