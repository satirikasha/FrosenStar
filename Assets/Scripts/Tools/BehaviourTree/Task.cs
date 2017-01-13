using System.Collections.Generic;
using UnityEngine;

namespace Tools.BehaviourTree {

    public abstract class Task : ScriptableObject {

        public BehaviourTree BehaviourTree { get; protected set; }
        public Task Parent { get; private set; }
        public TaskStatus Status { get; private set; }

        protected List<Task> Children { get; set; }

        private bool _Initialized = false;

        public TaskStatus UpdateTask() {
            if (!_Initialized) {
                this.hideFlags = HideFlags.HideAndDontSave;
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

        public void AddChild(Task child) {

            if (Children == null)
                Children = new List<Task>();

            Children.Add(child);
            child.BehaviourTree = BehaviourTree;
            BehaviourTree.RegisterTask(child);
            OnChildAdded(child);
        }

        public static T Instantiate<T>() where T : Task {
            var instance = ScriptableObject.CreateInstance<T>();
            instance.hideFlags = HideFlags.HideAndDontSave;
            return instance;
        }

        public abstract void Init();
        public abstract TaskStatus Run();

        public virtual void OnChildRunning(Task child) { }
        public virtual void OnChildSuccess(Task child) { }
        public virtual void OnChildFailure(Task child) { }
        public virtual void OnChildAdded(Task child) { }


    }
}
