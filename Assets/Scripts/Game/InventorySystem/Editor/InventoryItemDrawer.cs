//#if UNITY_EDITOR
//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//using System.Collections.Generic;
//using System;
//using System.Linq;

//[CustomPropertyDrawer(typeof(InventoryItem))]
//public class InventoryItemDrawer : PropertyDrawer {

//    private static List<Type> _ConditionTypesCache;
//    private static Dictionary<int, bool> _Foldout;

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
//        RefreshConditionTypes();
//        var selectedIndex = GetSelectedIndex(property, _ConditionTypesCache);
//        var classRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
//        var index = EditorGUI.Popup(
//            classRect,
//            " ",
//            selectedIndex,
//            _ConditionTypesCache.Select(_ => _.Name).ToArray()
//            );
//        if (selectedIndex != index)
//            RefreshValue(ref property, index, _ConditionTypesCache);

//        var id = property.FindPropertyRelative("ID").intValue;
//        var propertyRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height);
//        RefreshFoldout(id);
//        _Foldout[id] = EditorGUI.Foldout(position, _Foldout[id], label, true);
//        if (_Foldout[id]) {
//            EditorGUI.indentLevel++;
//            EditorGUI.PropertyField(propertyRect, property, new GUIContent("Item"), true);
//            EditorGUI.indentLevel--;
//        }
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
//        var id = property.FindPropertyRelative("ID").intValue;
//        RefreshFoldout(id);
//        if (_Foldout[property.FindPropertyRelative("ID").intValue]) {
//            return EditorGUI.GetPropertyHeight(property) + EditorGUIUtility.singleLineHeight;
//        }
//        else {
//            return EditorGUIUtility.singleLineHeight;
//        }
//    }

//    private void RefreshFoldout(int id) {
//        if (_Foldout == null)
//            _Foldout = new Dictionary<int, bool>();
//        if (!_Foldout.ContainsKey(id))
//            _Foldout.Add(id, false);
//    }

//    private void RefreshConditionTypes() {
//        if (_ConditionTypesCache == null)
//            _ConditionTypesCache = AppDomain.CurrentDomain.GetAssemblies()
//                      .SelectMany(assembly => assembly.GetTypes())
//                      .Where(type => type.IsSubclassOf(typeof(InventoryItem)) || type == typeof(InventoryItem))
//                      .OrderBy(_ => _.ToString().Length)
//                      .ToList();
//    }

//    private int GetSelectedIndex(SerializedProperty property, List<Type> classes) {
//        var type = fieldInfo.FieldType.ToString();
//        //int start = type.LastIndexOf("[") + 1;
//        //int end = type.LastIndexOf("]", start);
//        //string result = type.Substring(start, end - start);
//        Debug.Log(property.propertyPath);
//        return 0;// classes.IndexOf(property.objectReferenceValue.GetType());
//    }

//    private void RefreshValue(ref SerializedProperty property, int index, List<Type> conditionTypes) {
//        //property.objectReferenceValue = (QuestCondition)Activator.CreateInstance(conditionTypes[index]);
//    }
//}
//#endif