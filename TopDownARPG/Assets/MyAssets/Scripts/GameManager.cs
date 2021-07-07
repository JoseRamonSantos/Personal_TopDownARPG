using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [Space]
    [SerializeField]
    private GameObject m_winPanelCanvas = null;
    [SerializeField]
    private GameObject m_losePanelCanvas = null;
    [SerializeField]
    private TextMeshProUGUI m_loseWavesText = null;

    [SerializeField]
    private Canvas m_canvas = null;
    [SerializeField]
    private GameObject m_basicHitInfo = null;
    [SerializeField]
    private GameObject m_criticalHitInfo = null;
    [SerializeField]
    private GameObject m_missHitInfo = null;
    [SerializeField]
    private GameObject m_healInfo = null;

    [Space]

    [SerializeField]
    private GameObject m_lootPrefab = null;

    [Space]

    [SerializeField]
    private List<SpawnPoint> m_spawnPointsList = new List<SpawnPoint>();


    public List<SpawnPoint> SpawnPointsList { get => m_spawnPointsList; }



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (m_winPanelCanvas)
        {
            m_winPanelCanvas.SetActive(false);
        }

        if (m_losePanelCanvas)
        {
            m_losePanelCanvas.SetActive(false);
        }
    }

    public void DropItem(ItemData _itmData, Vector3 _pos, string _name = "", float _radius = 2)
    {
        Vector3 rndPos = Random.insideUnitSphere * _radius;

        Vector3 itemPos = _pos + rndPos;

        NavMeshHit nVHit;

        if (NavMesh.SamplePosition(itemPos, out nVHit, 100, Char_Player.Instance.CmpNVAgent.areaMask))
        {
            itemPos = nVHit.position;
        }

        Item_Loot itemLoot = Instantiate(m_lootPrefab, itemPos, Quaternion.identity).GetComponent<Item_Loot>();

        itemLoot.SetLootData(_itmData);

        Debug.Log(_name + " has dropped " + _itmData);
    }

    //Hit & Heal Info
    public void SpawnHitInfo(Vector3 _pos, int _damage, E_HIT_TYPE _type)
    {
        GameObject hitInfo = null;

        switch (_type)
        {
            case E_HIT_TYPE.BASIC:
                hitInfo = m_basicHitInfo;
                break;
            case E_HIT_TYPE.CRITICAL:
                hitInfo = m_criticalHitInfo;
                break;
            case E_HIT_TYPE.MISS:
                hitInfo = m_missHitInfo;
                break;
        }

        Vector3 hitPos = Camera.main.WorldToScreenPoint(_pos);


        GameObject hitinfo = Instantiate(hitInfo, hitPos, Quaternion.identity, m_canvas.transform);

        if (_type != E_HIT_TYPE.MISS)
        {
            hitinfo.GetComponent<TextMeshProUGUI>().text = _damage.ToString();
        }
        else
        {
            hitinfo.GetComponent<TextMeshProUGUI>().text = "miss";
        }
    }

    public void SpawnHealInfo(Vector3 _pos, int _amount)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(_pos);

        GameObject hitInfo = Instantiate(m_healInfo, pos, Quaternion.identity, m_canvas.transform);
        
        hitInfo.GetComponent<TextMeshProUGUI>().text = _amount.ToString();
    }

    //SPAWN POINTS
    public void AddSpawnPoint(SpawnPoint _sp)
    {
        m_spawnPointsList.Add(_sp);
    }

    public void RemoveSpawnPoint(SpawnPoint _sp)
    {
        m_spawnPointsList.Remove(_sp);
    }

    public Vector3 GetRndSpawnPos()
    {
        int rndI = Random.Range(0, m_spawnPointsList.Count);

        return m_spawnPointsList[rndI].transform.position;
    }


    //Win-Lose
    public void GameOver(bool _win)
    {
        if (_win)
        {
            Debug.Log("--------------------------------------------------");
            Debug.Log("--------------------GAME OVER!--------------------");
            Debug.Log("---------------------YOU WIN!---------------------");
            Debug.Log("--------------------------------------------------");
            Debug.Log("-------------------------------------------------- " + m_winPanelCanvas);

            if (!m_winPanelCanvas) { return; }

            m_winPanelCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("--------------------------------------------------");
            Debug.Log("--------------------GAME OVER!--------------------");
            Debug.Log("---------------------YOU LOSE---------------------");
            Debug.Log("--------------------------------------------------");

            if (m_loseWavesText)
            {
                m_loseWavesText.text = WavesManager.Instance?.IWaves.ToString();
            }

            if (m_losePanelCanvas)
            {
                m_losePanelCanvas.SetActive(true);
            }
        }
    }


    //Scene managment
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
