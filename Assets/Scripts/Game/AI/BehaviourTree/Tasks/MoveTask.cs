using System;
using Tools.BehaviourTree;
using UnityEngine;

public class MoveTask : Task {

    public const float SteerSensitivity = 100;
    public const float ViewSensitivity = 0.95f;
    public const float BoundsSensitivity = 0.45f;
    public const float CollisionSensitivity = 1.25f;
    public const float StoppingRadius = 3;
    public const float StoppingSensitivity = 1.15f;


    private ShipController _Ship;
    private Vector3 _Target;
    private float _StoppingDistance;
    private bool _UseSlowDown = true;

    private float _HealthStamp = 1; //(Debug general logic)
    private bool _Evading; //(Debug general logic)
    private int state;

    public override void Init() {
        _Ship = ((ShipAIController)BehaviourTree.Executor).Ship;
    }

    public override TaskStatus Run() {

        //Debug general logic
        var ray = new Ray(_Ship.transform.position + _Ship.transform.forward, _Ship.transform.forward);
        var hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 10)) {
            if(hit.transform == ShipController.LocalShip.transform) {
                _Ship.StartFire();
            }
            else {
                _Ship.StopFire();
            }
        }
        else {
            _Ship.StopFire();
        }

        //if (_Ship.NormalizedEnergy > 0.1f && _Ship.NormalizedHealth > 0.2f) {
        if (state % 2 == 1) {
            if (!_Evading/*_HealthStamp - _Ship.NormalizedHealth > 0.1f*/) {
                _HealthStamp = _Ship.NormalizedHealth;
                Debug.Log("Evade");
                _Evading = true;
                _Target = ShipController.LocalShip.Position + (ShipController.LocalShip.Position - _Ship.transform.position).normalized * 4;// + (ShipController.LocalShip.transform.right * UnityEngine.Random.Range(-1f, 1f) - ShipController.LocalShip.transform.forward) * 3;
                _StoppingDistance = 0.5f;
                _UseSlowDown = false;
            }
        }
        else {
            _Target = ShipController.LocalShip.Position;
            _StoppingDistance = 2f;
            _UseSlowDown = false;
        }
            //else {
            //    if (!_Evading) {
            //        _Target = ShipController.LocalShip.Position;
            //        _StoppingDistance = 3.5f;
            //        _UseSlowDown = true;
            //    }
            //}
        //}
        //else {
        //    _Target = new Vector3(234, 0, -46); // Go home (Debug general logic)
        //    _StoppingDistance = 1f;
        //    _UseSlowDown = true;
        //}

        var distance = (_Target - _Ship.Position).magnitude;
        var direction = (_Target - _Ship.Position).normalized;

        if (distance > _StoppingDistance) {
            ApplyLocalAvoidance(ref direction, ref distance);
            _Ship.SetSteering(CalculateSteering(direction));
            _Ship.SetThrottle(CalculateThrottle(direction, distance));
        }
        else {
            _Evading = false;
            state++;
            //if ((ShipController.LocalShip.Position - _Ship.Position).magnitude < 1.5f) { // Enemy is too close (Debug general logic)
            //    _Ship.SetSteering(CalculateSteering(direction));
            //    _Ship.SetThrottle(-1);
            //}
            //else {
            //    _Ship.SetSteering(CalculateSteering(direction)); // Should be 0 (Debug general logic)
            //    _Ship.SetThrottle(0);
            //}
        }

        return TaskStatus.Running;
    }

    private void ApplyLocalAvoidance(ref Vector3 direction, ref float distance) {
        var lonSpeed = Vector3.Project(_Ship.Velocity, _Ship.transform.forward).magnitude;
        var radius = (lonSpeed * BoundsSensitivity + 1) * _Ship.Width;
        var view = radius + lonSpeed * ViewSensitivity;

        var directionRay = new Ray(_Ship.transform.position, _Ship.Velocity.normalized);
        var directionHit = new RaycastHit();

        if (Physics.SphereCast(directionRay, radius, out directionHit, view, ~LayerMask.GetMask("Projectile"))) {
            var adjustment = (directionRay.GetPoint(directionHit.distance) - directionHit.point).normalized;
            direction = (adjustment * CollisionSensitivity * (1 - directionHit.distance / view) + direction).normalized;
        }

        var distanceRay = new Ray(_Ship.transform.position, _Ship.Velocity.normalized);
        var distanceHit = new RaycastHit();

        if (Physics.Raycast(distanceRay, out directionHit, view, ~LayerMask.GetMask("Projectile"))) {
            distance = distanceHit.distance;
        }

        //var resDir = direction;
        //var dist = directionHit.distance == 0 ? view : directionHit.distance;
        //GizmoDrawer.AddTask(() => Gizmos.color = Color.yellow);
        //GizmoDrawer.AddTask(() => Gizmos.DrawWireSphere(directionRay.origin, radius));
        //GizmoDrawer.AddTask(() => Gizmos.DrawWireSphere(directionRay.GetPoint(dist), radius));
        //GizmoDrawer.AddTask(() => Gizmos.DrawWireCube(directionHit.point, Vector3.one / 5));
        //GizmoDrawer.AddTask(() => Gizmos.color = Color.green);
        //GizmoDrawer.AddTask(() => Gizmos.DrawWireCube(_Target, Vector3.one / 7));
        //GizmoDrawer.AddTask(() => Gizmos.color = Color.Lerp(Color.green, Color.red, 1 - dist / view));
        //GizmoDrawer.AddTask(() => Gizmos.DrawLine(_Ship.transform.position, _Ship.transform.position + resDir * view));
    }

    private float CalculateSteering(Vector3 direction) {
        var side = Mathf.Sign(Vector3.Dot(_Ship.transform.right, direction));
        if (side == 0)
            throw new Exception("Side equals 0, can't calculate steering");
        var steer = side * (1 - Vector3.Dot(direction, _Ship.transform.forward)) * SteerSensitivity;
        return steer;
    }

    private float CalculateThrottle(Vector3 direction, float distance) {
        var stopping = 0f;
        if (_UseSlowDown && distance < StoppingRadius)
            stopping = (1 - Mathf.Clamp(distance, 0, StoppingRadius) / StoppingRadius) * _Ship.Velocity.magnitude * StoppingSensitivity;
        return Vector3.Dot(direction, _Ship.transform.forward) - stopping;
    }
}
