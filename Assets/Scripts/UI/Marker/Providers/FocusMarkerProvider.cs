using UnityEngine;
using System.Collections;
using System;
using Tools.EQS;
using Tools.UI.Markers;

namespace UI.Markers {


    public class FocusMarkerProvider : MarkerProvider<FocusMarkerWidget, FocusMarkerData, EQSItem> {

        //public override Type RequiredMarkerType {
        //    get {
        //        return typeof(FocusMarkerWidget);
        //    }
        //}

        //public override MarkerData GetMarkerData() {
        //    var data = new FocusMarkerData();
        //    data.WorldPosition = this.transform.position;
        //    data.FocusProgress = (Mathf.Sin(Time.timeSinceLevelLoad) + 1) / 2;
        //    return data;
        //}

        public override bool GetVisibility() {
            return Component == PlayerController.LocalPlayer.SelectedItem;
        }

        public override void UpdateData() {
            Data.WorldPosition = this.transform.position;
            Data.FocusProgress = (Mathf.Sin(Time.timeSinceLevelLoad) + 1) / 2;
        }
    }

    public class FocusMarkerData : MarkerData {
        public float FocusProgress;
    }
}