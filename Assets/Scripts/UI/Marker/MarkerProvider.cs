using UnityEngine;
using System.Collections;
using System;

namespace UI.Markers {

    public abstract class MarkerProvider : MonoBehaviour {

        public abstract Type RequiredMarkerType { get; }
  
        public abstract MarkerData GetMarkerData();
    }

    public class MarkerData {
        public bool Visible;
        public Vector3 WorldPosition;
    }
}
