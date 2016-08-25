using UnityEngine;
using System.Collections;

namespace UI.Markers {



    public abstract class MarkerProvider<T> : MonoBehaviour where T : MarkerData {

        public T Data { get; private set; }

        void Update() {

        }

        public abstract void UpdateData();
    }

    public abstract class MarkerData { }
}
