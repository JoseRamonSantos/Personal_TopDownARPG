using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SAttackStats
{
    public int m_damage;
    public float m_acc;
    public float m_critProb;
    public float m_critDamageMult;
}


public class AttackStats : MonoBehaviour
{
    public static SAttackStats ModifyBaseStats(SAttackStats _baseStats, SAttackStats _modStats)
    {
        SAttackStats newStats = _baseStats;

        newStats.m_damage = Mathf.RoundToInt(_baseStats.m_damage * _modStats.m_damage * 0.01f);
        newStats.m_acc += _modStats.m_acc;
        newStats.m_critProb += _modStats.m_critProb;
        newStats.m_critDamageMult += _modStats.m_critDamageMult;

        return newStats;
    }
}
