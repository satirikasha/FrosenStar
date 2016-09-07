using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(HangarNameAttribute))]
public class HangarNameDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        property.intValue
    }
}
