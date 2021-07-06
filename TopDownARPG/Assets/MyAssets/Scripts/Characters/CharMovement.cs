using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class CharMovement : MonoBehaviour
{
    private NavMeshAgent m_cmpAgent = null;
    private Animator m_cmpAnimator = null;

    [SerializeField]
    private Vector3 m_destination = Vector3.zero;

    [SerializeField]
    private Transform m_target = null;

    [SerializeField]
    private bool m_moveTo = false;

    private float m_speedRaw = 0;
    private float m_speedSmooth = 0;

    private void Awake()
    {
        m_cmpAgent = GetComponent<NavMeshAgent>();
        m_cmpAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnim();

        if (m_target)
        {
            m_destination = m_target.position;
        }

        if (m_moveTo)
        {
            m_cmpAgent.SetDestination(m_destination);

            if (IsInTheDestination() && !m_target)
            {
                m_moveTo = false;
            }
        }
    }

    public void Track(Transform _target)
    {
        Vector3 lookDirection = _target.position - transform.position;
        lookDirection.Normalize();

        Quaternion crntDirection = transform.rotation;
        Quaternion desiredDirection = Quaternion.LookRotation(lookDirection);

        float rSpeed = Time.deltaTime * m_cmpAgent.angularSpeed;

        transform.rotation = Quaternion.RotateTowards(crntDirection, desiredDirection, rSpeed);
    }

    private void UpdateAnim()
    {
        if (!m_cmpAnimator) { return; }

        m_speedRaw = m_cmpAgent.velocity.normalized.magnitude;

        m_speedSmooth = Mathf.Lerp(m_speedSmooth, m_speedRaw, Time.deltaTime * m_cmpAgent.acceleration);

        m_cmpAnimator.SetFloat("Speed", m_speedSmooth);
    }

    public bool IsInTheDestination()
    {
        return !m_cmpAgent.pathPending && m_cmpAgent.remainingDistance <= m_cmpAgent.stoppingDistance;
    }

    public void SetNewDestination(Vector3 _destination, float _sDistance = 1f)
    {
        m_moveTo = true;
        m_target = null;
        m_destination = _destination;
        m_cmpAgent.stoppingDistance = _sDistance;
        m_cmpAgent.SetDestination(m_destination);
    }

    public void SetNewDestination(Transform _target, float _sDistance = 1f)
    {
        m_moveTo = true;
        m_target = _target;
        m_cmpAgent.stoppingDistance = _sDistance;
        m_destination = m_target.position;
        m_cmpAgent.SetDestination(m_destination);
    }

    public void StopMovement()
    {
        m_cmpAgent.ResetPath();
        m_moveTo = false;
        m_target = null;
    }
}
