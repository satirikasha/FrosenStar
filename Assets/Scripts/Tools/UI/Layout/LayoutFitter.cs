using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.Layout {


    [ExecuteInEditMode]
    [RequireComponent(typeof(LayoutElement))]
    public class LayoutFitter : UIBehaviour {

        public float Ratio;

        private bool _Initialized = false;
        private LayoutElement _LayoutElement;

        protected override void Awake() {
            RefreshLayout();            
        }

        protected override void OnRectTransformDimensionsChange() {
            RefreshLayout();
        }

        private void Init() {
            if (!_Initialized) {
                _LayoutElement = this.GetComponent<LayoutElement>();
                _Initialized = true;
            }
        }

        private void RefreshLayout() {
            Init();
            _LayoutElement.preferredHeight = ((RectTransform)this.transform).rect.width * Ratio;
        }




#if UNITY_EDITOR
        protected override void OnValidate() {
            if (!Application.isPlaying) {
                _LayoutElement = this.GetComponent<LayoutElement>();
                RefreshLayout();
            }
        }
#endif
    }
}