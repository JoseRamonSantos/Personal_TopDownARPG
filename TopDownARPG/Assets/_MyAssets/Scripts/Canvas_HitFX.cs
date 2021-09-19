using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class Canvas_HitFX : MonoBehaviour
{
    private Animation m_cmpAnimation = null;
    

    private void Awake()
    {
        m_cmpAnimation = GetComponent<Animation>();
    }

    private void Start()
    {
        Char_Player.Instance.OnReceiveHit += OnReceiveHit;
    }

    private void OnReceiveHit()
    {
        m_cmpAnimation.Play();
    }
}
