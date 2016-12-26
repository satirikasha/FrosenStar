using System.Collections.Generic;
using UnityEngine;

namespace Tools.BehaviourTree {


    public abstract class Task<B> where B : Blackboard {

        public BehaviourTree<B> BehaviourTree { get; protected set; }
        public Task<B> Parent { get; private set; }
        public TaskStatus Status { get; private set; }

        protected List<Task<B>> Children { get; set; }

        private bool _Initialized = false;

        public TaskStatus UpdateTask() {
            if (!_Initialized) {
                Init();
                _Initialized = true;
            }

            Status = Run();
            if (Parent != null) {
                switch (Status) {
                    case TaskStatus.Running: Parent.OnChildRunning(this); break;
                    case TaskStatus.Success: Parent.OnChildSuccess(this); break;
                    case TaskStatus.Failure: Parent.OnChildFailure(this); break;
                }
            }

            return Status;
        }

        public void AddChild(Task<B> child) {

            if (Children == null)
                Children = new List<Task<B>>();

            Children.Add(child);
            child.BehaviourTree = BehaviourTree;
            BehaviourTree.RegisterTask(child);
            OnChildAdded(child);
        }

        public abstract void Init();
        public abstract TaskStatus Run();

        public virtual void OnChildRunning(Task<B> child) { }
        public virtual void OnChildSuccess(Task<B> child) { }
        public virtual void OnChildFailure(Task<B> child) { }
        public virtual void OnChildAdded(Task<B> child) { }


    }
}
