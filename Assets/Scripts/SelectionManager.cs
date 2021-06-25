using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Image m_Arrow;
    [SerializeField] private CanvasGroup m_ColumnGroup;

    private Transform m_ArrowParent;
    private SelectableColumn[] m_SelectableColumns;
    private int m_SelectedIndex;

    void Start()
    {
        m_ArrowParent = m_Arrow.transform.parent;
        int columns = m_ColumnGroup.transform.childCount;
        m_SelectableColumns = new SelectableColumn[columns];

        for (int i = 0; i < m_SelectableColumns.Length; i++)
        {
            int index = i;
            m_SelectableColumns[i] = m_ColumnGroup.transform.GetChild(i).GetComponent<SelectableColumn>();
            m_SelectableColumns[i].OnPointerEnterAction += () => EnableArrow(m_SelectableColumns[index].transform, index);
            m_SelectableColumns[i].OnPointerExitAction += DisableArrow;
            m_SelectableColumns[i].OnPointerUpAction += ColumnSelected;
        }
    }

    public void Lock()
    {
        m_ColumnGroup.blocksRaycasts = false;
    }

    public void Unlock()
    {
        m_ColumnGroup.blocksRaycasts = true;
    }

    private void EnableArrow( Transform selectedTransform, int columnIndex )
    {
        m_SelectedIndex = columnIndex;
        m_ArrowParent.position = new Vector3(selectedTransform.position.x, m_ArrowParent.position.y, m_ArrowParent.position.z);
        m_Arrow.enabled = true;
    }

    private void DisableArrow()
    {
        m_Arrow.enabled = false;
    }

    private void ColumnSelected()
    {
        if (GameManager.Instance.Board.AddChecker(m_SelectedIndex, true))
        {
            Lock();
        }
    }
}
