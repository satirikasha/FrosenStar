using UnityEngine;
using System.Collections;
using System;

namespace Tools.UI.Markers {

    public abstract class MarkerProvider<M, D, C> : MarkerProvider<M, D> where M : MarkerWidget where D : MarkerData, new() {

        protected C Component { get; set; }

        public virtual void Awake() {
            InitComponent();
        }

        public virtual void InitComponent() {
            Component = this.GetComponent<C>();
        }
    }



    public abstract class MarkerProvider<M, D> : MarkerProvider where M : MarkerWidget where D : MarkerData, new() {

        public sealed override Type RequiredMarkerType {
            get {
                return typeof(M);
            }
        }

        protected D Data {
            get {
                return _Data;
            }
            set {
                _Data = value;
            }
        }
        private D _Data = new D();

        public sealed override MarkerData GetMarkerData() {
            UpdateData();
            return Data;
        }

        public abstract void UpdateData();
    }



    public abstract class MarkerProvider : MonoBehaviour {

        public event Action<MarkerProvider> OnVisibilityChanged;

        public bool Visible {
            get {
                return _Visible;
            }
            set {
                if (_Visible != value) {
                    _Visible = value;
                    if (OnVisibilityChanged != null)
                        OnVisibilityChanged(this);
                }
            }
        }
        private bool _Visible;

        void OnEnable() {
            MarkerManager.RegisterProvider(this);
        }

        void OnDisable() {
            MarkerManager.UnregisterProvider(this);
        }

        public void Update() {
            Visible = GetVisibility();
        }

        public abstract Type RequiredMarkerType { get; }

        public abstract MarkerData GetMarkerData();

        public abstract bool GetVisibility();
    }

    public class MarkerData {
        public Vector3 WorldPosition;
    }
}
