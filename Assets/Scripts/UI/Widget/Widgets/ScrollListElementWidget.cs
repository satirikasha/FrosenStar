using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class ScrollListElementWidget : UIWidget, ISelectHandler {

    public abstract void Refresh();

    public virtual void OnSelect(BaseEventData eventData) { }
}