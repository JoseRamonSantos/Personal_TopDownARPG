using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class EnemyHpBar : MonoBehaviour
{
    private Image m_hpBar = null;
    private Char_Enemy m_enemy = null;


    private void Awake()
    {
        m_hpBar = GetComponent<Image>();
        m_enemy = GetComponentInParent<Char_Enemy>();

        m_hpBar = GetComponent<Image>();

    }

    private void Start()
    {
        if (!m_enemy) { return; }

        m_enemy.OnHpChange += OnHPChange;

        HpArgs hpArgs = new HpArgs(m_enemy.CrntHp, m_enemy.MaxHp);
        OnHPChange(m_enemy, hpArgs);
    }

    private void OnHPChange(object _sender, HpArgs _args)
    {
        if (!m_hpBar) { return; }

        m_hpBar.fillAmount = (float)_args.m_crntHP / _args.m_maxHP;
    }
}
