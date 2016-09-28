using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Tools;

public class InventoryWidget : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

    public bool UseSlotFilter;
    [ReadOnly("UseSlotFilter")]
    public SlotType SlotFilter;
    [ReadOnly("UseSlotFilter", false)]
    public bool CargoOnly;

    public float ScrollRectDamping = 5;

    private ScrollRect _ScrollRect;
    private List<ItemPreviewWidget> _Previews;

    private bool _ControlledByDrag;

    void Awake() {
        _ScrollRect = this.GetComponent<ScrollRect>();
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
        var source = _ScrollRect.verticalNormalizedPosition * _ScrollRect.
        var target = _ScrollRect.verticalNormalizedPosition 
        if (widget != null) {
            var dTop = widget.RectTransform.rect.yMax - _ScrollRect.viewport.rect.yMax;
            if (dTop > 0)
        }
    }

    void UpdateSelection() {

    }

    public void Refresh() {
        if(_Previews != null) {
            _Previews.ForEach(_ => Destroy(_.gameObject));
        }
        _Previews = new List<ItemPreviewWidget>();
        if (UseSlotFilter) {
            var items = PlayerController.LocalPlayer.Inventory.Items.OfType<SlotItem>().Where(_ => _.CheckCompatability(SlotFilter)).ToList();
            items.ForEach(_ => RegisterPreview(ItemPreviewWidget.Instantiate(_, _ScrollRect.content)));
        }
        else {
            if (CargoOnly) {
                var items = PlayerController.LocalPlayer.Inventory.Items.OfType<GenericItem>().ToList();
                items.ForEach(_ => RegisterPreview(ItemPreviewWidget.Instantiate(_, _ScrollRect.content)));
            }
            else {
                var items = PlayerController.LocalPlayer.Inventory.Items;
                items.ForEach(_ => RegisterPreview(ItemPreviewWidget.Instantiate(_, _ScrollRect.content)));
            }
        }
    }

    private void RegisterPreview(ItemPreviewWidget preview) {
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
