using System;
using Tools.BehaviourTree;
using UnityEngine;

public class GoToPlayer : Task<ShipBlackboard> {

    private ShipController _Ship;

    public override void Init() {
        Debug.Log("Init");
        _Ship = ((ShipAIController)BehaviourTree.Executor).Ship;
    }

    public override TaskStatus Run() {
        //Debug.Log(CalculateSteering());
        _Ship.SetSteering(1);
        _Ship.SetThrottle(1);
        return TaskStatus.Running;
    }

    private float CalculateSteering() {
        return -0.5f;
    }

    private float CalculateThrottle() {
        return 1;
    }
}
