using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.BehaviourTree {


    [CreateAssetMenu(fileName = "BehaviourTree", menuName = "AI/BehaviourTree", order = 1)]
    public class BehaviourTree : Task {

        public List<Task> Tasks { get; private set; }
        public Blackboard Blackboard { get; set; }
        public BehaviourTreeExecutor Executor { get; set; }

        public void OnEnable() {
            BehaviourTree = this;
            Debug.Log("Enable BT");
        }


        public override void Init() {
            this.hideFlags = HideFlags.HideAndDontSave;
        }

        public override TaskStatus Run() {
            return Children[0].UpdateTask();
        }

        public void RegisterTask(Task task) {
            if (Tasks == null)
                Tasks = new List<Task>();

            Tasks.Add(task);
        }
    }
}
