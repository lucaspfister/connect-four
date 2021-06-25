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

public enum Slot
{
    Empty,
    Player1,
    Player2
}

public class Board : MonoBehaviour
{
    [SerializeField] private RectTransform m_Checker1Prefab;
    [SerializeField] private RectTransform m_Checker2Prefab;
    [SerializeField] private Transform m_CheckerParent;
    [SerializeField] private float m_CheckerAnimDuration = 1f;

    private Slot[,] m_Slots;
    private int[] m_ColumnCounter;
    private float m_SpaceBetweenCheckers;
    private CheckerInfo m_LastChecker;
    private int m_CheckersCount;

    private readonly Vector2Int BOARD_SIZE = new Vector2Int(7, 6);

    public Slot[,] Slots => m_Slots;
    public int[] ColumnCounter => m_ColumnCounter;

    private void Start()
    {
        m_SpaceBetweenCheckers = (m_CheckerParent.GetComponent<RectTransform>().rect.width - (m_Checker1Prefab.rect.width * BOARD_SIZE.x)) / (BOARD_SIZE.x - 1);
    }

    public void Init()
    {
        m_Slots = new Slot[BOARD_SIZE.x, BOARD_SIZE.y];
        m_ColumnCounter = new int[BOARD_SIZE.x];
        m_LastChecker = new CheckerInfo();
        m_CheckersCount = 0;

        foreach (Transform item in m_CheckerParent)
        {
            Destroy(item.gameObject);
        }
    }

    public bool AddChecker(int columnIndex, bool isPlayer1)
    {
        if (m_ColumnCounter[columnIndex] >= BOARD_SIZE.y) return false;

        Slot slot = isPlayer1 ? Slot.Player1 : Slot.Player2;

        RectTransform prefab = isPlayer1 ? m_Checker1Prefab : m_Checker2Prefab;
        RectTransform checker = Instantiate(prefab, m_CheckerParent);
        checker.anchoredPosition = new Vector2((checker.rect.width + m_SpaceBetweenCheckers) * columnIndex, 0);
        float y = -checker.rect.height * (BOARD_SIZE.y - m_ColumnCounter[columnIndex]);

        m_LastChecker.Set(isPlayer1, columnIndex, m_ColumnCounter[columnIndex]);
        m_Slots[columnIndex, m_ColumnCounter[columnIndex]] = slot;
        m_ColumnCounter[columnIndex]++;
        m_CheckersCount++;

        checker.DOAnchorPosY(y, m_CheckerAnimDuration).SetEase(Ease.InExpo).OnComplete(() =>
        {
            GameManager.Instance.CheckerAdded();
        });

        return true;
    }

    public List<int> GetAvailableColumns()
    {
        List<int> lstAvailableColumns = new List<int>();

        for (int i = 0; i < m_ColumnCounter.Length; i++)
        {
            if (m_ColumnCounter[i] >= BOARD_SIZE.y) continue;

            lstAvailableColumns.Add(i);
        }

        return lstAvailableColumns;
    }

    public Result CheckResult()
    {
        if (m_CheckersCount < 7) return Result.None;

        Slot lastSlot = m_Slots[m_LastChecker.columnIndex, m_LastChecker.rowIndex];

        if (CheckVertical(lastSlot) || CheckHorizontal(lastSlot) || CheckLeftDiagonal(lastSlot) || CheckRightDiagonal(lastSlot))
        {
            return lastSlot == Slot.Player1 ? Result.Victory : Result.Defeat;
        }

        if (m_CheckersCount >= BOARD_SIZE.x * BOARD_SIZE.y) return Result.Draw;

        return Result.None;
    }

    #region Private Methods

    private bool CheckVertical(Slot slot)
    {
        int count = 0;

        //Down
        for (int y = m_LastChecker.rowIndex - 1; y >= 0; y--)
        {
            if (m_Slots[m_LastChecker.columnIndex, y] != slot) break;

            count++;
        }

        return count >= 3;
    }

    private bool CheckHorizontal(Slot slot)
    {
        int count = 0;

        //Left
        for (int x = m_LastChecker.columnIndex - 1; x >= 0; x--)
        {
            if (m_Slots[x, m_LastChecker.rowIndex] != slot) break;

            count++;
        }

        //Right
        for (int x = m_LastChecker.columnIndex + 1; x < BOARD_SIZE.x; x++)
        {
            if (m_Slots[x, m_LastChecker.rowIndex] != slot) break;

            count++;
        }

        return count >= 3;
    }

    private bool CheckLeftDiagonal(Slot slot)
    {
        int count = 0;
        int col = m_LastChecker.columnIndex - 1;
        int row = m_LastChecker.rowIndex + 1;

        //Up Left
        while (col >= 0 && row < BOARD_SIZE.y)
        {
            if (m_Slots[col, row] != slot) break;

            count++;
            col--;
            row++;
        }

        col = m_LastChecker.columnIndex + 1;
        row = m_LastChecker.rowIndex - 1;

        //Down Right
        while (col < BOARD_SIZE.x && row >= 0)
        {
            if (m_Slots[col, row] != slot) break;

            count++;
            col++;
            row--;
        }

        return count >= 3;
    }

    private bool CheckRightDiagonal(Slot slot)
    {
        int count = 0;
        int col = m_LastChecker.columnIndex + 1;
        int row = m_LastChecker.rowIndex + 1;

        //Up Right
        while (col < BOARD_SIZE.x && row < BOARD_SIZE.y)
        {
            if (m_Slots[col, row] != slot) break;

            count++;
            col++;
            row++;
        }

        col = m_LastChecker.columnIndex - 1;
        row = m_LastChecker.rowIndex - 1;

        //Down Left
        while (col >= 0 && row >= 0)
        {
            if (m_Slots[col, row] != slot) break;

            count++;
            col--;
            row--;
        }

        return count >= 3;
    }

    #endregion
}

public struct CheckerInfo
{
    public bool isPlayer1;
    public int columnIndex;
    public int rowIndex;

    public void Set(bool isPlayer1, int columnIndex, int rowIndex)
    {
        this.isPlayer1 = isPlayer1;
        this.columnIndex = columnIndex;
        this.rowIndex = rowIndex;
    }
}
