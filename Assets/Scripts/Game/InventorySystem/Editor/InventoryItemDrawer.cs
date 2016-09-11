#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(InventoryItem), true)]
public class InventoryItemDrawer : TargetPropertyDrawer<InventoryItem> {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        base.OnGUI(position, property, label);
        if (Target != null) {
            EditorGUI.PropertyField(position, property, new GUIContent(Target.Name, Target.Preview), true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property);
    }
}
#endif