﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Tools.UI.Markers;

namespace UI.Markers {


    public class SlotMarkerWidget : MarkerWidget<SlotMarkerData> {

        public RawImage Preview;

        public override void UpdateMarker(SlotMarkerData data) {
            Preview.texture = data.Preview;
        }
    }
}