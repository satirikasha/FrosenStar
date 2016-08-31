using NodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Heist.Quests {

    [Name("Task")]
    [Description("The Transitions of this node will constantly be checked. If any becomes true, the target connected State will Enter regardless of the current State. This node can have no incomming transitions.")]
    public class TaskState : QuestTreeState, IUpdatable {

        public static readonly Color CompleteColor = new Color(0.4f, 0.7f, 0.2f);
        public static readonly Color CancelColor   = new Color(0.6f, 0.3f, 0.6f);
        public static readonly Color FailColor     = new Color(0.8f, 0.2f, 0.2f);

        private static List<Type> _ConditionTypesCache;

        [SerializeField]
        public QuestTask Task = new QuestTask();

        public QuestTaskStatus TaskStatus { get; private set; }

        public override string name {
            get { return string.Format("<color=#ffffcc>{0}</color>", base.name.ToUpper()); }
        }

        public override int maxInConnections { get { return -1; } }
        public override int maxOutConnections { get { return -1; } }
        public override bool allowAsPrime { get { return false; } }

		#if UNITY_EDITOR
        public override Color successColor {
            get {
                switch (TaskStatus) {
                    case QuestTaskStatus.Complete: return CompleteColor;
                    case QuestTaskStatus.Cancel: return CancelColor;
                    case QuestTaskStatus.Fail: return FailColor;
                }
                return base.successColor;
            }
        }
		#endif

        protected override void OnUpdate() {
            base.OnUpdate();

            if (Task.CompleteCondition.Check()) {
                CompleteTask(QuestTaskStatus.Complete);
                return;
            }

            if (Task.CancelCondition.Check()) {
                CompleteTask(QuestTaskStatus.Cancel);
                return;
            }

            if (Task.FailCondition.Check()) {
                CompleteTask(QuestTaskStatus.Fail);
                return;
            }

        }

        public void CompleteTask(QuestTaskStatus status) {
            if (status == QuestTaskStatus.Fail && Task.Mandatory)
                QuestTree.QuestFailed = true;

            this.status = Status.Success;
            TaskStatus = status;
            QuestTree.LeaveState(this);
            OnLeave();
        }

        private void OnLeave() {
            for (var i = 0; i < outConnections.Count; i++) {

                var connection = (QuestTreeConnection)outConnections[i];

                if (!connection.isActive)
                    continue;

                if (status == Status.Success && connection.TaskStatus == TaskStatus) {
                    QuestTree.EnterState((QuestTreeState)connection.targetNode);
                    connection.connectionStatus = Status.Success;
                }
            }
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR
        private void RefreshConditionTypes() {
            if (_ConditionTypesCache == null)
                _ConditionTypesCache = AppDomain.CurrentDomain.GetAssemblies()
                          .SelectMany(assembly => assembly.GetTypes())
                          .Where(type => type.IsSubclassOf(typeof(QuestCondition)) || type == typeof(QuestCondition))
                          .OrderBy(_ => _.ToString().Length)
                          .ToList();
        }

        private int GetSelectedIndex(QuestCondition property, List<Type> classes) {
            return classes.IndexOf(property.GetType());
        }

        private void RefreshValue(ref QuestCondition property, int index, List<Type> conditionTypes) {
            property = (QuestCondition)Activator.CreateInstance(conditionTypes[index]);
        }

        protected override void OnNodeGUI() {
            base.OnNodeGUI();
            UnityEngine.GUILayout.Label(Task.Name);
        }

        protected override void OnNodeInspectorGUI() {
            Task.Mandatory = EditorGUILayout.Toggle("Mandatory", Task.Mandatory);
            Task.Visible = EditorGUILayout.Toggle("Visible", Task.Visible);
            Task.Name = EditorGUILayout.TextField("Task name", Task.Name);

            RefreshConditionTypes();

            OnConditionInspectorGUI(ref Task.CompleteCondition, "Complete condition");
            OnConditionInspectorGUI(ref Task.CancelCondition, "Cancel condition");
            OnConditionInspectorGUI(ref Task.FailCondition, "Fail condition");
        }

        private void OnConditionInspectorGUI(ref QuestCondition condition, string name) {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            var selectedIndex = GetSelectedIndex(condition, _ConditionTypesCache);
            var index = EditorGUILayout.Popup(
                name,
                selectedIndex,
                _ConditionTypesCache.Select(_ => _.Name).ToArray()
                );
            if (selectedIndex != index)
                RefreshValue(ref condition, index, _ConditionTypesCache);
            condition.OnInspectorGUI();
            GUILayout.EndVertical();
        }

#endif
    }
}