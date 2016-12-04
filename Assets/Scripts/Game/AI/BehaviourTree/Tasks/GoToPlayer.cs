using System;
using Tools.BehaviourTree;

public class GoToPlayer : Task<ShipBlackboard> {

    public override TaskStatus Run() {

        return TaskStatus.Running;
    }
}
