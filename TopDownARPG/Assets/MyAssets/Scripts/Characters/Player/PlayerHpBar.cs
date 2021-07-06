using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerHpBar : MonoBehaviour
{
    private Image m_hpBar = null;

    private void Awake()
    {
        m_hpBar = GetComponent<Image>();

    }

    private void Start()
    {
        Char_Player.Instance.OnHpChange += OnHPChange;

        HpArgs hpArgs = new HpArgs(Char_Player.Instance.CrntHp, Char_Player.Instance.MaxHp);
        OnHPChange(Char_Player.Instance, hpArgs);
    }

    private void OnHPChange(object _sender, HpArgs _args)
    {
        if (!m_hpBar) { return; }

        m_hpBar.fillAmount = (float)_args.m_crntHP / _args.m_maxHP;
    }
}
