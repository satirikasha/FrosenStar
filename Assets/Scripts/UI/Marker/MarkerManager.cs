using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace UI.Markers {

    public class MarkerManager : SingletonBehaviour<MarkerManager> {

        private List<MarkerWidget> _Markers;
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