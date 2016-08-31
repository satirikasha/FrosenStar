using NodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using UnityEngine;
using System;
using System.Linq;

namespace Heist.Quests {

    [Name("Action")]
    [Description("The Transitions of this node will constantly be checked. If any becomes true, the target connected State will Enter regardless of the current State. This node can have no incomming transitions.")]
    public class ActionState : QuestTreeState, IUpdatable {

        [SerializeField]
        public List<QuestAction> Actions = new List<QuestAction>();

        public override string name {
            get { return string.Format("<color=#ccffff>{0}</color>", base.name.ToUpper()); }
        }

        public override int maxInConnections { get { return -1; } }
        public override int maxOutConnections { get { return -1; } }
        public override bool allowAsPrime { get { return false; } }

        private static List<Type> _ActionTypesCache;

        protected override void OnUpdate() {
            base.OnUpdate();

            if (outConnections.Count == 0)
                return;

            Actions.ForEach(_ => _.Execute());

            for (var i = 0; i < outConnections.Count; i++) {

                var connection = (QuestTreeConnection)outConnections[i];

                if (!connection.isActive)
                    continue;

                QuestTree.EnterState((QuestTreeState)connection.targetNode);
                connection.connectionStatus = Status.Success;
            }

            QuestTree.LeaveState(this);
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        private QuestAction currentViewAction;

        private void RefreshConditionTypes() {
            if (_ActionTypesCache == null)
                _ActionTypesCache = AppDomain.CurrentDomain.GetAssemblies()
                          .SelectMany(assembly => assembly.GetTypes())
                          .Where(type => type.IsSubclassOf(typeof(QuestAction)) || type == typeof(QuestAction))
                          .OrderBy(_ => _.ToString().Length)
                          .ToList();
        }

        private int GetSelectedIndex(QuestAction property, List<Type> classes) {
            return classes.IndexOf(property.GetType());
        }

        private void RefreshValue(ref QuestAction property, int index, List<Type> conditionTypes) {
            property = (QuestAction)Activator.CreateInstance(conditionTypes[index]);
        }

        protected override void OnNodeGUI() {
            base.OnNodeGUI();
        }

        private void OnActionInspectorGUI(ref QuestAction action) {
            GUI.backgroundColor = action == currentViewAction ? Color.grey : Color.white;
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;
            var selectedIndex = GetSelectedIndex(action, _ActionTypesCache);
            var index = EditorGUILayout.Popup(
                "Action",
                selectedIndex,
                _ActionTypesCache.Select(_ => _.Name).ToArray()
                );
            if (selectedIndex != index)
                RefreshValue(ref action, index, _ActionTypesCache);
            action.OnInspectorGUI();
            GUILayout.EndVertical();

            Event e = Event.current;
            if (e.type == EventType.mouseDown && GUILayoutUtility.GetLastRect().Contains(e.mousePosition)) {
                currentViewAction = action == currentViewAction ? null : action;
            }
        }

        protected override void OnNodeInspectorGUI() {
            RefreshConditionTypes();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            if (Actions.Count == 0) {
                EditorGUILayout.HelpBox("No Actions", MessageType.None);
            }
            else {
                EditorUtils.ReorderableList(Actions, delegate (int i) {
                    var action = Actions[i];
                    OnActionInspectorGUI(ref action);
                    Actions[i] = action;
                    EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
                });
            }
            if (GUILayout.Button("Add action"))
                Actions.Add(new QuestAction());
            if (currentViewAction != null && GUILayout.Button("Remove action")) {
                Actions.Remove(currentViewAction);
                currentViewAction = null;
            }
            GUILayout.EndVertical();
        }

#endif
    }
}