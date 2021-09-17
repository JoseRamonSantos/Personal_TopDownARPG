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
    private GameObject m_pausePanel = null;
    private bool m_pauseActivated = false;
    private float m_pauseTS = 1;
    [SerializeField]
    private GameObject m_winPanelCanvas = null;
    [SerializeField]
    private GameObject m_losePanelCanvas = null;
    [SerializeField]
    private TextMeshProUGUI m_loseWavesText = null;

    [Space]

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



    public Canvas Canvas
    {
        get
        {
            if (m_canvas == null)
            {
                m_canvas = FindObjectOfType<Canvas>();
            }

            return m_canvas;
        }
    }
    public List<SpawnPoint> SpawnPointsList { get => m_spawnPointsList; }
    public bool PauseActivated { get => m_pauseActivated; }



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

        EndPause();
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
    /*public void SpawnHitInfo(Vector3 _pos, int _damage, E_HP_INFO_TYPE _type)
    {
        GameObject hitInfo = null;

        switch (_type)
        {
            case E_HP_INFO_TYPE.BASIC_HIT:
                hitInfo = m_basicHitInfo;
                break;
            case E_HP_INFO_TYPE.CRITICAL_HIT:
                hitInfo = m_criticalHitInfo;
                break;
            case E_HP_INFO_TYPE.MISS_HIT:
                hitInfo = m_missHitInfo;
                break;
        }


        Vector3 infoPos = Camera.main.WorldToScreenPoint(_pos);

        float rndDelta = Random.Range(-10.0f, 10.0f);

        infoPos.x += rndDelta;
        infoPos.z = 0;

        GameObject hitinfo = Instantiate(hitInfo, infoPos, Quaternion.identity, Canvas.transform);

        if (_type != E_HP_INFO_TYPE.MISS_HIT)
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

        Vector3 infoPos = Camera.main.WorldToScreenPoint(_pos);

        float rndDelta = Random.Range(-10.0f, 10.0f);

        infoPos.x += rndDelta;
        infoPos.z = 0;

        GameObject hitInfo = Instantiate(m_healInfo, infoPos, Quaternion.identity, Canvas.transform);

        hitInfo.GetComponent<TextMeshProUGUI>().text = _amount.ToString();
    }*/

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

    //Pause 
    public void TogglePause()
    {
        if (m_pauseActivated)
        {
            EndPause();
        }
        else
        {
            StartPause();
        }
    }

    public void StartPause()
    {
        m_pauseActivated = true;

        m_pauseTS = Time.timeScale;

        Time.timeScale = 0;

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(true);
        }
    }

    public void EndPause()
    {
        m_pauseActivated = false;

        Time.timeScale = m_pauseTS;

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(false);
        }
    }

    //Scene managment
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        int lIndex = SceneManager.GetActiveScene().buildIndex;

        if (lIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(lIndex + 1);
        }
        else
        {
            Debug.LogError(transform.name + " there is NO NEXT LEVEL");
        }
    }

    public void PreviousLevel()
    {
        int lIndex = SceneManager.GetActiveScene().buildIndex;

        if (lIndex > 0)
        {
            SceneManager.LoadScene(lIndex - 1);
        }
        else
        {
            Debug.LogError(transform.name + " there is NO PREVIOUS LEVEL");
        }
    }

    public void GoToLevel(string _lName)
    {
        if (SceneManager.GetSceneByName(_lName).IsValid())
        {
            SceneManager.LoadScene(_lName);
        }
        else
        {
            Debug.LogError(transform.name + " INVALID LEVEL NAME");
        }
    }

    public void GoToLevel(int _lIndex)
    {
        if (_lIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(_lIndex);
        }
        else
        {
            Debug.LogError(transform.name + " INVALID LEVEL INDEX");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
