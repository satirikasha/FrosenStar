#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PrefabRegistry))]
public class PrefabRegistryEditor : Editor {

    private ReorderableList list;

    private void OnEnable() {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("Registry"),
                false, true, false, false);

        list.elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 3;

        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Prefab Registry");
        };

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += EditorGUIUtility.standardVerticalSpacing;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(
                rect,
                element.FindPropertyRelative("ID"));
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(
                rect,
                element.FindPropertyRelative("Prefab"));
        };
    }


    public override void OnInspectorGUI() {
        serializedObject.Update();
        GUI.enabled = false;
        list.DoLayoutList();
        GUI.enabled = true;
        serializedObject.ApplyModifiedProperties();
    }
}
#endif