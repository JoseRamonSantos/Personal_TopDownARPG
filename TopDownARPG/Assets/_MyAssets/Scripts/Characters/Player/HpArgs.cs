using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpArgs : EventArgs
{
    public int m_crntHP;
    public int m_maxHP;

    public HpArgs(int _crnt, int _max)
    {
        m_crntHP = _crnt;
        m_maxHP = _max;
    }
}
