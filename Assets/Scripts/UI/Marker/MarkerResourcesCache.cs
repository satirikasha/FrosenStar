using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI.Markers {


    public static class MarkerResourcesCache {

        private const string MarkerWidgetPath = "Prefabs/UI/Markers";

        private static MarkerWidget[] Markers;

        public static MarkerWidget GetMarker(Type type) {
            PrepareMarkers();
            return Markers.FirstOrDefault(_ => _.GetType() == type);
        }

        public static T GetMarker<T>() where T : MarkerWidget {
            PrepareMarkers();
            return Markers.OfType<T>().FirstOrDefault();
        }

        private static void PrepareMarkers() {
            if (Markers == null)
                Markers = Resources.LoadAll<MarkerWidget>(MarkerWidgetPath);
        }
    }
}
