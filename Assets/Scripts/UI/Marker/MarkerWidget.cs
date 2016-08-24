using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UI.Markers {


    public abstract class MarkerWidget : MonoBehaviour {

        public event Action<MarkerWidget> OnProviderDestroyed;

        public MarkerProvider MarkerProvider;

        public bool Visible { get; private set; }

        protected RectTransform RectTransform { get; private set; }
        private Animator _Animator;

        private Coroutine CurrentDisableCor;

        // Use this for initialization
        public virtual void Init() {
            RectTransform = (RectTransform)this.transform;
            _Animator = this.GetComponent<Animator>();

            this.gameObject.SetActive(false);
            Visible = false;
        }

        // Update is called once per frame
        void LateUpdate() {
            
            if(MarkerProvider == null || ((MonoBehaviour)MarkerProvider)==null) {
                OnProviderDestroyed(this);
                Destroy(this.gameObject);
                return;
            }

            //if (!MarkerProvider.GetMarkerVisibility())
            //    Hide();
            //SetPosition(TransformPosition(MarkerProvider.GetMarkerWorldPos()));
            //SetType(MarkerProvider.GetMarkerType());
            //SetColor(MarkerProvider.GetMarkerColor());
            //SetMessage(MarkerProvider.GetMarkerMessage());
        }

        public void Show() {
            //if (!Visible && MarkerProvider.GetMarkerVisibility()) {
            //    Visible = true;
            //    if (CurrentDisableCor != null)
            //        StopCoroutine(CurrentDisableCor);
            //    this.gameObject.SetActive(true);
            //    if (_Animator)
            //        _Animator.SetBool("Visible", true);
            //}
        }

        public void Hide() {
            if (Visible) {
                Visible = false;
                if (_Animator)
                    _Animator.SetBool("Visible", false);
                CurrentDisableCor = StartCoroutine(DisableOnAnimationFinished());
            }
        }

        private IEnumerator DisableOnAnimationFinished() {
            yield return null;
            yield return new WaitForSeconds(_Animator.GetCurrentAnimatorStateInfo(0).length);
            this.gameObject.SetActive(false);
        }

        protected virtual Vector2 TransformPosition(Vector3 position) {
            return Vector3.Scale(
                new Vector3(((RectTransform)RectTransform.parent).rect.width, 
                ((RectTransform)RectTransform.parent).rect.height),
                UnityEngine.Camera.main.WorldToViewportPoint(position) - Vector3.one * 0.5f
                );
        }

        public virtual void SetPosition(Vector2 position) {
            RectTransform.localPosition = new Vector3(position.x, position.y, 0);
        }

        public virtual void SetColor(Color color) { }

        public virtual void SetType(int type) { }

        public virtual void SetMessage(string message) { }
    }
}