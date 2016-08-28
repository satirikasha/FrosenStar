using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace UI.Markers {


    public class SlotMarkerWidget : MarkerWidget {

        public override void UpdateMarker(MarkerData data) {
            base.UpdateMarker(data);

            var focusData = (SlotMarkerData)data;
        }
    }
}