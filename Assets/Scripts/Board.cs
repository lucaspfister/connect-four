using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Result
{
    None,
    Victory,
    Defeat,
    Draw
}

public class Board : MonoBehaviour
{
    public enum Slot
    {
        Empty,
        Player1,
        Player2
    }

    [SerializeField] private RectTransform m_Checker1Prefab;
    [SerializeField] private RectTransform m_Checker2Prefab;
    [SerializeField] private Transform m_CheckerParent;
    [SerializeField] private float m_CheckerAnimDuration = 1f;

    private Slot[,] m_Slots;
    private int[] m_ColumnCounter;
    private float m_SpaceBetweenCheckers;

    private readonly Vector2Int BOARD_SIZE = new Vector2Int(7, 6);

    private void Start()
    {
        m_SpaceBetweenCheckers = (m_CheckerParent.GetComponent<RectTransform>().rect.width - (m_Checker1Prefab.rect.width * BOARD_SIZE.x)) / (BOARD_SIZE.x - 1);
    }

    public void Init()
    {
        m_Slots = new Slot[BOARD_SIZE.x, BOARD_SIZE.y];
        m_ColumnCounter = new int[BOARD_SIZE.x];

        foreach (Transform item in m_CheckerParent)
        {
            Destroy(item.gameObject);
        }
    }

    public bool AddChecker(int columnIndex, bool isPlayer1)
    {
        if (columnIndex >= m_ColumnCounter.Length) return false;

        Slot slot = isPlayer1 ? Slot.Player1 : Slot.Player2;
        
        RectTransform prefab = isPlayer1 ? m_Checker1Prefab : m_Checker2Prefab;
        RectTransform checker = Instantiate(prefab, m_CheckerParent);
        checker.anchoredPosition = new Vector2((checker.rect.width + m_SpaceBetweenCheckers) * columnIndex, 0);
        float y = -checker.rect.height * (BOARD_SIZE.y - m_ColumnCounter[columnIndex]);
        
        m_Slots[columnIndex, m_ColumnCounter[columnIndex]] = slot;
        m_ColumnCounter[columnIndex]++;
        
        checker.DOAnchorPosY(y, m_CheckerAnimDuration).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            GameManager.Instance.CheckerAdded();
        });

        return true;
    }

    public Result CheckResult()
    {
        //TODO
        return Result.None;
    }
}
