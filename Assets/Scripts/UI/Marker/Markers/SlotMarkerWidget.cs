using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Tools.UI.Markers;

namespace UI.Markers {


    public class SlotMarkerWidget : MarkerWidget<SlotMarkerData> {

        public RawImage Preview;
        [Header("Selection")]
        public float SelectionDamping = 0.75f;
        public CanvasGroup SelectedContent;
        public RectTransform ResizedRect;
        [Header("Anchors")]
        [Header("Deselected")]
        public Vector2 DeselectedMin;
        public Vector2 DeselectedMax;
        [Header("Selected")]
        public Vector2 SelectedMin;
        public Vector2 SelectedMax;

        public override void UpdateMarker(SlotMarkerData data) {
            Preview.texture = data.Preview;
            if (data.Focused) {
                ResizedRect.anchorMin = Vector2.Lerp(ResizedRect.anchorMin, SelectedMin, SelectionDamping * Time.deltaTime);
                ResizedRect.anchorMax = Vector2.Lerp(ResizedRect.anchorMax, SelectedMax, SelectionDamping * Time.deltaTime);
                SelectedContent.alpha = Mathf.Lerp(SelectedContent.alpha, 1, SelectionDamping * Time.deltaTime);
                this.RectTransform.SetAsLastSibling();
            }
            else {
                ResizedRect.anchorMin = Vector2.Lerp(ResizedRect.anchorMin, DeselectedMin, SelectionDamping * Time.deltaTime);
                ResizedRect.anchorMax = Vector2.Lerp(ResizedRect.anchorMax, DeselectedMax, SelectionDamping * Time.deltaTime);
                SelectedContent.alpha = Mathf.Lerp(SelectedContent.alpha, 0, SelectionDamping * Time.deltaTime);
            }
        }
    }
}