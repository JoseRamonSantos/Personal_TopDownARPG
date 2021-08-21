using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public abstract class Ability_Base : MonoBehaviour
{
    #region VARIABLES
    protected Char_Base m_cmpChar = null;
    protected Animator m_cmpAnimator = null;
    protected CharAttack m_cmpCharAttack = null;

    [SerializeField]
    protected string m_abilityName = "AbilityTest";

    [SerializeField]
    protected Sprite m_abilityIcon = null;

    [SerializeField]
    protected SAttackStats m_attackStatsMod;

    [SerializeField]
    protected float m_maxCooldown = 5;
    [SerializeField]
    protected float m_crntCooldown = 0;

    [SerializeField]
    protected bool m_cooldownActive = false;
    #endregion


    #region PROPERTIES
    public float MaxCooldown { get => m_maxCooldown; }

    public float CrntCooldown { get =>  m_crntCooldown; }

    public bool CooldownActive { get => m_cooldownActive; }

    public Sprite AbilityIcon { get => m_abilityIcon; }

    public string AbilityName { get => m_abilityName; }
    #endregion


    #region METHODS
    protected virtual void Awake()
    {
        m_cmpChar = GetComponentInParent<Char_Base>();
        m_cmpAnimator = GetComponentInParent<Animator>();
        m_cmpCharAttack = GetComponentInParent<CharAttack>();
    }

    protected virtual void Update()
    {
        UpdateCooldown();
    }

    public virtual void Activate()
    {
        AbilityEffect();

        m_crntCooldown = m_maxCooldown;
        m_cooldownActive = true;
    }

    protected void UpdateCooldown()
    {
        if (!m_cooldownActive) { return; }

        m_crntCooldown -= Time.deltaTime;

        if (m_crntCooldown > 0) { return; }
        
        m_cooldownActive = false;
    }

    protected abstract void AbilityEffect();
    #endregion
}
