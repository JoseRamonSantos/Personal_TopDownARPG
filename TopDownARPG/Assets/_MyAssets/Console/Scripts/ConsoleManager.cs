using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleManager : MonoBehaviour
{
    public static ConsoleManager Instance = null;

    [SerializeField]
    private GameObject m_pfConsoleMessage = null;
    [SerializeField]
    private GameObject m_content = null;
    [SerializeField]
    private GameObject m_scroll = null;

    [Space]
    [SerializeField]
    private List<ConsoleMessage> m_cMessagesList = new List<ConsoleMessage>();

    [Space]

    [SerializeField]
    private float m_maxTimeToHide = 4;
    private float m_crntTimeToHide = 0;
    private bool m_consoleActivated = false;

    private void Awake()
    {
        Instance = this;

        HideConsole();
    }

    private void Update()
    {
        UpdateHideConsoleTimer();

        //DEBUG
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddTestMessage();
        }
    }

    public void AddTestMessage()
    {
        ConsoleMessage cMessage = NewMessage();

        cMessage.ResetText();
        cMessage.AddText("This is a");
        cMessage.AddText("TEST", "red", true);
        cMessage.AddText("message", "red", true);
        cMessage.AddText("(id." + (Random.value * 10000).ToString("F0") + ")", "yellow", false, true);
    }

    public void AddPlayerCauseDamage(Char_Enemy _target, int _dmgTaken, int _totalDmg, int _dmgAbsorved, E_HP_INFO_TYPE _hitInfo)
    {
        ConsoleMessage cMessage = NewMessage();

        cMessage.ResetText();
        cMessage.AddText("Damage");
        cMessage.AddText("given", "green", true);
        cMessage.AddText("to");
        if (_target)
        {
            cMessage.AddText(_target.name.ToString(), "red", false, true);
        }
        cMessage.AddText(":");
        cMessage.AddText(_dmgTaken.ToString(), "red", true);
        cMessage.AddText("(" + _totalDmg.ToString() + "-" + _dmgAbsorved.ToString() + ")");
        if (_hitInfo == E_HP_INFO_TYPE.CRITICAL_HIT)
        {
            cMessage.AddText("CRITICAL HIT", "orange");
        }
    }

    public void AddPlayerReceiveDamage(Char_Enemy _owner, int _dmgTaken, int _totalDmg, int _dmgAbsorved, E_HP_INFO_TYPE _hitInfo)
    {
        ConsoleMessage cMessage = NewMessage();

        cMessage.ResetText();
        cMessage.AddText("Damage");
        cMessage.AddText("taken", "red", true);
        cMessage.AddText("from");
        if (_owner)
        {
            cMessage.AddText(_owner.name.ToString(), "red", false, true);
        }
        cMessage.AddText(":");
        cMessage.AddText(_dmgTaken.ToString(), "red", true);
        cMessage.AddText("(" + _totalDmg.ToString() + "-" + _dmgAbsorved.ToString() + ")");
        if (_hitInfo == E_HP_INFO_TYPE.CRITICAL_HIT)
        {
            cMessage.AddText("CRITICAL HIT", "orange");
        }
    }

    public void AddPlayerHeal(int _amount)
    {
        ConsoleMessage cMessage = NewMessage();

        cMessage.ResetText();
        cMessage.AddText("Heal:");
        cMessage.AddText(_amount.ToString(), "green", true);
        cMessage.AddText("HP");
    }

    public void AddLootItem(ItemData _itm)
    {
        ConsoleMessage cMessage = NewMessage();

        cMessage.ResetText();
        cMessage.AddText("+", "purple", true);
        cMessage.AddText(_itm.m_itmName.ToString(), "purple", true, true);
    }

    private ConsoleMessage NewMessage()
    {
        ShowConsole();

        ConsoleMessage cMessage;
        cMessage = Instantiate(m_pfConsoleMessage, m_content.transform).GetComponentInChildren<ConsoleMessage>();

        if (m_cMessagesList.Count > 0)
        {
            m_cMessagesList[m_cMessagesList.Count - 1].Predeactivate();
        }

        m_cMessagesList.Add(cMessage);

        return cMessage;
    }

    private void UpdateHideConsoleTimer()
    {
        if (m_consoleActivated)
        {
            m_crntTimeToHide -= Time.deltaTime;

            if (m_crntTimeToHide <= 0)
            {
                HideConsole();
            }
        }
    }

    private void ShowConsole()
    {
        m_scroll.gameObject.SetActive(true);
        m_crntTimeToHide = m_maxTimeToHide;
        m_consoleActivated = true;
    }

    private void HideConsole()
    {
        m_scroll.gameObject.SetActive(false);
        m_consoleActivated = false;

        for (int i = 0; i < m_cMessagesList.Count; i++)
        {
            m_cMessagesList[i].Deactivate();
        }
    }

}
