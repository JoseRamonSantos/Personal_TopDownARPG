using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerWavesInfo : MonoBehaviour
{
    public static PlayerWavesInfo Instance = null;

    [SerializeField]
    private TextMeshProUGUI m_crntWave = null;
    [SerializeField]
    private TextMeshProUGUI m_crntEnemies = null;
    [SerializeField]
    private GameObject m_waitTimePanel = null;
    [SerializeField]
    private TextMeshProUGUI m_crntWaitTime = null;
    private float m_crntTime;


    public float CrntTime
    {
        get
        {
            return m_crntTime;
        }
        set
        {
            m_crntTime = Mathf.Clamp(value, 0, float.MaxValue);
        }
    }


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (m_waitTimePanel.activeSelf)
        {
            CrntTime -= Time.deltaTime;
            m_crntWaitTime.text = CrntTime.ToString("F1");
        }
    }

    public void UpdateCrntWave(int _crntWave)
    {
        m_crntWave.text = _crntWave.ToString();
    }

    public void UpdateCrntEnemies(int _crntEnemies)
    {
        m_crntEnemies.text = _crntEnemies.ToString();
    }

    public void StartWaitTime(float _initTime)
    {
        CrntTime = _initTime;

        m_waitTimePanel.SetActive(true);
    }

    public void EndWaitTime()
    {
        m_waitTimePanel.SetActive(false);
    }

}
