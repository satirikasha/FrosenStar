using System;
using Tools.BehaviourTree;
using UnityEngine;

public class GoToPlayer : Task<ShipBlackboard> {

    public const float SteerSensitivity = 195;
    public const float ViewSensitivity = 0.95f;
    public const float BoundsSensitivity = 0.45f;
    public const float CollisionSensitivity = 1.25f;


    private ShipController _Ship;

    public override void Init() {
        _Ship = ((ShipAIController)BehaviourTree.Executor).Ship;
    }

    public override TaskStatus Run() {
        var direction = (PlayerController.LocalPlayer.Position - _Ship.Position).normalized;

        ApplyLocalAvoidance(ref direction);

        _Ship.SetSteering(CalculateSteering(direction));
        _Ship.SetThrottle(CalculateThrottle(direction));

        return TaskStatus.Running;
    }

    private void ApplyLocalAvoidance(ref Vector3 direction) {
        var lonSpeed = Vector3.Project(_Ship.Velocity, _Ship.transform.forward).magnitude;
        var radius = (lonSpeed * BoundsSensitivity + 1) * _Ship.Width;
        var view = lonSpeed * ViewSensitivity;

        var ray = new Ray(_Ship.transform.position, _Ship.transform.forward);
        var hit = new RaycastHit();
        if (Physics.SphereCast(ray, radius, out hit, view, ~LayerMask.GetMask("Projectile"))) {
            var adjustment = (ray.GetPoint(hit.distance) - hit.point).normalized;
            direction = (adjustment * CollisionSensitivity * (1 - hit.distance / view) + direction).normalized;
        }

        //var resDir = direction;
        //var dist = hit.distance == 0 ? view : hit.distance;
        //GizmoDrawer.AddTask(() => Gizmos.color = Color.yellow);
        //GizmoDrawer.AddTask(() => Gizmos.DrawWireSphere(ray.origin, radius));
        //GizmoDrawer.AddTask(() => Gizmos.DrawWireSphere(ray.GetPoint(dist), radius));
        //GizmoDrawer.AddTask(() => Gizmos.DrawWireCube(hit.point, Vector3.one / 5));
        //GizmoDrawer.AddTask(() => Gizmos.color = Color.Lerp(Color.green, Color.red, 1 - dist / view));
        //GizmoDrawer.AddTask(() => Gizmos.DrawLine(_Ship.transform.position, _Ship.transform.position + resDir * view));
    }

    private float CalculateSteering(Vector3 direction) {
        direction.Normalize();
        var side = Mathf.Sign(Vector3.Dot(_Ship.transform.right, direction));
        var steer = side * (1 - Vector3.Dot(direction, _Ship.transform.forward)) * SteerSensitivity;
        return steer;
    }

    private float CalculateThrottle(Vector3 direction) {
        var distance = (PlayerController.LocalPlayer.Position - _Ship.Position).magnitude;
        if (distance > 3)
            return Vector3.Dot(direction, _Ship.transform.forward)/* * speedMult*/;
        else
            return 0;
    }
}
