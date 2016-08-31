using UnityEngine;
using System.Collections;
using System;
using System.Linq;

[Serializable]
public class ObjectStateCondition : QuestCondition {

    public string QueryObjectID;

    public int SelectedIndex;

    public override bool Check() {
        if (!String.IsNullOrEmpty(QueryObjectID)) {
            var obj = QuestQueryItem.GetItem(QueryObjectID);
            if (obj != null && obj.QueryItem != null) {
                return obj.QueryItem.CheckState(SelectedIndex);
            }
        }
        return false;
    }

    public string[] GetQueryOptions() {
        if (!String.IsNullOrEmpty(QueryObjectID)) {
            var obj = QuestQueryItem.GetItem(QueryObjectID);
            if (obj != null && obj.QueryItem != null) {
                return obj.QueryItem.GetObjectStates();
            }
        }
        return new string[0];

    }

#if UNITY_EDITOR
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var options = QuestQueryItem.GetItemIDs();

        if (options.Count == 0) {
            UnityEditor.EditorGUILayout.HelpBox("There are no quest items in this scene.", UnityEditor.MessageType.Warning);
        }
        else {
            if (!options.Contains(QueryObjectID))
                options.Insert(0, QueryObjectID);  

            QueryObjectID = options[UnityEditor.EditorGUILayout.Popup(
                "Query Object ID",
                options.IndexOf(QueryObjectID),
                options.ToArray()
                )];

            SelectedIndex = UnityEditor.EditorGUILayout.Popup(
                "State",
                SelectedIndex,
                GetQueryOptions()
                );
        }
    }
#endif
}
