using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace UI.Markers {

    public class MarkerManager : SingletonBehaviour<MarkerManager> {

        private static List<MarkerProvider> _MarkerProviders = new List<MarkerProvider>();

        private List<MarkerWidget> _MarkerWidgets = new List<MarkerWidget>();

        public static void RegisterProvider(MarkerProvider provider) {
            _MarkerProviders.Add(provider);
        }

        public static void UnregisterProvider(MarkerProvider provider) {
            _MarkerProviders.Remove(provider);

            //if (Instance != null){
            //    var bindedWidget = Instance._MarkerWidgets.ForEach(_ => _.MarkerProvider == provider ? 
            //}
        }

        void Start() {
            StartCoroutine(LazyUpdate());
        }

        IEnumerator LazyUpdate() {
            while (true) {
                yield return new WaitForSeconds(0.5f);

                var viewedProviders  = _MarkerWidgets.Select(_ => _.MarkerProvider);
                var visibleProviders = _MarkerProviders.Where(_ => _.GetMarkerData().Visible);
                var pendingProviders = viewedProviders.Intersect(visibleProviders);

                AddMarkers(pendingProviders);
            }
        }

        private void AddMarkers(IEnumerable<MarkerProvider> providers) {
            foreach (var p in providers) {
               
                    //Debug.Log("Adding Marker for " + p);
                    _MarkerWidgets.Add(InstantiateMarker(p));
                    _Markers[p].Show();
            }
        }

        private MarkerWidget InstantiateMarker(MarkerProvider provider) {
            var markerWidget = Instantiate(MarkerResourcesCache.GetMarker(provider.RequiredMarkerType));
            //markerWidget.OnProviderDestroyed += _ => _Markers.Remove(_.MarkerProvider);
            markerWidget.transform.SetParent(this.transform);
            markerWidget.transform.localScale = Vector3.one;
            markerWidget.MarkerProvider = provider;
            markerWidget.Init();
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