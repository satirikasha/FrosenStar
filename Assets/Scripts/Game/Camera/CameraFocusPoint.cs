using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFocusPoint : MonoBehaviour {

    public static List<CameraFocusPoint> CameraFocusPoints {
        get
        {
            return _CameraFocusPoints;
        }
    }
    private static List<CameraFocusPoint> _CameraFocusPoints = new List<CameraFocusPoint>();

    [Range(0,1)]
    public float Weight = 0.25f;

    void OnEnable() {
        _CameraFocusPoints.Add(this);
    }

    void OnDisable() {
        _CameraFocusPoints.Remove(this);
    }
}
