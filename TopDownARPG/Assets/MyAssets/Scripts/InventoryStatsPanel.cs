using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryStatsPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_damageText = null; 
    [SerializeField]
    private TextMeshProUGUI m_accText = null; 
    [SerializeField]
    private TextMeshProUGUI m_cProbText = null; 
    [SerializeField]
    private TextMeshProUGUI m_cMultText = null;
    [SerializeField]
    private TextMeshProUGUI m_armorText = null;


    private void Awake()
    {
        Char_Player.Instance.OnStatsChange += OnUpdateStats;
    }

    private void Start()
    {

    }

    private void OnUpdateStats(SAttackStats _aStats, int _armor)
    {
        m_damageText.text = _aStats.m_damage.ToString();
        m_armorText.text = _armor.ToString();
        m_accText.text = _aStats.m_acc.ToString() + "%";
        m_cProbText.text = _aStats.m_critProb.ToString() + "%";
        m_cMultText.text = _aStats.m_critDamageMult.ToString() + "%";
    }

}
