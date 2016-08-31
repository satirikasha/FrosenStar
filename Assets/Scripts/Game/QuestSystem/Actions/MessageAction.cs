using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class MessageAction : QuestAction {

    public string QueryObjectID;
    public string Message;

    public override void Execute() {
        if (!String.IsNullOrEmpty(QueryObjectID)) {
            var obj = QuestQueryItem.GetItem(QueryObjectID);
            obj.BroadcastMessage(Message);
        }
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

            Message = UnityEditor.EditorGUILayout.TextField("Message", Message);
        }
    }
#endif
}
