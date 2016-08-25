using UnityEngine;
using System.Collections;
using System;

namespace UI.Markers {


    public class FocusMarkerProvider : MarkerProvider {

        public override Type RequiredMarkerType {
            get {
                return typeof(FocusMarkerWidget);
            }
        }

        public override MarkerData GetMarkerData() {
            var data = new FocusMarkerData();
            data.WorldPosition = this.transform.position;
            data.FocusProgress = Mathf.Sin(Time.timeSinceLevelLoad);
            return data;
        }
    }

    public class FocusMarkerData : MarkerData {
        public float FocusProgress;
    }
}