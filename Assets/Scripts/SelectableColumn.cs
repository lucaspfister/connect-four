using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableColumn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action OnPointerEnterAction;
    public Action OnPointerExitAction;
    public Action OnPointerUpAction;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterAction?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitAction?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpAction?.Invoke();
    }
}
