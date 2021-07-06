using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Char/New Enemy", order = 2)]
public class EnemyData : CharData
{
    public List<SEnemyLoot> m_lootData = new List<SEnemyLoot>();
    public E_ENEMY_TYPE m_type = E_ENEMY_TYPE.BASIC;
    public E_ENEMY_TIER m_tier = E_ENEMY_TIER.C;
    public Char_Enemy m_enemyPrefab = null;
}
