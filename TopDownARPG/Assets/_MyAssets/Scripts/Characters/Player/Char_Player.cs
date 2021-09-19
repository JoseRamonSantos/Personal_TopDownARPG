using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(CharMovement))]
[RequireComponent(typeof(PlayerEquipment))]
public class Char_Player : Char_Base
{
    public static Char_Player Instance = null;

    public delegate void CanvasHitFXHandler();
    public event CanvasHitFXHandler OnReceiveHit;
    
    public delegate void CanvasLowHpFXHandler();
    public event CanvasLowHpFXHandler OnLowHp;


    private NavMeshAgent m_cmpNVAgent = null;
    [SerializeField]
    protected HeroData m_data = null;

    public NavMeshAgent CmpNVAgent { get => m_cmpNVAgent; }

    protected override void Awake()
    {
        base.Awake();

        Instance = this;

        m_cmpNVAgent = GetComponent<NavMeshAgent>();

        m_maxHp = m_data.m_maxHp;
        ArmorBase = m_data.m_armor;
        m_attackStatsBase = m_data.m_attackStatsBase;
        m_baseAttackDistance = m_data.m_attackDistance;
        m_attackDistance = m_baseAttackDistance;

    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {

    }

    public override void Heal(int _amount)
    {
        base.Heal(_amount);
        ConsoleManager.Instance.AddPlayerHeal(_amount);
    }

    public override void Die()
    {
        /*GetComponent<Animator>().Play("Die", 0, 0);
        GetComponent<CharMovement>().StopMovement();
        GetComponent<PlayerController>().enabled = false;*/

        CursorManager.Instance.ChangeCursorTexture(E_CURSOR_MODE.DEFAULT);

        GameManager.Instance?.GameOver(false);

        WavesManager.Instance?.StopAllCoroutines();

        Destroy(this.gameObject);
    }

    public override void TakeDamage(Char_Base _owner, int _damage, E_HP_INFO_TYPE _hitType)
    {
        base.TakeDamage(_owner, _damage, _hitType);

        if(_hitType == E_HP_INFO_TYPE.MISS_HIT) { return; }

        OnReceiveHit?.Invoke();

        if(CrntHp <= MaxHp * 0.2f)
        {
            OnLowHp?.Invoke();
        }
    }
}
