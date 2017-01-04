using System;

namespace Tools.BehaviourTree {


    public class Sequence : Task {


        public override void Init() {
            
        }

        public override TaskStatus Run() {
            TaskStatus childStatus;
            foreach (var child in Children) {
                childStatus = child.Run();
                if (childStatus != TaskStatus.Success)
                    return childStatus;
            }
            return TaskStatus.Success;
        }
    }
}
