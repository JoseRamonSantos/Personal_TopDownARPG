using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AbilitiesBar : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    private UI_AbilityBtn m_pfAbilityBtn = null;

    private GameObject m_player = null;
    #endregion

    #region METHODS
    private void Start()
    {
        m_player = Char_Player.Instance.transform.gameObject;

        Ability_Base[] listaHabilidades = m_player.GetComponentsInChildren<Ability_Base>();

        if (listaHabilidades.Length > 0)
        {
            for (int i = 0; i < listaHabilidades.Length; i++)
            {
                UI_AbilityBtn newBtn = Instantiate(m_pfAbilityBtn, this.transform);
                newBtn.CmpAbility = listaHabilidades[i];
                newBtn.InputKey = (i + 1).ToString();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    #endregion
}
