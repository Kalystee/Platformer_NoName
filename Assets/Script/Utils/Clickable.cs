using System;
using System.Collections.Generic;
using UIEventDelegate;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Clickable : Selectable, IPointerClickHandler
{ 
    [SerializeField]
    ReorderableEventList leftClick;
    [SerializeField]
    ReorderableEventList middleClick;
    [SerializeField]
    ReorderableEventList rightClick;

    public void AddLeftClick(Action callback)
    {
        if(leftClick.List == null)
        {
            leftClick.List = new List<EventDelegate>();
        }
        EventDelegate.Add(leftClick.List, callback);
    }

    public void ResetLeftClick()
    {
        leftClick = new ReorderableEventList();
        if (leftClick.List == null)
        {
            leftClick.List = new List<EventDelegate>();
        }
    }

    public void AddRightClick(Action callback)
    {
        if (rightClick.List == null)
        {
            rightClick.List = new List<EventDelegate>();
        }
        EventDelegate.Add(rightClick.List, callback);
    }

    public void ResetRightClick()
    {
        rightClick = new ReorderableEventList();
        if (rightClick.List == null)
        {
            rightClick.List = new List<EventDelegate>();
        }
    }

    public void AddMiddleClick(Action callback)
    {
        if (middleClick.List == null)
        {
            middleClick.List = new List<EventDelegate>();
        }
        EventDelegate.Add(middleClick.List, callback);
    }

    public void ResetMiddleClick()
    {
        middleClick = new ReorderableEventList();
        if (middleClick.List == null)
        {
            middleClick.List = new List<EventDelegate>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                EventDelegate.Execute(leftClick.List);
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                EventDelegate.Execute(middleClick.List);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                EventDelegate.Execute(rightClick.List);
            }
        }
    }
}
