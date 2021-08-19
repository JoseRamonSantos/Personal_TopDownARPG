using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SEnemyLoot
{
    public List<ItemData> m_itemData;
    public bool m_unique;
    public float m_probability;
}


[RequireComponent(typeof(CharMovement))]
public class Char_Enemy : Char_Base
{
    [SerializeField]
    protected List<SEnemyLoot> m_lootData = new List<SEnemyLoot>();
    protected E_ENEMY_TYPE m_type = E_ENEMY_TYPE.BASIC;
    protected E_ENEMY_TIER m_tier = E_ENEMY_TIER.C;

    [SerializeField]
    protected EnemyData m_data = null;

    public E_ENEMY_TYPE Type { get => m_type; }
    public E_ENEMY_TIER Tier { get => m_tier; }


    protected override void Awake()
    {
        base.Awake();

        m_maxHp = m_data.m_maxHp;
        ArmorBase = m_data.m_armor;
        m_attackStatsBase = m_data.m_attackStatsBase;
        m_baseAttackDistance = m_data.m_attackDistance;
        m_attackDistance = m_baseAttackDistance;
        m_lootData = m_data.m_lootData;
        m_type = m_data.m_type;
        m_tier = m_data.m_tier;
    }

    protected override void Start()
    {
        base.Start();

        WavesManager.Instance.AddEnemy(this);

        m_cmpAttack.NewAttackTarget(Char_Player.Instance, AttackDistance);
    }

    public override void Die()
    {
        DropLoot();
        WavesManager.Instance.RemoveEnemy(this);
        Destroy(this.gameObject);
    }

    private void DropLoot()
    {
        if (m_lootData.Count == 0) { return; }

        float rndValue = Random.value;

        List<ItemData> itemsToDrop = new List<ItemData>();

        int index = -1;
        float minValue = float.MaxValue;
        
        //Get the best posible loot
        for (int i = 0; i < m_lootData.Count; i++)
        {
            float crntValue = m_lootData[i].m_probability * 0.01f;

            if (rndValue <= crntValue && crntValue <= minValue)
            {
                minValue = crntValue;
                index = i;
            }
        }

        //Asign loot
        if (index >= 0)
        {
            if (m_lootData[index].m_unique)
            {
                int rndItm = Random.Range(0, m_lootData[index].m_itemData.Count);
                itemsToDrop.Add(m_lootData[index].m_itemData[rndItm]);
            }
            else
            {
                for (int i = 0; i < m_lootData[index].m_itemData.Count; i++)
                {
                    itemsToDrop.Add(m_lootData[index].m_itemData[i]);
                }
            }
        }

        //Drop loot
        for (int i = 0; i < itemsToDrop.Count; i++)
        {
            GameManager.Instance.DropItem(itemsToDrop[i], transform.position, m_data.m_name);
        }
    }
}
