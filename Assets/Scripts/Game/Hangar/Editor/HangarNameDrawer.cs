using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

[CustomPropertyDrawer(typeof(HangarNameAttribute))]
public class HangarNameDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var hangarNames = ApplicationManager.GetHangarNames();

        if (hangarNames.Count == 0) {
            EditorGUI.LabelField(position, "No hangars available");
            return;
        }

        var selectedValue = hangarNames.IndexOf(property.stringValue);
        selectedValue = selectedValue == -1 ? 0 : selectedValue;
        property.stringValue = hangarNames[
            EditorGUI.Popup(
            position,
            "Hangar name",
            selectedValue,
            hangarNames.ToArray())
            ];
    }
}
