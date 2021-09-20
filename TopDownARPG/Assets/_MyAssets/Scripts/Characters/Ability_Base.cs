using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Ability_Base : MonoBehaviour
{
    #region VARIABLES
    public delegate void UpdateCooldownHandler(float _crntValue, float _maxValue);
    public event UpdateCooldownHandler OnUpdateCooldown;

    public delegate void ActivateButtonHandler();
    public event ActivateButtonHandler OnActivateButton;

    public delegate void DeactivateButtonHandler();
    public event DeactivateButtonHandler OnDeactivateButton;

    protected Char_Base m_cmpChar = null;
    protected Animator m_cmpAnimator = null;
    protected CharAttack m_cmpCharAttack = null;
    protected Button m_cmpButton = null;
    protected Image m_cmpImage = null;

    [SerializeField]
    protected string m_abilityName = "AbilityTest";

    [SerializeField]
    protected Sprite m_abilityIcon = null;

    [SerializeField]
    protected float m_maxCooldown = 5;
    [SerializeField]
    protected float m_crntCooldown = 0;

    [SerializeField]
    protected bool m_cooldownActive = false;
    #endregion


    #region PROPERTIES
    public float MaxCooldown { get => m_maxCooldown; }


    public bool CooldownActive { get => m_cooldownActive; }

    public Sprite AbilityIcon { get => m_abilityIcon; }

    public string AbilityName { get => m_abilityName; }
    public float CrntCooldown { get => m_crntCooldown; }
    #endregion


    #region METHODS
    protected virtual void Awake()
    {
        m_cmpChar = GetComponentInParent<Char_Base>();
        m_cmpAnimator = GetComponentInParent<Animator>();
        m_cmpCharAttack = GetComponentInParent<CharAttack>();
    }

    private void Start()
    {
        EndCooldown();
    }

    protected virtual void Update()
    {
        UpdateCooldown();
    }

    public void ActivateInput()
    {
        if (!CanBeUsed()) { return; }
        
        Activate();
    }

    protected virtual bool CanBeUsed()
    {
        return !m_cooldownActive;
    }

    protected virtual void Activate()
    {
        Debug.Log(transform.name + " - ACTIVATE");
        AbilityEffect();

        StartCooldown();
        DeactivateButton();
    }


    protected virtual void StartCooldown()
    {
        Debug.Log(transform.name + " START COOLDOWN");
        m_crntCooldown = m_maxCooldown;
        m_cooldownActive = true;
        OnUpdateCooldown?.Invoke(CrntCooldown, m_maxCooldown);
    }

    protected virtual void EndCooldown()
    {
        Debug.Log(transform.name + " END COOLDOWN");
        m_crntCooldown = 0;
        m_cooldownActive = false;
        OnUpdateCooldown?.Invoke(CrntCooldown, m_maxCooldown);
    }

    protected void UpdateCooldown()
    {
        if (m_cooldownActive)
        {
            m_crntCooldown -= Time.deltaTime;

            OnUpdateCooldown?.Invoke(CrntCooldown, m_maxCooldown);

            if (CrntCooldown <= 0)
            {
                EndCooldown();
                ActivateButton();
            }
        }
    }

    protected abstract void AbilityEffect();

    //BTN
    protected void ActivateButton()
    {
        OnActivateButton?.Invoke();
    }

    protected void DeactivateButton()
    {
        OnDeactivateButton?.Invoke();
    }
    #endregion
}
