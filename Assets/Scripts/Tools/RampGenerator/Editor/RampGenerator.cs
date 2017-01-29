using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RampGenerator : EditorWindow {

    [MenuItem("Tools/RampGenerator")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(RampGenerator));
    }

    void OnGUI() {
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.EndHorizontal();
    }
}
