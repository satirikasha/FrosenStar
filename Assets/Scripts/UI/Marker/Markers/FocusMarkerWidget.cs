using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Tools.UI.Markers;

namespace UI.Markers {


    public class FocusMarkerWidget : MarkerWidget {

        public Image ProgressImage;
        public Image FocusedImage;

        public override void UpdateMarker(MarkerData data) {
            base.UpdateMarker(data);

            var focusData = (FocusMarkerData)data;
            if (focusData.LookAt) {
                ProgressImage.fillAmount = focusData.Focused ? 1 : focusData.FocusProgress;
                FocusedImage.enabled = focusData.Focused;
            }
        }
    }
}