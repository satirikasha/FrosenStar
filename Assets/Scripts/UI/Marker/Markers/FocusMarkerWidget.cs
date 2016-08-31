using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Tools.UI.Markers;

namespace UI.Markers {


    public class FocusMarkerWidget : MarkerWidget {

        public Image ProgressImage;

        public override void UpdateMarker(MarkerData data) {
            base.UpdateMarker(data);

            var focusData = (FocusMarkerData)data;
            ProgressImage.fillAmount = focusData.FocusProgress;
        }
    }
}