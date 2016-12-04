using System.Collections;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityEngine;

public class ShipAIController : BTExecutor<Blackboard> {

    protected override void Awake() {
        base.Awake();
        Construct();
    }

    private void Construct() {
        //BehaviourTree.AddChild()
    }
}
