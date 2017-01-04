using System.Collections;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityEngine;

public class ShipAIController : BehaviourTreeExecutor {

    public ShipController Ship { get; private set; }

    protected override void Awake() {
        base.Awake();
        Ship = this.GetComponent<ShipController>();
        Construct();
    }

    private void Construct() {
        Ship.RefreshSlots();
        Ship.ItemSlots.ForEach(_ => _.Construct());

        var move = Task.Instantiate<MoveTask>();
        BehaviourTree.AddChild(move);
    }
}
