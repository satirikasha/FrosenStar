using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor;

public class TargetPropertyDrawer<T> : TargetPropertyDrawer {

    public T Target { get; private set; }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        base.OnGUI(position, property, label);
        Target = (T)FindTarget(property);
    }
}

public class TargetPropertyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) { }

    public object FindTarget(SerializedProperty property) {
        var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
        if (obj is IList) {
            var array = obj as IList;
            var index = int.Parse(property.propertyPath.Substring(property.propertyPath.IndexOf("[") + 1).Replace("]", ""));
            var type = fieldInfo.FieldType.GetGenericArguments()[0];
            if (index < array.Count)
                return array[index];
            return null;
        }
        return obj;
    }
}
