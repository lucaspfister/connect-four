﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        PlayerTurn,
        AITurn,
        GameOver
    }

    [SerializeField] private Board m_Board;
    [SerializeField] private SelectionManager m_SelectionManager;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI m_TurnText;
    [SerializeField] private Button m_ResetButton;

    private GameState m_CurrentState;

    private const string PLAYER_TURN = "Your Turn";
    private const string AI_TURN = "AI's Turn";

    public Board Board => m_Board;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_ResetButton.onClick.AddListener(ResetGame);
        ResetGame();
    }

    private void ResetGame()
    {
        m_CurrentState = GameState.AITurn;
        m_Board.Init();
        NextTurn();
    }

    private void NextTurn()
    {
        switch (m_CurrentState)
        {
            case GameState.PlayerTurn:
                m_CurrentState = GameState.AITurn;
                m_SelectionManager.Lock();
                m_TurnText.text = AI_TURN;
                break;
            case GameState.AITurn:
                m_CurrentState = GameState.PlayerTurn;
                m_SelectionManager.Unlock();
                m_TurnText.text = PLAYER_TURN;
                break;
            case GameState.GameOver:
                //TODO
                break;
        }
    }

    public void CheckerAdded()
    {
        switch (m_Board.CheckResult())
        {
            case Result.None:
                NextTurn();
                break;
            case Result.Victory:
                break;
            case Result.Defeat:
                break;
            case Result.Draw:
                break;
        }
    }
}
