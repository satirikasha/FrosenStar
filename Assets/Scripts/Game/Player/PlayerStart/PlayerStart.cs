using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class PlayerStart : MonoBehaviour {

    [ReadOnly]
    public string ID;

    protected static List<PlayerStart> _Instances = new List<PlayerStart>();
    protected static PlayerStart _Default;

    public virtual void Awake() {
        _Instances.Add(this);
    }

    public virtual void OnDestroy() {
        _Instances.Remove(this);
    }

    public virtual ShipController SpawnShip(ShipItem ship) {
        var go = ship.Instantiate();
        go.transform.position = this.transform.position;
        go.transform.rotation = this.transform.rotation;
        return go;
    }

    public virtual void OnValidate() {
        this.transform.position = Vector3.Scale(this.transform.position, new Vector3(1, 0, 1));
        //var starts = GameObject.FindObjectsOfType<PlayerStart>();
        //if(String.IsNullOrEmpty(ID) || starts.Any(_ => _ != this && _.ID == ID)) {
        //    ID = Guid.NewGuid().ToString().ToUpper();
        //}
    }

    public static List<PlayerStart> GetAvailable() {
        return _Instances;
    }

    public static PlayerStart GetDefault() {
        return _Default;
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.ArrowCap(-1, this.transform.position, this.transform.rotation, UnityEditor.HandleUtility.GetHandleSize(this.transform.position) * 1.25f);
    }
#endif
}
