using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability_TimeEffect : Ability_Base
{
    [SerializeField]
    protected float m_maxDuration = 3;

    [SerializeField]
    protected float m_crntDuration = 0;

    [SerializeField]
    protected Color m_defaultColor = Color.white;
    [SerializeField]
    protected Color m_activatedColor = Color.white;
    
    [SerializeField]
    protected bool m_isActivated = false;


    protected override void Update()
    {
        base.Update();

        UpdateDuration();
    }

    protected override bool CanBeUsed()
    {
        return !m_cooldownActive && !m_isActivated;
    }

    protected override void Activate()
    {
        AbilityEffect();
        DeactivateButton();
    }

    protected override void AbilityEffect()
    {
        m_crntDuration = m_maxDuration;
        m_isActivated = true;

        StartAbility();
    }

    protected abstract void StartAbility();

    protected abstract void EndAbility();

    public void UpdateDuration()
    {
        if (m_isActivated)
        {
            m_crntDuration -= Time.deltaTime;

            if (m_crntDuration <= 0)
            {
                m_crntDuration = m_maxDuration;
                m_isActivated = false;
                
                EndAbility();

                StartCooldown();
            }
        }
    }
}
