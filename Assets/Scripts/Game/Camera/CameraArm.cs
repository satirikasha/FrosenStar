using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraArm : MonoBehaviour {

    public float FocusRadius = 5;
    [Space]
    public float Height = 10;

    public float DistanceDamping = 2;
    public float AngleDamping = 3;

    void FixedUpdate() {
        var targetPosition = GetTargetPosition();

        this.transform.position = Vector3.Lerp(
            this.transform.position,
            targetPosition + Vector3.up * Height,
            Time.fixedDeltaTime * DistanceDamping
            );
        //this.transform.rotation = Quaternion.Slerp(
        //    this.transform.rotation, 
        //    Quaternion.LookRotation(targetPosition - this.transform.position, Vector3.forward),
        //    Time.fixedDeltaTime * AngleDamping
        //    );
    }

    private Vector3 GetTargetPosition() {
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
}
