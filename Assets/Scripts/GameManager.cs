using System.Collections;
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
    }

    [SerializeField] private SelectionManager m_SelectionManager;
    [SerializeField] private Board m_Board;
    [SerializeField] private EndPopup m_EndPopup;
    [SerializeField] private float m_AIDelay = 1f;
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
        m_EndPopup.OnClosePopup += ResetGame;
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
                StartCoroutine(AITurn());
                break;
            case GameState.AITurn:
                m_CurrentState = GameState.PlayerTurn;
                m_SelectionManager.Unlock();
                m_TurnText.text = PLAYER_TURN;
                break;
        }
    }

    public void CheckerAdded()
    {
        Result result = m_Board.CheckResult();

        switch (result)
        {
            case Result.None:
                NextTurn();
                break;
            case Result.Victory:
            case Result.Defeat:
            case Result.Draw:
                m_TurnText.text = string.Empty;
                m_EndPopup.ShowPopup(result);
                break;
        }
    }

    private IEnumerator AITurn()
    {
        yield return new WaitForSeconds(m_AIDelay);
        int index = AI.GetRandomValue(m_Board.GetAvailableColumns());
        m_Board.AddChecker(index, false);
    }
}
