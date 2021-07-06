using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera m_mainCam;

    public Camera MainCam
    {
        get
        {
            if (!m_mainCam)
            {
                m_mainCam = Camera.main;
            }

            return m_mainCam;
        }
    }


    void Update()
    {
        transform.forward = -MainCam.transform.forward;
    }
}
