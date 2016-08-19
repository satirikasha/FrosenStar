using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(IntervalAttribute))]
public class IntervalDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (property.type != "Vector2")
            Debug.LogWarning("Use only with Vector2 type");
        else {
            var range = attribute as IntervalAttribute;
            var minValue = property.FindPropertyRelative("x");
            var maxValue = property.FindPropertyRelative("y");
            var newMin = minValue.floatValue;
            var newMax = maxValue.floatValue;

            const float padding = 100;

            EditorGUI.MinMaxSlider(
                new GUIContent(property.displayName), 
                new Rect(position.x,position.y, position.width - padding, position.height), 
                ref newMin, ref newMax, range.min, range.max
                );

            minValue.floatValue = newMin;
            maxValue.floatValue = newMax;

            property.vector2Value = EditorGUI.Vector2Field(
                new Rect(position.x + position.width - padding + 3, position.y, padding - 3, position.height),
                "",
                property.vector2Value
                );

        }
    }
}