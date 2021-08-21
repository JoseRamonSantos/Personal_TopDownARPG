using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_HitStats_01 : Ability_HitStats
{
    #region VARIABLES
    
    #endregion

    #region METHODS
    public override void Activate()
    {
        //if (m_cmpCharAttack.IsAttacking && m_cmpCharAttack.Target)
        //{
            base.Activate();
        //}
    }

    protected override void AbilityEffect()
    {
        Debug.Log("PLAYER USE ABILITY 1");
        
        base.AbilityEffect();
        
        m_cmpAnimator.Play("Ability01");

    }

    #endregion
}
