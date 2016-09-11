#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(StackableItemWrapper))]
public class StackableItemTemplateDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        //EditorGUI.DrawRect(position, Color.cyan);
        var template = (InventoryItemTemplate)property.FindPropertyRelative("Template").objectReferenceValue;
        if (template != null) {
            if (template.GetType().GetField("Quantity") != null) {
                position = EditorGUI.PrefixLabel(position, new GUIContent(template.Name));
                EditorGUI.indentLevel--;
                var intPos = new Rect(position.x, position.y, position.width / 5 - EditorGUIUtility.standardVerticalSpacing, position.height);
                var itemPos = new Rect(position.x + position.width / 5, position.y, position.width * 4 / 5, position.height);
                property.FindPropertyRelative("Quantity").intValue = EditorGUI.IntField(intPos, property.FindPropertyRelative("Quantity").intValue);
                EditorGUI.PropertyField(itemPos, property.FindPropertyRelative("Template"), GUIContent.none);
                EditorGUI.indentLevel++;
            }
            else {
                EditorGUI.PropertyField(position, property.FindPropertyRelative("Template"), new GUIContent(template.Name));
            }
        }
        else {            
            EditorGUI.PropertyField(position, property.FindPropertyRelative("Template"), new GUIContent("Item"));
        }
    }
}
#endif