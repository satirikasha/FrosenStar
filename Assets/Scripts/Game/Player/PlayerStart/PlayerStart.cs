using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class PlayerStart : MonoBehaviour {

    [ReadOnly]
    public string ID;

    private static List<PlayerStart> _Instances = new List<PlayerStart>();

    void Awake() {
        _Instances.Add(this);
    }

    void OnDestroy() {
        _Instances.Remove(this);
    }

    public void SpawnShip(ShipItem ship) {
        var go = ship.Instantiate();
        go.transform.position = this.transform.position;
        go.transform.rotation = this.transform.rotation;
        go.transform.localScale = this.transform.localScale;
    }

    public void OnValidate() {
        //var starts = GameObject.FindObjectsOfType<PlayerStart>();
        //if(String.IsNullOrEmpty(ID) || starts.Any(_ => _ != this && _.ID == ID)) {
        //    ID = Guid.NewGuid().ToString().ToUpper();
        //}
    }

    public static List<PlayerStart> GetAvailable() {
        return _Instances;
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.ArrowCap(-1, this.transform.position, this.transform.rotation, UnityEditor.HandleUtility.GetHandleSize(this.transform.position) * 1.25f);
    }
}
#endif