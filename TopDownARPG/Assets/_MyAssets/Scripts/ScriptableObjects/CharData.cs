using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharData : ScriptableObject
{
    public string m_name = "";
    public Sprite m_icon = null;
    public int m_maxHp = 100;
    public int m_armor = 1;
    public float m_attackDistance = 3;
    public SAttackStats m_attackStatsBase;

}
