#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ObjectRegistry))]
public class ObjectRegistryEditor : Editor {

    private ReorderableList _PrefabList;
    private ReorderableList _TextureList;

    private void OnEnable() {
        _PrefabList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("PrefabRegistry"),
                false, true, false, false);

        _TextureList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("TextureRegistry"),
                false, true, false, false);

        _PrefabList.elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 3;
        _TextureList.elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 3;

        _PrefabList.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Prefab Registry");
        };

        _TextureList.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Texture Registry");
        };

        _PrefabList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            DrawListElement(_PrefabList, rect, index, isActive, isFocused);
        };

        _TextureList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            DrawListElement(_TextureList, rect, index, isActive, isFocused);
        };
    }

    private void DrawListElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused) {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += EditorGUIUtility.standardVerticalSpacing;
        rect.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(
            rect,
            element.FindPropertyRelative("ID"));
        rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(
            rect,
            element.FindPropertyRelative("Object"));
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        _PrefabList.DoLayoutList();
        _TextureList.DoLayoutList();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clean Up")) {
            ((ObjectRegistry)target).CleanUpRegistry();
        }
        if (GUILayout.Button("Refresh")) {
            ((ObjectRegistry)target).RefreshRegistry();
        }
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif