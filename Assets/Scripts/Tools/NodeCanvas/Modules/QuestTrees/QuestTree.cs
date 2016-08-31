using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using NodeCanvas;
using System.Collections.Generic;

namespace Heist.Quests {

    public class QuestTree : Graph {

        public TaskState[] Tasks;

        private IUpdatable[] _UpdatableNodes;
        private TaskState[] _MandatoryTasks;

        private event System.Action<IState> CallbackEnter;
        private event System.Action<IState> CallbackStay;
        private event System.Action<IState> CallbackExit;

        public HashSet<QuestTreeState> CurrentStates { get; private set; }

        public override System.Type baseNodeType { get { return typeof(QuestTreeState); } }
        public override bool requiresAgent { get { return false; } }
        public override bool requiresPrimeNode { get { return true; } }
        public override bool autoSort { get { return false; } }
        public override bool useLocalBlackboard { get { return false; } }
        public bool QuestComplete { get; set; }
        public bool QuestFailed { get; set; }

        public void StartGraph() {
            ResetGraph();
            OnGraphValidate();
            OnGraphStarted();
        }

        public void ResetGraph() {
            QuestComplete = false;
            QuestFailed = false;
            allNodes.ForEach(_ => _.Reset());
        }

        new public void UpdateGraph() {
            OnGraphUpdate();
        }

        protected override void OnGraphValidate() {
            base.OnGraphValidate();
            if (primeNode == null) {
                AddNode<StartState>(new Vector2(5500, 5250));
                AddNode<FinishState>(new Vector2(5500, 5500));
            }
        }

        protected override void OnGraphStarted() {
            _UpdatableNodes = allNodes.OfType<IUpdatable>().ToArray();
            _MandatoryTasks = allNodes.OfType<TaskState>().ToArray();
            Tasks = allNodes.OfType<TaskState>().ToArray();
            CurrentStates = new HashSet<QuestTreeState>();
            EnterState((QuestTreeState)primeNode);
        }

        protected override void OnGraphUpdate() {
            foreach (var currentState in CurrentStates) {
                if (currentState.status == Status.Running) {
                    currentState.Update();
                }
            }
            PerformTransitions();
        }

        private List<QuestTreeState> _StatesToEnter = new List<QuestTreeState>();
        ///Enter a state providing the state itself
        public bool EnterState(QuestTreeState state) {

            if (state == null) {
                Debug.LogWarning("Tried to Enter Null State");
                return false;
            }

            _StatesToEnter.Add(state);
            return true;
        }

        private List<QuestTreeState> _StatesToLeave = new List<QuestTreeState>();
        public bool LeaveState(QuestTreeState state) {

            if (state == null) {
                Debug.LogWarning("Tried to Enter Null State");
                return false;
            }

            _StatesToLeave.Add(state);
            return true;
        }

        private void PerformTransitions() {
            _StatesToEnter.ForEach(_ => {
                if (!CurrentStates.Contains(_)) {
                    CurrentStates.Add(_);
                    _.OnEnterState();
                    if (CallbackEnter != null) {
                        CallbackEnter(_);
                    }
                }
            });
            _StatesToEnter.Clear();

            _StatesToLeave.ForEach(_ => {
                CurrentStates.Remove(_);
                _.OnLeaveState();
                if (CallbackExit != null) {
                    CallbackExit(_);
                }
            });
            _StatesToLeave.Clear();
        }

        //Gather and creates delegates from MonoBehaviours on agents that implement required methods
        //void GatherDelegates() {

        //    foreach (var _mono in agent.gameObject.GetComponents<MonoBehaviour>()) {

        //        var mono = _mono;
        //        var enterMethod = mono.GetType().RTGetMethod("OnStateEnter");
        //        var stayMethod = mono.GetType().RTGetMethod("OnStateUpdate");
        //        var exitMethod = mono.GetType().RTGetMethod("OnStateExit");

        //        if (enterMethod != null) {
        //            try { CallbackEnter += enterMethod.RTCreateDelegate<System.Action<IState>>(mono); } //JIT
        //            catch { CallbackEnter += (m) => { enterMethod.Invoke(mono, new object[] { m }); }; } //AOT
        //        }

        //        if (stayMethod != null) {
        //            try { CallbackStay += stayMethod.RTCreateDelegate<System.Action<IState>>(mono); } //JIT
        //            catch { CallbackStay += (m) => { stayMethod.Invoke(mono, new object[] { m }); }; } //AOT
        //        }

        //        if (exitMethod != null) {
        //            try { CallbackExit += exitMethod.RTCreateDelegate<System.Action<IState>>(mono); } //JIT
        //            catch { CallbackExit += (m) => { exitMethod.Invoke(mono, new object[] { m }); }; } //AOT
        //        }
        //    }
        //}

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        [UnityEditor.MenuItem("Tools/Create/Quest Tree", false, 0)]
        public static void Editor_CreateGraph() {
            var newGraph = EditorUtils.CreateAsset<QuestTree>(true);
            UnityEditor.Selection.activeObject = newGraph;
        }

        [UnityEditor.MenuItem("Assets/Create/Quest Tree")]
        public static void Editor_CreateGraphFix() {
            var path = EditorUtils.GetAssetUniquePath("QuestTree.asset");
            var newGraph = EditorUtils.CreateAsset<QuestTree>(path);
            UnityEditor.Selection.activeObject = newGraph;
        }

#endif
    }
}