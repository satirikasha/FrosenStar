using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.BehaviourTree {



    public class BehaviourTree<B> : Task<B> where B : Blackboard {

        public List<Task<B>> Tasks { get; private set; }
        public B Blackboard { get; private set; }
        public BTExecutor<B> Executor { get; private set; }

        public BehaviourTree(B blackboard, BTExecutor<B> executor) {
            BehaviourTree = this;
            Blackboard = blackboard;
            Executor = executor;
        }


        public override void Init() { }

        public override TaskStatus Run() {
            return Children[0].UpdateTask();
        }

        public void RegisterTask(Task<B> task) {
            if (Tasks == null)
                Tasks = new List<Task<B>>();

            Tasks.Add(task);
        }
    }
}
