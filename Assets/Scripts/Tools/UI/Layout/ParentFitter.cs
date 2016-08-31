using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Tools.UI.Layout {


    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class ParentFitter : UIBehaviour, ILayoutSelfController {

        private RectTransform rectTransform {
            get {
                if (rect == null)
                    rect = GetComponent<RectTransform>();
                return rect;
            }
        }
        private RectTransform rect;

        private DrivenRectTransformTracker m_Tracker;

        [Range(0, 1)]
        public float HeightRatio;
        [Range(0, 1)]
        public float WidthRatio;

        public void SetLayoutHorizontal() {
        }

        public void SetLayoutVertical() {
        }

        public void UpdateRect() {

            m_Tracker.Clear();

            if (!IsActive())
                return;

            m_Tracker.Add(this, rectTransform,
                                DrivenTransformProperties.Anchors |
                                DrivenTransformProperties.AnchoredPosition |
                                DrivenTransformProperties.SizeDeltaX |
                                DrivenTransformProperties.SizeDeltaY);


            var parentSize = GetParentSize();
            rectTransform.sizeDelta = new Vector2(parentSize.x * WidthRatio, parentSize.y * HeightRatio);
        }

        protected override void OnTransformParentChanged() {
            base.OnTransformParentChanged();
            SetDirty();
        }

        // Seems that parent rect transform is not initialized yet when OnEnable is called
        protected override void OnEnable() {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable() {
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        private Vector2 GetParentSize() {
            RectTransform parent = rectTransform.parent as RectTransform;
            if (!parent)
                return Vector2.zero;
            return parent.rect.size;
        }

        protected void SetDirty() {
            if (!IsActive())
                return;

            UpdateRect();
        }

#if UNITY_EDITOR
        protected override void OnValidate() {
            SetDirty();
        }
#endif
    }
}
