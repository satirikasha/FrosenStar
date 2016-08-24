using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI.Markers {


    public static class MarkerResourcesCache {

        private const string MarkerWidgetPath = "Prefabs/UI/Markers";

        private static MarkerWidget[] Markers;

        public static T GetMarker<T>() where T : MarkerWidget {
            if (Markers == null)
                Markers = Resources.LoadAll<MarkerWidget>(MarkerWidgetPath);

            return Markers.OfType<T>().FirstOrDefault();
        }
    }
}
