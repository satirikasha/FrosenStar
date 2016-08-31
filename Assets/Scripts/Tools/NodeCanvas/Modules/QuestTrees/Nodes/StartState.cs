using NodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Heist.Quests {

    [Name("Start")]
    [Description("The Transitions of this node will constantly be checked. If any becomes true, the target connected State will Enter regardless of the current State. This node can have no incomming transitions.")]
    public class StartState : QuestTreeState, IUpdatable {

        public override string name {
            get { return string.Format("<color=#b3ff7f>{0}</color>", base.name.ToUpper()); }
        }

        public override int maxInConnections { get { return 0; } }
        public override int maxOutConnections { get { return -1; } }
        public override bool allowAsPrime { get { return true; } }

        protected override void OnUpdate() {
            base.OnUpdate();

            if (outConnections.Count == 0)
                return;

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

        protected override void OnNodeGUI() {
            base.OnNodeGUI();
        }

        protected override void OnNodeInspectorGUI() {

            //ShowBaseQuestTreeInspectorGUI();
        }

#endif
    }
}