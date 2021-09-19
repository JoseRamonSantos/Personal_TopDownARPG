using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    protected Canvas m_canvas = null;

    protected CharAttack m_cmpAttack = null;
    [Header("Hp Info")]
    [SerializeField]
    protected GameObject m_hpInfoHeal = null;
    [SerializeField]
    protected GameObject m_hpInfoBasicHit = null;
    [SerializeField]
    protected GameObject m_hpInfoCriticalHit = null;
    [SerializeField]
    protected GameObject m_hpInfoMissHit = null;

    [Space]

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

    public Canvas Canvas
    {
        get
        {
            if (m_canvas == null)
            {
                m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

                if (m_canvas == null)
                {
                    Debug.LogError(transform.name + " CANVAS Missing");
                }
            }

            return m_canvas;
        }
    }
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
        E_HP_INFO_TYPE hitType;
        int damage = CalculateDamage(out hitType, OnAttackStats.Invoke());

        if (_target)
        {
            _target.TakeDamage(this, damage, hitType);
        }

        Debug.Log("-->" + transform.name + " deals " + damage + " damage " + "<---");
    }

    public virtual int CalculateDamage(out E_HP_INFO_TYPE _hitType, SAttackStats _attackStats)
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
            _hitType = E_HP_INFO_TYPE.BASIC_HIT;

            if (rndCriticalProb <= criticalProb)
            {
                Debug.Log(transform.name + " - critical hit ----------");
                _hitType = E_HP_INFO_TYPE.CRITICAL_HIT;

                damage = Mathf.RoundToInt(damage * criticalMultiplier * 0.01f);
            }
        }
        else
        {
            Debug.Log(transform.name + " - miss hit -----");
            _hitType = E_HP_INFO_TYPE.MISS_HIT;

            damage = 0;
        }

        return damage;
    }

    private SAttackStats BasicAttack()
    {
        return m_attackStatsTotal;
    }

    //HP
    public virtual void TakeDamage(Char_Base _owner, int _damage, E_HP_INFO_TYPE _hitType)
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
            SpawnHPInfo(HitInfoDisplay.position, damageTaken, _hitType);
        }

        if (_hitType != E_HP_INFO_TYPE.MISS_HIT)
        {
            if (this is Char_Enemy)
            {
                ConsoleManager.Instance.AddPlayerCauseDamage(this as Char_Enemy, damageTaken, _damage, damageAbsorved, _hitType);
            }
            else if (this is Char_Player)
            {
                ConsoleManager.Instance.AddPlayerReceiveDamage(_owner as Char_Enemy, damageTaken, _damage, damageAbsorved, _hitType);
            }
        }

    }

    public virtual void Heal(int _amount)
    {
        _amount = Mathf.Clamp(_amount, 0, int.MaxValue);

        SpawnHPInfo(transform.position, _amount);

        Debug.Log(transform.name + ": " + _amount + " HP restored");
        CrntHp += _amount;
    }

    public abstract void Die();

    protected void SpawnHPInfo(Vector3 _pos, int _ammount = 0, E_HP_INFO_TYPE _type = E_HP_INFO_TYPE.HEAL)
    {
        GameObject hpInfo = null;
        float delta = 0;

        switch (_type)
        {
            case E_HP_INFO_TYPE.HEAL:
                hpInfo = m_hpInfoHeal;
                delta = 10;
                break;

            case E_HP_INFO_TYPE.BASIC_HIT:
                delta = 5;
                hpInfo = m_hpInfoBasicHit;
                break;

            case E_HP_INFO_TYPE.CRITICAL_HIT:
                delta = 1;
                hpInfo = m_hpInfoCriticalHit;
                break;

            case E_HP_INFO_TYPE.MISS_HIT:
                delta = 5;
                hpInfo = m_hpInfoMissHit;
                break;
        }

        Debug.Log(transform.name + " 1");

        if (!hpInfo) { return; }

        Vector3 infoPos = Camera.main.WorldToScreenPoint(_pos);

        float rndDelta = Random.Range(-delta, delta);

        infoPos.x += rndDelta;
        infoPos.z = 0;

        GameObject hitinfo = Instantiate(hpInfo, infoPos, Quaternion.identity, Canvas.transform);

        if (_type == E_HP_INFO_TYPE.MISS_HIT)
        {
            hitinfo.GetComponent<TextMeshProUGUI>().text = "miss";
        }
        else
        {
            hitinfo.GetComponent<TextMeshProUGUI>().text = _ammount.ToString();
        }
    }


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
