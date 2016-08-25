﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace UI.Markers {


    public class FocusMarkerWidget : MarkerWidget {

        public Image ProgressImage;

        public override void UpdateMarker(MarkerData data) {
            var focusData = (FocusMarkerData)data;
            ProgressImage.fillAmount = focusData.FocusProgress;
        }
    }
}