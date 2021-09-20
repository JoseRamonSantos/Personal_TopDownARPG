using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_TimeEffect_10 : Ability_TimeEffect
{
    [SerializeField]
    private float m_aSpeedMultiplier = 2f;


    protected override void StartAbility()
    {
        Debug.Log(transform.name + ": Start Ability Effect " + name);

        float aSpeedM = m_cmpAnimator.GetFloat("AttackSpeed");
        aSpeedM *= m_aSpeedMultiplier;

        m_cmpAnimator.SetFloat("AttackSpeed", aSpeedM);
    }

    protected override void EndAbility()
    {
        Debug.Log(transform.name + ": End Ability Effect " + name);
        
        float aSpeedM = m_cmpAnimator.GetFloat("AttackSpeed");
        aSpeedM /= m_aSpeedMultiplier;

        m_cmpAnimator.SetFloat("AttackSpeed", aSpeedM);
    }
}