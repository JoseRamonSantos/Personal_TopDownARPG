using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public abstract class Ability_Base : MonoBehaviour
{
    protected Image m_cmpImage = null;

    [SerializeField]
    protected string m_abilityName = "AbilityTest";

    protected Sprite m_abilityIcon = null;

    [SerializeField]
    protected float m_maxCooldown = 5;
    protected float m_crntCooldown = 0;

    protected bool m_cooldownActive = false;


    protected virtual void Awake()
    {
        m_cmpImage = GetComponentInChildren<Image>();
    }

    
    private void Update()
    {
        UpdateCooldown();
    }
    protected void UseAbility()
    {
        StartCooldown();
    }


    //COOLDOWN
    protected void StartCooldown()
    {
        //Deactivate Ability
        //Deactivate Icon

        m_crntCooldown = m_maxCooldown;
        m_cooldownActive = true;

    }

    protected void UpdateCooldown()
    {
        if (!m_cooldownActive) { return; }

        m_crntCooldown -= Time.deltaTime;

        if (m_crntCooldown < 0) { return; }

        EndCooldown();
    }

    protected void EndCooldown()
    {
        //Activate Ability
        //Activate Icon
        
        m_crntCooldown = m_maxCooldown;
        m_cooldownActive = false;
    }
}
