using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_HitStats_00 : Ability_HitStats
{
    #region VARIABLES

    #endregion

    #region METHODS
    /*protected override bool CanBeUsed()
    {
        return base.CanBeUsed() && m_cmpCharAttack.IsAttacking && m_cmpCharAttack.Target;
    }*/

    protected override void AbilityEffect()
    {
        Debug.Log("PLAYER USE ABILITY 1");
        
        base.AbilityEffect();
        
        m_cmpAnimator.Play("Ability01");

    }

    #endregion
}
