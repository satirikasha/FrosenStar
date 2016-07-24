using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraArm : MonoBehaviour {

    public float FocusRadius = 5;
    [Space]
    public float Height = 10;
    public float VelocityOffset = 0.25f;
    public float DistanceDamping = 2;
    public float AngleDamping = 3;

    void Start() {
        this.transform.position = GetTargetPosition();
    }

    void FixedUpdate() {

        //Maybe try Damp instead of Lerp for better results??
        this.transform.position = Vector3.Lerp(
            this.transform.position,
            GetTargetPosition(),
            Time.fixedDeltaTime * DistanceDamping
            );
        //this.transform.rotation = Quaternion.Slerp(
        //    this.transform.rotation, 
        //    Quaternion.LookRotation(targetPosition - this.transform.position, Vector3.forward),
        //    Time.fixedDeltaTime * AngleDamping
        //    );
    }

    private Vector3 GetFocusPosition() {
        var offset = Vector3.zero;
        var count = 0;
        foreach (var point in CameraFocusPoint.CameraFocusPoints) {
            var currentOffset = point.transform.position - PlayerController.LocalPlayer.transform.position;
            if (currentOffset.sqrMagnitude > 0 && currentOffset.sqrMagnitude < FocusRadius * FocusRadius) {
                offset += currentOffset * (1 - currentOffset.magnitude / FocusRadius);
                count++;
            }
        }
        if (count > 0) {
            offset /= count;
            return offset + PlayerController.LocalPlayer.transform.position;
        }
        else {
            return PlayerController.LocalPlayer.transform.position;
        }
    }

    private Vector3 GetTargetPosition() {
        return GetFocusPosition() + Vector3.up * (Height + PlayerController.LocalPlayer.Velocity.magnitude * VelocityOffset);
    }
}
