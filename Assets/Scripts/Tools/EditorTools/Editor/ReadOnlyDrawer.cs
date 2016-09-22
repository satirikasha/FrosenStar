using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var readOnly = attribute as ReadOnlyAttribute;
        var enabled = true;
        if (readOnly.PropertyName != null) {          
            enabled = property.serializedObject.FindProperty(readOnly.PropertyName).boolValue;
            enabled = readOnly.Invert ? !enabled : enabled;
        }

        GUI.enabled = !enabled;
        EditorGUI.PropertyField(position, property);
        GUI.enabled = true;
    }
}