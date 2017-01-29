using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Tools;

public abstract class ScrollListWidget<T> : UIWidget, IBeginDragHandler, IEndDragHandler where T : ScrollListElementWidget {

    public float ScrollRectDamping = 15;

    protected HangarPanel _Panel;
    protected ScrollRect _ScrollRect;
    protected List<T> _Previews;

    private bool _ControlledByDrag;

    protected virtual void Start() {
        _ScrollRect = this.GetComponentInChildren<ScrollRect>();
        _Panel = this.GetComponent<HangarPanel>();
        Refresh();
    }

    void Update() {
        if (_ControlledByDrag) {
            UpdateSelection();
        }
        else {
            UpdateScrollRect();
        }
    }

    void UpdateScrollRect() {
        var widget = _Previews.FirstOrDefault(_ => _.gameObject == EventSystem.current.currentSelectedGameObject);
        var scroll = _ScrollRect.viewport;
        var target = _ScrollRect.verticalNormalizedPosition;

        if (widget != null) {
            var widgetCorners = new Vector3[4];
            var scrollCorners = new Vector3[4];
            widget.RectTransform.GetWorldCorners(widgetCorners);
            scroll.GetWorldCorners(scrollCorners);
            var widgetMax = widget.RectTransform.TransformPoint(0,widget.RectTransform.rect.yMax,0).y;
            var scrollMax = scrollCorners.Max(_ => _.y);
            var widgetMin = widgetCorners.Min(_ => _.y);
            var scrollMin = scrollCorners.Min(_ => _.y);
            var dMax = widgetMax - scrollMax;
            var dMin = widgetMin - scrollMin;
            if (dMax > 0) {
                target += (_ScrollRect.content.InverseTransformVector(Vector3.up * dMax).y) / _ScrollRect.content.rect.height;
            }
            if (dMin < 0) {
                target += (_ScrollRect.content.InverseTransformVector(Vector3.up * dMin).y) / _ScrollRect.content.rect.height;
            }

            _ScrollRect.verticalNormalizedPosition = Mathf.Lerp(_ScrollRect.verticalNormalizedPosition, target, ScrollRectDamping * Time.unscaledDeltaTime);
        }
    }

    void UpdateSelection() {

    }

    public abstract void Refresh();

    protected void RegisterPreview(T preview) {
        _Previews.Add(preview);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        _ControlledByDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData) {
        this.WaitUntil(
            () => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0,
            () => _ControlledByDrag = false
            );
    }
}
