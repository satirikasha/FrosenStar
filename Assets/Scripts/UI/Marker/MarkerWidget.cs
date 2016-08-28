using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace UI.Markers {

    public abstract class MarkerWidget : MonoBehaviour {

        //public event Action<MarkerWidget> OnProviderDestroyed;

        public bool Visible { get; protected set; }

        protected RectTransform RectTransform { get; private set; }

        public MarkerProvider _MarkerProvider;
        private Animator _Animator;
        private Coroutine _CurrentDisableCor;

        void Awake() {
            RectTransform = (RectTransform)this.transform;
            _Animator = this.GetComponent<Animator>();
        }

        void LateUpdate() {
            if (_MarkerProvider != null) {
                UpdateMarker(_MarkerProvider.GetMarkerData());
            }
            else {
                Hide();
            }
        }

        public virtual void UpdateMarker(MarkerData data) {
            RectTransform.localPosition = TransformPosition(data.WorldPosition);
        }

        public void AssignProvider(MarkerProvider provider) {
            _MarkerProvider = provider;
            provider.OnVisibilityChanged += OnProviderVisibilityChanged;
            Show();
        }

        private void OnProviderVisibilityChanged(MarkerProvider provider) {
            if (!provider.Visible) {
                provider.OnVisibilityChanged -= OnProviderVisibilityChanged;
                _MarkerProvider = null;
                Hide();
            }
        }

        public void Show() {
            if (!Visible) {
                Visible = true;
                if (_CurrentDisableCor != null)
                    StopCoroutine(_CurrentDisableCor);
                this.gameObject.SetActive(true);
                if (_Animator)
                    _Animator.SetBool("Visible", true);
            }
        }

        public void Hide() {
            if (Visible) {
                Visible = false;
                if (_Animator)
                    _Animator.SetBool("Visible", false);
                _CurrentDisableCor = StartCoroutine(DisableOnAnimationFinished());
            }
        }

        private IEnumerator DisableOnAnimationFinished() {
            yield return null;
            if (_Animator != null)
                yield return new WaitUntil(() => _Animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 1);
            //yield return new WaitForSeconds(_Animator.GetCurrentAnimatorStateInfo(0).length);
            this.gameObject.SetActive(false);
        }

        protected virtual Vector2 TransformPosition(Vector3 position) {
            return Vector3.Scale(
                new Vector3(((RectTransform)RectTransform.parent).rect.width,
                ((RectTransform)RectTransform.parent).rect.height),
                UnityEngine.Camera.main.WorldToViewportPoint(position) - Vector3.one * 0.5f
                );
        }
    }
}