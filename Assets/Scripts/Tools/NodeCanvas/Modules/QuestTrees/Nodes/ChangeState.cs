using NodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace Heist.Quests {

    [Name("Force complete")]
    [Description("The Transitions of this node will constantly be checked. If any becomes true, the target connected State will Enter regardless of the current State. This node can have no incomming transitions.")]
    public class ChangeState : QuestTreeState, IUpdatable {

        public override string name {
            get { return string.Format("<color=#ccccff>{0}</color>", base.name.ToUpper()); }
        }

        public override int maxInConnections { get { return -1; } }
        public override int maxOutConnections { get { return -1; } }
        public override bool allowAsPrime { get { return false; } }

        protected override void OnUpdate() {
            base.OnUpdate();

            if (outConnections.Count == 0)
                return;

            for (var i = 0; i < outConnections.Count; i++) {

                var connection = (QuestTreeConnection)outConnections[i];

                if (!connection.isActive)
                    continue;

                var task = (TaskState)connection.targetNode;
                QuestTree.EnterState(task);
                task.CompleteTask(connection.TaskStatus);
                connection.connectionStatus = Status.Success;
            }

            QuestTree.LeaveState(this);
        }

        public override void OnChildConnected(int connectionIndex) {
            base.OnChildConnected(connectionIndex);
            var connection = outConnections[connectionIndex];
            if (!(connection.targetNode is TaskState))
                graph.RemoveConnection(connection);
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnNodeGUI() {
            base.OnNodeGUI();
        }

        protected override void OnNodeInspectorGUI() {
            //base.OnNodeInspectorGUI();
        }

#endif
    }
}