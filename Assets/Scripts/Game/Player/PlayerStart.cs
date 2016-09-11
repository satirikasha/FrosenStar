using UnityEngine;
using System.Collections;

public class PlayerStart : MonoBehaviour {
    public ShipItemTemplate DefaultShip;

    void Awake() {
        var go = (DefaultShip.GenerateItem() as ShipItem).Instantiate();
        go.transform.position = this.transform.position;
        go.transform.rotation = this.transform.rotation;
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.ArrowCap(-1, this.transform.position, this.transform.rotation, UnityEditor.HandleUtility.GetHandleSize(this.transform.position) * 1.25f);
    }
}
#endif