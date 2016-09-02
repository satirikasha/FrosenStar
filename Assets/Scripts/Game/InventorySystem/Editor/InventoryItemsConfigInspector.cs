//#if UNITY_EDITOR
//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//using UnityEditorInternal;

//[CustomEditor(typeof(InventoryItemsConfig))]
//public class InventoryItemsConfigInspector : Editor {

//    private ReorderableList list;

//    private void OnEnable() {
//        list = new ReorderableList(serializedObject,
//                serializedObject.FindProperty("Items"),
//                true, true, true, true
//        );
//        list.drawHeaderCallback = (Rect rect) => {
//            EditorGUI.LabelField(rect, "Items");
//        };
//        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
//            EditorGUIUtility.hierarchyMode = false;
//            var element = list.serializedProperty.GetArrayElementAtIndex(index);
//            rect.y += EditorGUIUtility.standardVerticalSpacing;
//            EditorGUI.PropertyField(rect, element, new GUIContent(element.FindPropertyRelative("Name").stringValue), true);
//        };
//        list.elementHeightCallback = (int index) => {
//            var element = list.serializedProperty.GetArrayElementAtIndex(index);
//            return EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing * 2;
//        };
//    }

//    public override void OnInspectorGUI() {
//        serializedObject.Update();
//        list.DoLayoutList();
//        serializedObject.ApplyModifiedProperties();
//    }
//}
//#endif
