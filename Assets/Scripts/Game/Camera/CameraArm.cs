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
        Debug.DrawLine(PlayerController.LocalPlayer.transform.position, GetFocusPosition());
        this.transform.position = Vector3.Lerp(
            this.transform.position,
            GetTargetPosition(),
            Time.fixedDeltaTime * DistanceDamping
            );
    }

    private Vector3 GetFocusPosition() {
        var offset = Vector3.zero;
        var count = 0;
        foreach (var point in CameraFocusPoint.CameraFocusPoints) {
            var currentOffset = point.transform.position - PlayerController.LocalPlayer.transform.position;
            var offsetMagnitude = currentOffset.magnitude;
            if (point.transform != PlayerController.LocalPlayer.transform && offsetMagnitude < FocusRadius) {
                var distanceMult = 1 - currentOffset.magnitude / FocusRadius;
                offset += currentOffset * distanceMult * distanceMult * point.Weight;
                count++;
            }
        }
        if (count > 0) {
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
