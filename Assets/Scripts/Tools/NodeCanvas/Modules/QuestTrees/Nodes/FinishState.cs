using NodeCanvas;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Heist.Quests {

    [Name("Finish")]
    [Description("The Transitions of this node will constantly be checked. If any becomes true, the target connected State will Enter regardless of the current State. This node can have no incomming transitions.")]
    public class FinishState : QuestTreeState, IUpdatable {

        public override string name {
            get { return string.Format("<color=#b3ff7f>{0}</color>", base.name.ToUpper()); }
        }

        public override int maxInConnections { get { return -1; } }
        public override int maxOutConnections { get { return 0; } }
        public override bool allowAsPrime { get { return false; } }

        protected override void OnUpdate() {
            base.OnUpdate();

            QuestTree.QuestComplete = true;

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

        }

#endif
    }
}