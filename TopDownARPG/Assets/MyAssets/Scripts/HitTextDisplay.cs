using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitTextDisplay : MonoBehaviour
{
    [SerializeField]
    private float m_initSpeed = 0.05f;
    [SerializeField]
    private float m_timeToFade = 0.5f;
    private float m_crntTime = 0;

    [SerializeField]
    private float m_fadeVelocity = 2;


    private TextMeshProUGUI m_text = null;

    private void Awake()
    {
        m_text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        //MOVE
        transform.Translate(Vector3.up * m_initSpeed);

        m_crntTime += Time.deltaTime;

        if(m_crntTime >= m_timeToFade)
        {
            float crntAlfa = m_text.color.a;
            float dAlfa = 0;

            if(crntAlfa == 0)
            {
                Destroy(this.gameObject);
            }

            crntAlfa = Mathf.Lerp(crntAlfa, dAlfa, Time.deltaTime * m_fadeVelocity);


            Color newColor = m_text.color;
            newColor.a = crntAlfa;

            m_text.color = newColor;
        }
    }
}
