using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharAttack))]
public abstract class Char_Base : MonoBehaviour, IDisplayInfo
{
    #region VARIABLES
    public event EventHandler<HpArgs> OnHpChange;

    public delegate void StatsHandler(SAttackStats _aStats, int _armor);
    public event StatsHandler OnStatsChange;

    public delegate SAttackStats AttackStatsHandler();
    public event AttackStatsHandler OnAttackStats;

    protected CharAttack m_cmpAttack = null;

    [SerializeField]
    protected Transform m_hitInfoDisplay = null;
    [SerializeField]
    private bool m_isInvulnerable = false;
    [SerializeField]
    protected float m_baseAttackDistance = 2.5f;
    [SerializeField]
    protected float m_attackDistance = 2.5f;
    [SerializeField]
    protected int m_maxHp = 100;
    [SerializeField]
    protected int m_crntHp = 0;
    [SerializeField]
    protected SAttackStats m_attackStatsBase;
    [SerializeField]
    protected int m_armorBase = 1;

    [SerializeField]
    protected SAttackStats m_attackStatsTotal;
    [SerializeField]
    protected int m_armorTotal = 1;

    [SerializeField]
    private List<Item_Equipable> m_equipedItemsList = new List<Item_Equipable>();
    #endregion

    #region PROPERTIES
    public Transform HitInfoDisplay { get => m_hitInfoDisplay; }
    public int MaxHp { get => m_maxHp; }
    public int CrntHp
    {
        get { return m_crntHp; }
        set
        {
            m_crntHp = Mathf.Clamp(value, 0, MaxHp);

            HpArgs hpArgs = new HpArgs(m_crntHp, MaxHp);
            OnHpChange?.Invoke(this, hpArgs);
        }
    }
    public SAttackStats AttackStatsBase { get => m_attackStatsBase; }
    public int ArmorBase { get => m_armorBase; set => m_armorBase = Mathf.Clamp(value, 0, 100); }

    public SAttackStats AttackStatsTotal { get => m_attackStatsTotal; }
    public int ArmorTotal { get => m_armorTotal; set => m_armorTotal = Mathf.Clamp(value, 0, 100); }

    public float AttackDistance { get => m_attackDistance; }
    #endregion


    #region METHODS
    protected virtual void Awake()
    {
        m_cmpAttack = GetComponent<CharAttack>();
        OnAttackStats += BasicAttack;
    }

    protected virtual void Start()
    {
        m_attackStatsTotal = m_attackStatsBase;
        ArmorTotal = ArmorBase;

        OnStatsChange?.Invoke(m_attackStatsTotal, ArmorTotal);

        if (CrntHp == 0)
        {
            CrntHp = MaxHp;
        }
    }

    //DAMAGE
    public virtual void DoDamage(Char_Base _target)
    {
        E_HIT_TYPE hitType;
        int damage = CalculateDamage(out hitType, OnAttackStats.Invoke());

        if (_target)
        {
            _target.TakeDamage(damage, hitType);
        }

        Debug.Log("-->" + transform.name + " deals " + damage + " damage " + "<---");
    }
    /*public virtual void DoDamage(Char_Base _target, SAttackStats _attackStats)
    {
        E_HIT_TYPE hitType;
        int damage = CalculateDamage(out hitType, _attackStats);

        if (_target)
        {
            _target.TakeDamage(damage, hitType);
        }

        Debug.Log("-->" + transform.name + " deals " + damage + " damage " + "<---");
    }*/

    public virtual int CalculateDamage(out E_HIT_TYPE _hitType, SAttackStats _attackStats)
    {
        int damage = _attackStats.m_damage;

        float rndAcc = Random.value;
        float acc = _attackStats.m_acc * 0.01f;

        float rndCriticalProb = Random.value;
        float criticalProb = _attackStats.m_critProb * 0.01f;

        float criticalMultiplier = _attackStats.m_critDamageMult;
        
        /*Debug.Log("----------------------CALCULATE DAMAGE---------------------------");
        Debug.Log("-> BASE DAMAGE= " + damage);
        Debug.Log("-> ACC= " + acc + " / rnd= " + rndAcc);
        Debug.Log("-> CRITICAL PROB = " + criticalProb + " / rnd = " + rndCriticalProb);
        Debug.Log("-> CRITICAL MULTIPLPIER = " + criticalMultiplier);
        Debug.Log("-----------------------------------------------------------------");*/

        if (rndAcc <= acc)
        {
            _hitType = E_HIT_TYPE.BASIC;

            if (rndCriticalProb <= criticalProb)
            {
                Debug.Log(transform.name + " - critical hit ----------");
                _hitType = E_HIT_TYPE.CRITICAL;

                damage = Mathf.RoundToInt(damage * criticalMultiplier * 0.01f);
            }
        }
        else
        {
            Debug.Log(transform.name + " - miss hit -----");
            _hitType = E_HIT_TYPE.MISS;

            damage = 0;
        }

        return damage;
    }

    private SAttackStats BasicAttack()
    {
        return m_attackStatsTotal;
    }

    //HP
    public virtual void TakeDamage(int _damage, E_HIT_TYPE _hitType)
    {
        if (m_isInvulnerable) { return; }

        int damageAbsorved = Mathf.RoundToInt(ArmorTotal * 0.01f * (float)_damage);

        int damageTaken = Mathf.Clamp(_damage - damageAbsorved, 0, int.MaxValue);

        Debug.Log(transform.name + "has taken " + damageTaken + "(" + _damage + " - " + damageAbsorved + ") damage");

        CrntHp -= damageTaken;

        if (CrntHp == 0)
        {
            Die();
        }

        if (HitInfoDisplay)
        {
            GameManager.Instance.SpawnHitInfo(HitInfoDisplay.position, damageTaken, _hitType);
        }

    }

    public virtual void Heal(int _amount)
    {
        _amount = Mathf.Clamp(_amount, 0, int.MaxValue);

        GameManager.Instance.SpawnHealInfo(transform.position, _amount);

        Debug.Log(transform.name + ": " + _amount + " HP restored");
        CrntHp += _amount;
    }

    public abstract void Die();

    //STATS
    public void AddEquipedItem(Item_Equipable _itm)
    {
        m_equipedItemsList.Add(_itm);
        UpdateStats();
    }

    public void RemoveEquipedItem(Item_Equipable _itm)
    {
        m_equipedItemsList.Remove(_itm);
        UpdateStats();
    }

    private void UpdateStats()
    {
        m_attackStatsTotal = AttackStatsBase;
        ArmorTotal = ArmorBase;

        for (int i = 0; i < m_equipedItemsList.Count; i++)
        {
            Debug.Log("++++ " + m_equipedItemsList[i].ItmName);
            Item_Equipable crntItem = m_equipedItemsList[i];


            if (crntItem is Item_Weapon)
            {
                Item_Weapon itmWeapon = crntItem as Item_Weapon;
                Debug.Log(" ---------> " + itmWeapon.ItmName + " " + itmWeapon.StatsMod.m_damage);

                m_attackStatsTotal.m_damage += itmWeapon.StatsMod.m_damage;
                m_attackStatsTotal.m_acc += itmWeapon.StatsMod.m_acc;
                m_attackStatsTotal.m_critProb += itmWeapon.StatsMod.m_critProb;
                m_attackStatsTotal.m_critDamageMult = itmWeapon.StatsMod.m_critDamageMult;
                m_attackDistance = itmWeapon.AttackDistance;
            }
            else if (crntItem is Item_Armor)
            {
                Item_Armor itmArmor = crntItem as Item_Armor;

                ArmorTotal += itmArmor.Armor;
            }
        }

        OnStatsChange?.Invoke(m_attackStatsTotal, ArmorTotal);
    }

    //IDisplayInfo
    public void StartHover()
    {

    }

    public void EndHover()
    {

    }
    #endregion
}
