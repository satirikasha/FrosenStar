using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace Heist.Quests {

    public class QuestTreeConnection : Connection, ITaskAssignable<ConditionTask> {

        public static readonly Color NotActiveColor  = new Color(0.5f, 0.5f, 0.5f, 1f);
        public static readonly Color InProgressColor = new Color(0.0f, 0.5f, 0.5f, 1f);
        public static readonly Color CompleteColor   = new Color(0.5f, 1.0f, 0.5f, 1f);
        public static readonly Color CanceledColor   = new Color(0.5f, 0.5f, 0.5f, 1f);
        public static readonly Color FailedColor     = new Color(1.0f, 0.5f, 0.5f, 1f);

        [SerializeField]
        public QuestTaskStatus TaskStatus { get; private set; }

        [SerializeField]
        private ConditionTask _condition;

        public bool IsTaskOutput {
            get {
                return sourceNode is TaskState;
            }
        }

        public bool IsChangeStateOutput {
            get {
                return sourceNode is ChangeState;
            }
        }

        public ConditionTask condition {
            get { return _condition; }
            set { _condition = value; }
        }

        public Task task {
            get { return condition; }
            set { condition = (ConditionTask)value; }
        }

		#if UNITY_EDITOR
        protected override Color defaultColor {
            get {
                if (IsTaskOutput || IsChangeStateOutput) {
                    if (TaskStatus == QuestTaskStatus.Complete)
                        return CompleteColor;
                    if (TaskStatus == QuestTaskStatus.Cancel)
                        return CanceledColor;
                    if (TaskStatus == QuestTaskStatus.Fail)
                        return FailedColor;
                }
                return base.defaultColor;
            }
        }
		#endif

        ///Perform the transition disregarding whether or not the condition (if any) is valid
        public void PerformTransition() {
            (graph as QuestTree).EnterState((QuestTreeState)targetNode);
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override TipConnectionStyle tipConnectionStyle {
            get { return TipConnectionStyle.Arrow; }
        }

        protected override string GetConnectionInfo(bool isExpanded) {
            if (isExpanded) {
                if (IsTaskOutput) {
                    return "On" + TaskStatus;
                }
                else {
                    if (IsChangeStateOutput) {
                        return TaskStatus.ToString();
                    }
                    else {
                        return "OnFinish";
                    }
                }
            }
            else {
                return "...";
            }
        }

        protected override void OnConnectionInspectorGUI() {
            base.OnConnectionInspectorGUI();
            if (IsTaskOutput || IsChangeStateOutput) {
                TaskStatus = (QuestTaskStatus)EditorGUILayout.EnumPopup("Task status", TaskStatus);
            }
        }

#endif
    }

    public enum QuestTaskStatus {
        Complete,
        Cancel,
        Fail
    }
}