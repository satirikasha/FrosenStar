using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Heist.UI {

    public class MarkerManager : MonoBehaviour {

        public float MarkerRadius = 3;
        //private Dictionary<IMarkerProvider, MarkerWidgetBase> _Markers;

        //void Start() {
        //    _Markers = new Dictionary<IMarkerProvider, MarkerWidgetBase>();

        //    var playerPos = Vector3.zero; // CharacterController.LocalPlayer.RaycastTarget.position;

        //    List<IMarkerProvider> providers = new List<IMarkerProvider>();
        //    foreach (var Col in Physics.OverlapSphere(playerPos, 1000)) {//, LayerMask.GetMask("Interaction", "InteractionAntiSensor"))) {
        //        providers.AddRange(Col.GetComponents<IMarkerProvider>());
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

        //private void AddMarkers(List<IMarkerProvider> providers) {
        //    foreach (var p in providers) {
        //        if (!_Markers.ContainsKey(p)) {
        //            //Debug.Log("Adding Marker for " + p);
        //            _Markers.Add(p, InstantiateMarker(p));
        //            _Markers[p].Show();
        //        }
        //    }
        //}

        //private MarkerWidgetBase InstantiateMarker(IMarkerProvider provider) {
        //    var markerWidget = Instantiate(provider.GetMarkerWidget()).GetComponent<MarkerWidgetBase>();
        //    markerWidget.OnProviderDestroyed += _ => _Markers.Remove(_.MarkerProvider);
        //    markerWidget.transform.SetParent(this.transform);
        //    markerWidget.transform.localScale = Vector3.one;
        //    markerWidget.MarkerProvider = provider;
        //    markerWidget.Init();
        //    return markerWidget;
        //}
    }
}