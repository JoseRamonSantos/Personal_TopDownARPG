using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AbilityBtn : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    private Ability_Base m_cmpAbility;
    [SerializeField]
    private string m_inputKey;

    [SerializeField]
    private Image m_imgAbilityIcon = null;
    [SerializeField]
    private Image m_imgCooldown = null;
    [SerializeField]
    private Image m_imgBorder = null;

    [SerializeField]
    private Button m_cmpAbilityBtn = null;
    #endregion


    #region PROPERTIES
    public Ability_Base CmpAbility { get => m_cmpAbility; set => m_cmpAbility = value; }
    public string InputKey { get => m_inputKey; set => m_inputKey = value; }
    public Image ImgAbilityIcon { get => m_imgAbilityIcon; set => m_imgAbilityIcon = value; }
    public Image ImgCooldown { get => m_imgCooldown; set => m_imgCooldown = value; }
    public Button CmpAbilityBtn { get => m_cmpAbilityBtn; set => m_cmpAbilityBtn = value; }
    #endregion


    #region METHODS
    void Start()
    {
        if (m_cmpAbility == null)
        {
            this.enabled = false;
            return;
        }

        m_cmpAbilityBtn = GetComponent<Button>();
        m_cmpAbilityBtn.onClick.AddListener(m_cmpAbility.ActivateInput);
        m_imgAbilityIcon = GetComponent<Image>();
        m_imgCooldown = transform.Find("Cooldown").GetComponent<Image>();
        m_imgAbilityIcon.sprite = m_cmpAbility.AbilityIcon;

        m_imgBorder = transform.Find("Border").GetComponent<Image>();

        CmpAbility.OnUpdateCooldown += UpdateCooldown;
        CmpAbility.OnDeactivateButton += DeactivateButton;
        CmpAbility.OnActivateButton += ActivateButton;

        ActivateButton();
        UpdateCooldown(0, 1);
    }

    void Update()
    {
        if (Input.GetKeyDown(m_inputKey))
        {
            m_cmpAbility.ActivateInput();
        }
    }

    private void ActivateButton()
    {
        m_cmpAbilityBtn.interactable = true;

        m_imgBorder.color = Color.grey;
    }

    private void DeactivateButton()
    {
        m_cmpAbilityBtn.interactable = false;
        m_imgBorder.color = Color.black;
    }

    private void UpdateCooldown(float _crntValue, float _maxValue)
    {
        m_imgCooldown.fillAmount = _crntValue / _maxValue;
    }
    #endregion
}
