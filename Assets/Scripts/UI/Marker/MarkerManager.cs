using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Tools;
using System;

namespace UI.Markers {

    public class MarkerManager : SingletonBehaviour<MarkerManager> {

        private static List<MarkerProvider> _MarkerProviders = new List<MarkerProvider>();

        private List<MarkerWidget> _MarkerWidgets = new List<MarkerWidget>();

        public static void RegisterProvider(MarkerProvider provider) {
            _MarkerProviders.Add(provider);
            provider.OnVisibilityChanged += OnProviderVisibilityChanged;
            OnProviderVisibilityChanged(provider);
        }

        public static void UnregisterProvider(MarkerProvider provider) {
            _MarkerProviders.Remove(provider);
            provider.OnVisibilityChanged -= OnProviderVisibilityChanged;
        }

        private static void OnProviderVisibilityChanged(MarkerProvider provider) {
            if (provider.Visible) {
                Instance.GetMarker(provider.RequiredMarkerType).AssignProvider(provider);
            }
        }

        private MarkerWidget GetMarker(Type t) {
            var marker = _MarkerWidgets.FirstOrDefault(_ => !_.isActiveAndEnabled && _.GetType() == t);
            if (marker == null) {
                marker = AddMarker(t);
            }
            return marker;
        }

        private MarkerWidget AddMarker(Type type) {
            var markerWidget = Instantiate(MarkerResourcesCache.GetMarker(type));
            markerWidget.transform.SetParent(this.transform);
            markerWidget.transform.localScale = Vector3.one;
            _MarkerWidgets.Add(markerWidget);
            return markerWidget;
        }

        //private Dictionary<MarkerProvider, MarkerWidget> _Markers;

        //void Start() {
        //    _Markers = new Dictionary<MarkerProvider, MarkerWidget>();

        //    var playerPos = PlayerController.LocalPlayer.Position;

        //    List<MarkerProvider> providers = new List<MarkerProvider>();
        //    foreach (var Col in Physics.OverlapSphere(playerPos, 1000)) {
        //        providers.AddRange(Col.GetComponents<MarkerProvider>());
        //    }

        //    AddMarkers(providers);
        //    StartCoroutine(LazyUpdate());
        //}

        //IEnumerator LazyUpdate() {
        //    while (true) {

        //        yield return new WaitForSeconds(0.5f);

        //        foreach (var m in _Markers) {
        //            m.Value.Show();
        //        }
        //    }
        //}

        //private void AddMarkers(List<MarkerProvider> providers) {
        //    foreach (var p in providers) {
        //        if (!_Markers.ContainsKey(p)) {
        //            //Debug.Log("Adding Marker for " + p);
        //            _Markers.Add(p, InstantiateMarker(p));
        //            _Markers[p].Show();
        //        }
        //    }
        //}

        //private MarkerWidget InstantiateMarker(MarkerProvider provider) {
        //    var markerWidget = Instantiate(MarkerResourcesCache.GetMarker(provider.RequiredMarkerType));
        //    //markerWidget.OnProviderDestroyed += _ => _Markers.Remove(_.MarkerProvider);
        //    markerWidget.transform.SetParent(this.transform);
        //    markerWidget.transform.localScale = Vector3.one;
        //    markerWidget.MarkerProvider = provider;
        //    markerWidget.Init();
        //    return markerWidget;
        //}
    }
}