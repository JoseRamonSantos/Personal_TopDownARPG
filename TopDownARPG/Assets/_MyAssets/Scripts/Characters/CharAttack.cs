using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharMovement))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Char_Base))]
public class CharAttack : MonoBehaviour
{
    private CharMovement m_cmpMovement = null;
    private Animator m_cmpAnimator = null;
    private Char_Base m_charOwner = null;

    [SerializeField]
    protected Char_Base m_target = null;
    [SerializeField]
    private float m_attackDistance = 3;

    [SerializeField]
    private bool m_isAttacking = false;


    private void Awake()
    {
        m_cmpMovement = GetComponent<CharMovement>();
        m_cmpAnimator = GetComponent<Animator>();
        m_charOwner = GetComponent<Char_Base>();
    }

    private void Update()
    {
        if (!m_target)
        {
            EndAttack();
            return;
        }

        if (!m_isAttacking)
        {
            if (m_cmpMovement.IsInTheDestination() && IsInAttackRange())
            {
                StartAttack();
            }
        }
        else if (!IsInAttackRange())
        {
            if (m_isAttacking)
            {
                EndAttack();
            }
        }
        else
        {
            m_cmpMovement.Track(m_target.transform);
        }
    }

    //Anim Event
    public void HitTarget()
    {
        Debug.Log("HIT TARGET " + transform.name);
        if (!IsInAttackRange()) { return; }

        E_HIT_TYPE hitType;

        m_target.TakeDamage(m_charOwner.CalculateDamage(out hitType), hitType);


    }

    private bool IsInAttackRange()
    {
        if (!m_target)
        {
            return false;
        }

        float d = (m_target.transform.position - transform.position).magnitude;

        return d <= m_attackDistance + m_attackDistance * 0.05f;
    }


    public void NewAttackTarget(Char_Base _target, float _aDistance)
    {
        if (!_target) { return; }
        
        m_cmpMovement.SetNewDestination(_target.transform, _aDistance);
        m_attackDistance = _aDistance;
        m_target = _target;
    }

    protected  void StartAttack()
    {
        m_isAttacking = true;
        m_cmpAnimator.SetBool("BasicAttack", m_isAttacking);
    }

    public void EndAttack()
    {
        m_isAttacking = false;
        m_cmpAnimator.SetBool("BasicAttack", m_isAttacking);
    }
}
