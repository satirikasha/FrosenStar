using System.Collections;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityEngine;

public class ShipAIController : BTExecutor<ShipBlackboard> {

    public ShipController Ship { get; private set; }

    protected override void Awake() {
        base.Awake();
        Ship = this.GetComponent<ShipController>();
        Construct();
    }

    private void Construct() {
        Ship.RefreshSlots();
        Ship.ItemSlots.ForEach(_ => _.Construct());

        BehaviourTree.AddChild(new GoToPlayer());
    }
}
