using System;
using Tools.BehaviourTree;
using UnityEngine;

[Serializable]
public class ShipBlackboard : Blackboard {
    public Vector3 TargetPosition;
    public GameObject TargetEnemy;
}
