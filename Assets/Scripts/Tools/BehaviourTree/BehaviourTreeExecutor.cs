using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.BehaviourTree {



    public class BehaviourTreeExecutor : MonoBehaviour {

        public BehaviourTree BehaviourTree;
        public Blackboard Blackboard;

        protected virtual void Awake() {
            BehaviourTree = Task.Instantiate<BehaviourTree>();
            BehaviourTree.Blackboard = Blackboard;
            BehaviourTree.Executor = this;
            BehaviourTree.Init();
            BehaviourTree.hideFlags = HideFlags.HideAndDontSave;
        }

        protected virtual void Update() {
            BehaviourTree.Run();
        }
    }
}
