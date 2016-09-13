using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class PlayerStart : MonoBehaviour {

    [ReadOnly]
    public string ID;

    public void SpawnPlayer(ShipItem ship) {
        var go = ship.Instantiate();
        go.transform.position = this.transform.position;
        go.transform.rotation = this.transform.rotation;
    }

    public void OnValidate() {
        //var starts = GameObject.FindObjectsOfType<PlayerStart>();
        //if(String.IsNullOrEmpty(ID) || starts.Any(_ => _ != this && _.ID == ID)) {
        //    ID = Guid.NewGuid().ToString().ToUpper();
        //}
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.ArrowCap(-1, this.transform.position, this.transform.rotation, UnityEditor.HandleUtility.GetHandleSize(this.transform.position) * 1.25f);
    }
}
#endif