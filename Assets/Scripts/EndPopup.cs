using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EndPopup : MonoBehaviour
{
    [SerializeField] private Button m_ButtonClose;
    [SerializeField] private TextMeshProUGUI m_Text;

    private Canvas m_Canvas;
    private Animator m_Animator;

    private const string VICTORY = "YOU WIN!";
    private const string DEFEAT = "YOU LOSE";
    private const string DRAW = "DRAW";
    
    public Action OnClosePopup;

    private void Start()
    {
        m_Canvas = GetComponent<Canvas>();
        m_Animator = GetComponent<Animator>();
        m_Canvas.enabled = false;

        m_ButtonClose.onClick.AddListener(() => 
        {
            m_Animator.SetTrigger("reset");
            m_Canvas.enabled = false;
            OnClosePopup?.Invoke();
        });
    }

    public void ShowPopup(Result result)
    {
        switch (result)
        {
            case Result.Victory:
                m_Text.text = VICTORY;
                m_Animator.SetTrigger("victory");
                break;
            case Result.Defeat:
                m_Text.text = DEFEAT;
                m_Animator.SetTrigger("defeat");
                break;
            case Result.Draw:
                m_Text.text = DRAW;
                m_Animator.SetTrigger("defeat");
                break;
        }

        m_Canvas.enabled = true;
    }
}
