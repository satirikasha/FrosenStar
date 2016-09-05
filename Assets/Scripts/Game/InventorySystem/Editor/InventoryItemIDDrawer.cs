#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

[CustomPropertyDrawer(typeof(InventoryItemIDAttribute))]
public class InventoryItemIDDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var options  = InventoryItemsConfig.GetItemNames();
        var selected = options.IndexOf(property.stringValue);
        var result   = EditorGUI.Popup(position, "Inventory Item", selected, options.ToArray());
        property.stringValue = options.ElementAtOrDefault(result);
    }
}
#endif