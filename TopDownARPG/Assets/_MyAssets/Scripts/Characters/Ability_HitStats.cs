using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability_HitStats : Ability_Base
{
    #region VARIABLES
    [SerializeField]
    protected int m_numberOfHits = 1;

    protected int m_crntHits = 0;
    #endregion


    #region METHODS
    protected override void AbilityEffect()
    {
        m_cmpChar.OnAttackStats += OnAbility01;

        m_crntHits = 0;
    }

    protected virtual SAttackStats OnAbility01()
    {
        m_crntHits++;

        if (m_crntHits >= m_numberOfHits)
        {
            m_cmpChar.OnAttackStats -= OnAbility01;
        }

        return AttackStats.ModifyBaseStats(m_cmpChar.AttackStatsTotal, m_attackStatsMod);
    }
    #endregion
}
