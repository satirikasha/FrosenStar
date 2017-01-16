using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColorRamp))]
public class ColorRampInspector : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        ColorRamp ramp = (ColorRamp)target;

        if (GUILayout.Button("Generate"))
            ramp.Generate();
    }
}
