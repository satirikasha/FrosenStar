using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.BehaviourTree {



    public class BTExecutor<B> : MonoBehaviour where B : Blackboard {

        public BehaviourTree<B> BehaviourTree;
        public B Blackboard;

        protected virtual void Awake() {
            BehaviourTree = new BehaviourTree<B>(Blackboard, this);
        }

        protected virtual void Update() {
            BehaviourTree.Run();
        }
    }
}
