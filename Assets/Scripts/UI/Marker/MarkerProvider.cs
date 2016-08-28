using UnityEngine;
using System.Collections;
using System;

namespace UI.Markers {

    public abstract class MarkerProvider : MonoBehaviour {

        public event Action<MarkerProvider> OnVisibilityChanged;

        public bool Visible {
            get
            {
                return _Visible;
            }
            set
            {
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
