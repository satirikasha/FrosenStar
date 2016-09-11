using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

public class ObjectDrawer : TargetPropertyDrawer {

    //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    //    base.OnGUI(position, property, label);
    //    var obj = FindTarget(property);
    //    if (obj != null) {
    //        var inheritedProperty = new SerializedProperty(obj);
    //}

    //private static Dictionary<object, bool> _FoldoutCache = new Dictionary<object, bool>();

    //public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
    //    base.OnGUI(position, property, label);
    //    var obj = FindTarget(property);
    //    if (obj != null) {
    //        if (!_FoldoutCache.ContainsKey(obj))
    //            _FoldoutCache.Add(obj, false);
    //        OnGUI(position, obj, label);
    //    }
    //}

    //public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
    //    var obj = FindTarget(property);
    //    if (obj != null) {
    //        if (!_FoldoutCache.ContainsKey(obj))
    //            _FoldoutCache.Add(obj, false);
    //        return GetPropertyHeight(obj, label);
    //    }
    //    return 0;
    //}

    //public virtual void OnGUI(Rect position, object property, GUIContent label) {
    //    var fields = property.GetType().GetFields();
    //    var rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
    //    _FoldoutCache[property] = EditorGUI.Foldout(rect, _FoldoutCache[property], label, true);
    //    if (_FoldoutCache[property]) {
    //        for (int i = 0; i < fields.Length; i++) {
    //            rect.y += EditorGUIUtility.singleLineHeight;
    //            DrawField(EditorGUI.IndentedRect(rect), property, fields[i]);
    //        }
    //    }
    //}

    //public virtual float GetPropertyHeight(object property, GUIContent label) {
    //    return (_FoldoutCache[property] ? property.GetType().GetFields().Length + 1 : 1) * EditorGUIUtility.singleLineHeight;
    //}

    //public static void DrawField(Rect position, object property, FieldInfo fieldInfo) {
    //    if (fieldInfo.FieldType == typeof(string)) {
    //        fieldInfo.SetValue(property, EditorGUI.TextField(position, new GUIContent(fieldInfo.Name), fieldInfo.GetValue(property) as string));
    //    }
    //    if (fieldInfo.FieldType == typeof(Object)) {
    //        EditorGUI.DrawRect(position, Color.yellow);
    //        fieldInfo.SetValue(property, EditorGUI.ObjectField(position, new GUIContent(fieldInfo.Name), fieldInfo.GetValue(property) as Object, fieldInfo.FieldType, true));
    //    }
    //}
}
