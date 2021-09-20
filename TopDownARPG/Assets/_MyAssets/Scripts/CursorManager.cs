using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance = null;

    private Texture2D   m_crntCTexture            = null;

    [SerializeField]
    private Texture2D   m_cursorDefaultTexture    = null;
    [SerializeField]
    private Texture2D   m_cursorMoveTexture       = null;
    [SerializeField]
    private Texture2D   m_cursorItemTexture       = null;
    [SerializeField]
    private Texture2D   m_cursorAttackTexture     = null;



    private void Awake()
    {
        Instance = this;
        ChangeCursorTexture(E_CURSOR_MODE.DEFAULT);
    }

    public void ChangeCursorTexture(E_CURSOR_MODE _newMode)
    {
        switch (_newMode)
        {
            case E_CURSOR_MODE.DEFAULT:
                m_crntCTexture = m_cursorDefaultTexture;
                break;
            case E_CURSOR_MODE.MOVE:
                m_crntCTexture = m_cursorMoveTexture;
                break;
            case E_CURSOR_MODE.GET_ITEM:
                m_crntCTexture = m_cursorItemTexture;
                break;
            case E_CURSOR_MODE.ATTACK:
                m_crntCTexture = m_cursorAttackTexture;
                break;
        }

        Cursor.SetCursor(m_crntCTexture, Vector2.zero, CursorMode.Auto);
    }
}
