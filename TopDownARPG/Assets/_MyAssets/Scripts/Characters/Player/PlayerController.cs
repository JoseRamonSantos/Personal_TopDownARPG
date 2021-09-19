using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance = null;

    private Char_Player m_cmpPlayer = null;
    private NavMeshAgent m_cmpAgent = null;
    private CharMovement m_cmpMovement = null;
    private CharAttack m_cmpAttack = null;
    private CinemachineFreeLook m_cmpFreeLokCam = null;

    [SerializeField]
    private LayerMask m_mouseLM;

    [SerializeField]
    private E_CURSOR_MODE m_crntMode = E_CURSOR_MODE.DEFAULT;

    [SerializeField]
    private UnityEngine.GameObject m_lastHover = null;
    [SerializeField]
    private UnityEngine.GameObject m_crntHover = null;

    private void Awake()
    {
        Instance = this;

        m_cmpPlayer = GetComponent<Char_Player>();
        m_cmpAgent = GetComponent<NavMeshAgent>();
        m_cmpMovement = GetComponent<CharMovement>();
        m_cmpAttack = GetComponent<CharAttack>();
        m_cmpFreeLokCam = FindObjectOfType<CinemachineFreeLook>();
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        DisenableCameraMovement();
    }

    private void Update()
    {
        //CURSOR
        RaycastHit rHit = new RaycastHit();
        NavMeshHit nVHit = new NavMeshHit();
        IDisplayInfo dInfo = null;

        //PAUSE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (dInfo != null)
            {
                dInfo.EndHover();
            }
            CursorManager.Instance.ChangeCursorTexture(E_CURSOR_MODE.DEFAULT);
            GameManager.Instance.TogglePause();
        }

        if (GameManager.Instance.PauseActivated) { return; }

        Char_Enemy enemyTarget = null;
        Item_Loot lootItemTarget = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //CURSOR MODE (Lo del event system es para que no funcione cuando se tiene el cursor sobre un elemento de la UI)
        if (Physics.Raycast(ray.origin, ray.direction * 10, out rHit, float.MaxValue, m_mouseLM) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (NavMesh.SamplePosition(rHit.point, out nVHit, 0.5f, m_cmpAgent.areaMask)) { }

            if (rHit.transform.TryGetComponent(out enemyTarget))
            {
                m_crntMode = E_CURSOR_MODE.ATTACK;
            }
            else if (rHit.transform.TryGetComponent(out lootItemTarget))
            {
                m_crntMode = E_CURSOR_MODE.GET_ITEM;
            }
            else if (nVHit.hit)
            {
                m_crntMode = E_CURSOR_MODE.MOVE;
            }
            else
            {
                m_crntMode = E_CURSOR_MODE.DEFAULT;
            }

            m_crntHover = rHit.transform.gameObject;

            if (m_lastHover != m_crntHover)
            {
                if (m_lastHover)
                {
                    //End Hover
                    if (m_lastHover.TryGetComponent(out dInfo))
                    {
                        dInfo.EndHover();
                    }

                }

                //Start Hover
                if (m_crntHover.TryGetComponent(out dInfo))
                {
                    dInfo.StartHover();
                }

                m_lastHover = m_crntHover;
            }

        }
        else
        {
            m_crntHover = null;

            //End Hover
            if (m_lastHover)
            {
                if (m_lastHover.TryGetComponent(out dInfo))
                {
                    dInfo.EndHover();
                }

                m_lastHover = null;
            }

            nVHit.hit = false;
            m_crntMode = E_CURSOR_MODE.DEFAULT;
        }

        CursorManager.Instance.ChangeCursorTexture(m_crntMode);

        //CURSOR ACTIONS
        if (Input.GetMouseButtonDown(0))
        {
            switch (m_crntMode)
            {
                case E_CURSOR_MODE.DEFAULT:
                    break;
                case E_CURSOR_MODE.MOVE:
                    m_cmpAttack.EndAttack();
                    m_cmpMovement.SetNewDestination(nVHit.position);
                    break;
                case E_CURSOR_MODE.GET_ITEM:
                    //m_cmpMovement.SetNewDestination(rHit.transform.position);
                    ItemData itemData = lootItemTarget.LootData;

                    Inventory.Instance.AddItem(lootItemTarget.LootData);

                    Debug.Log("Player has taken " + itemData.m_itmName + " item");
                    ConsoleManager.Instance.AddLootItem(itemData);

                    Destroy(lootItemTarget.gameObject);
                    break;

                case E_CURSOR_MODE.ATTACK:
                    //m_cmpMovement.SetNewDestination(rHit.transform);
                    m_cmpAttack.NewAttackTarget(enemyTarget, m_cmpPlayer.AttackDistance);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Inventory.Instance.QuickUseHpPotion();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (PlayerEquipmentDisplay.Instance)
            {
                PlayerEquipmentDisplay.Instance.ToggleInventoryVisibility();
            }
        }


        if (Input.GetMouseButtonDown(2))
        {
            EnableCameraMovement();
        }
        else if (Input.GetMouseButtonUp(2))
        {
            DisenableCameraMovement();
        }

        //DEBUG
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Time.timeScale = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Time.timeScale = 0.25f;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Time.timeScale = 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            m_cmpPlayer.DoDamage(m_cmpPlayer);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            m_cmpPlayer.Heal(10);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            m_cmpPlayer.Die();
        }
    }

    private void EnableCameraMovement()
    {
        m_cmpFreeLokCam.m_XAxis.m_InputAxisName = "Mouse X";
    }

    private void DisenableCameraMovement()
    {
        m_cmpFreeLokCam.m_XAxis.m_InputAxisName = "";
        m_cmpFreeLokCam.m_XAxis.m_InputAxisValue = 0;
    }
}
