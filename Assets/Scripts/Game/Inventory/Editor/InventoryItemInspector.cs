#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(InventoryItem))]
public class InventoryItemInspector : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        //base.OnGUI(position, property, label);
        //EditorGUILayout.PropertyField(property.FindPropertyRelative("Name"));
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUIUtility.singleLineHeight;
    }

    //public override void OnGUI() {
    //    serializedObject.Update();
    //    EditorGUILayout.BeginVertical();
    //    EditorGUILayout.BeginHorizontal();
    //    EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"));
    //    EditorGUILayout.PropertyField(serializedObject.FindProperty("Preview"));
    //    EditorGUILayout.EndHorizontal();
    //    EditorGUILayout.PropertyField(serializedObject.FindProperty("Description"));
    //    EditorGUILayout.EndVertical();
    //    serializedObject.ApplyModifiedProperties();
    //}
}
#endif
