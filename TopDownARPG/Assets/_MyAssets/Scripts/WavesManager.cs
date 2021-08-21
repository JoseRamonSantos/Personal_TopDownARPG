using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[System.Serializable]
public struct STierInfo
{
    public E_ENEMY_TIER m_tier;
    [Range(0, 1)]
    public float m_percent;
}

[System.Serializable]
public struct SWavesInfo
{
    public int m_enemiesPerWave;
    public E_ENEMY_TIER m_tier;
    public float m_minSpawnTime;
    public float m_maxSpawnTime;
    public int m_maxEnemies;
    public bool m_finalBoss;
}



public class WavesManager : MonoBehaviour
{
    public static WavesManager Instance = null;

    [SerializeField]
    private float m_initWaitTime = 2;
    [SerializeField]
    private float m_crntIWT = 0;
    [SerializeField]
    private float m_nextRoundWaitTime = 5;

    [SerializeField]
    private SWavesInfo[] m_wavesInfo;

    private List<GenericEnemyQueue<EnemyData>> m_wavesList = new List<GenericEnemyQueue<EnemyData>>();

    [SerializeField]
    private int m_iWaves = 0;

    private List<List<EnemyData>> m_basicEnemiesListPerTier = new List<List<EnemyData>>();
    [SerializeField]
    private List<List<EnemyData>> m_bossesListPerTier = new List<List<EnemyData>>();

    private EnemyData[] m_enemiesList;

    [SerializeField]
    private List<Char_Enemy> m_crntEnemiesList = null;
    [SerializeField]
    private int m_crntEnemies = 0;
    [SerializeField]
    private int m_totalEnemies = 0;

    public int IWaves { get => m_iWaves; }



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GetEnemies();
        GenerateWaves();

        PlayerWavesInfo.Instance?.StartWaitTime(m_initWaitTime);

        UpdateWavesInfo();
    }

    private void Update()
    {
        //Spawn managment
        if (m_crntIWT < m_initWaitTime)
        {
            m_crntIWT += Time.deltaTime;

            if (m_crntIWT >= m_initWaitTime)
            {
                Debug.Log("---------- START ----------");

                PlayerWavesInfo.Instance?.EndWaitTime();

                SpawnEnemy();
            }

            return;
        }
    }

    private void GetEnemies()
    {
        m_enemiesList = Resources.LoadAll<EnemyData>("Characters/Enemies");

        string[] tierEnum = Enum.GetNames(typeof(E_ENEMY_TIER));

        //Clasificate the enemies in their category list (Basic of FinalBoss) per tiers (Index 0: Tier A --> Index 2: Tier C)
        for (int i = 0; i < tierEnum.Length; i++)
        {
            m_basicEnemiesListPerTier.Add(new List<EnemyData>());
            m_bossesListPerTier.Add(new List<EnemyData>());

            for (int j = 0; j < m_enemiesList.Length; j++)
            {
                if (m_enemiesList[j].m_tier.ToString().Equals(tierEnum[i], System.StringComparison.Ordinal))
                {
                    if (m_enemiesList[j].m_type == E_ENEMY_TYPE.BASIC)
                    {
                        m_basicEnemiesListPerTier[i].Add(m_enemiesList[j]);
                    }
                    else if (m_enemiesList[j].m_type == E_ENEMY_TYPE.FINAL_BOSS)
                    {
                        m_bossesListPerTier[i].Add(m_enemiesList[j]);
                    }
                }
            }
        }
    }

    void GenerateWaves()
    {
        for (int i = 0; i < m_wavesInfo.Length; i++)
        {
            m_wavesList.Add(new GenericEnemyQueue<EnemyData>());

            int maxEnemies = m_wavesInfo[i].m_enemiesPerWave;

            if (m_wavesInfo[i].m_finalBoss)
            {
                maxEnemies -= 1;
            }

            //List with the posible enemies per wave
            int iTier = (byte)m_wavesInfo[i].m_tier;

            for (int j = 0; j < maxEnemies; j++)
            {

                int rndEnemy = Random.Range(0, m_basicEnemiesListPerTier[iTier].Count);
                m_wavesList[i].Enqueue(m_basicEnemiesListPerTier[iTier][rndEnemy]);
                Debug.Log(i + ": " + m_basicEnemiesListPerTier[iTier][rndEnemy].m_name);
            }

            if (m_wavesInfo[i].m_finalBoss)
            {
                int rndBoss = Random.Range(0, m_bossesListPerTier[iTier].Count);
                m_wavesList[i].Enqueue(m_bossesListPerTier[iTier][rndBoss]);
                Debug.Log(i + ": " + m_bossesListPerTier[iTier][rndBoss].m_name);
            }
        }
    }

    private bool EnemiesWaveFinished()
    {
        return !m_wavesList[IWaves].PeekHead();
    }

    private bool WavesFinished()
    {
        if (IWaves + 1 < m_wavesList.Count)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    IEnumerator NextWaveRoutine()
    {
        Debug.Log("---------- WAVE " + IWaves + " ----------");
        PlayerWavesInfo.Instance?.StartWaitTime(m_nextRoundWaitTime);

        yield return new WaitForSeconds(m_nextRoundWaitTime);

        Debug.Log("---------- START ----------");
        PlayerWavesInfo.Instance?.EndWaitTime();

        m_iWaves++;
        UpdateWavesInfo();
        SpawnEnemy();
    }

    private void UpdateWavesInfo()
    {
        m_totalEnemies = m_wavesList[IWaves].m_count;
        PlayerWavesInfo.Instance?.UpdateCrntEnemies(m_totalEnemies);
        PlayerWavesInfo.Instance?.UpdateCrntWave(IWaves);
    }

    IEnumerator SpawnEnemyRoutine(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Debug.Log("HEY " + m_wavesList[IWaves]);
        Debug.Log("HEY " + m_wavesList[IWaves].PeekHead());

        if (CanSpawnEnemy())
        {
            EnemyData newEnemy = m_wavesList[IWaves].Dequeue();

            if (!newEnemy)
            {
                Debug.LogError("Invalid Enemy");
                return;
            }

            if (!newEnemy.m_enemyPrefab)
            {
                Debug.LogError("Enemy " + newEnemy.m_name + " has a invalid enemy prefab ");
                return;
            }

            Char_Enemy enemy = newEnemy.m_enemyPrefab;

            Vector3 spawnPos = GameManager.Instance.GetRndSpawnPos();

            Instantiate(enemy, spawnPos, Quaternion.identity);

            Debug.Log("Enemy Spawned: " + newEnemy.m_name);
        }

        //Next enemy
        if (EnemiesWaveFinished()) { return; }

        float minT = m_wavesInfo[IWaves].m_minSpawnTime;
        float maxT = m_wavesInfo[IWaves].m_maxSpawnTime;

        float nEnemyT = Random.Range(minT, maxT);
        Debug.Log("                - Next enemy in: " + nEnemyT + "s");

        StartCoroutine(SpawnEnemyRoutine(nEnemyT));
    }

    public bool CanSpawnEnemy()
    {
        return m_crntEnemies < m_wavesInfo[IWaves].m_maxEnemies;
    }


    public void AddEnemy(Char_Enemy _enemy)
    {
        m_crntEnemiesList.Add(_enemy);
        m_crntEnemies++;
    }

    public void RemoveEnemy(Char_Enemy _enemy)
    {
        m_crntEnemiesList.Remove(_enemy);
        m_crntEnemies--;

        m_totalEnemies--;
        PlayerWavesInfo.Instance?.UpdateCrntEnemies(m_totalEnemies);

        if (m_crntEnemies == 0 && EnemiesWaveFinished())
        {
            if (!WavesFinished())
            {
                StartCoroutine(NextWaveRoutine());
            }
            else
            {
                GameManager.Instance?.GameOver(true);
            }
        }
    }
}
