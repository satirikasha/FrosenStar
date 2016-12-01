using UnityEngine;
using System.Collections;
using System;
using Tools.EQS;
using Tools.UI.Markers;

namespace UI.Markers {


    public abstract class FocusMarkerProvider : MarkerProvider<FocusMarkerWidget, FocusMarkerData, FocusedItem> {

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
            return FocusedItem.CurrentLookAtItem == Component || FocusedItem.CurrentFousedItem == Component;
        }

        public override void UpdateData() {
            Data.WorldPosition = this.transform.position;
            Data.FocusProgress = Component.NormalizedFocus;
            Data.Focused = Component.Focused;
            Data.LookAt = Component.LookAt;
        }
    }

    public class FocusMarkerData : MarkerData {
        public float FocusProgress;
        public bool Focused;
        public bool LookAt;
    }
}