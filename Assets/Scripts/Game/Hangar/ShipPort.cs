using UnityEngine;
using System.Collections;
using Tools;

public class ShipPort : SingletonBehaviour<ShipPort> {

    public float Damping = 0.75f;

    public ItemSlot FocusedSlot { get; private set; }
    public bool SelectingEnabled { get; private set; }

    private Quaternion _TargetRotation;
	
    void Start() {
    }

	void Update () {
        if (FocusedSlot != null) {
            ShipController.LocalShip.Rigidbody.rotation = Quaternion.Lerp(ShipController.LocalShip.Rigidbody.rotation, _TargetRotation, Damping * Time.deltaTime);
        }
    }

    public void FocusSlot(ItemSlot slot) {
        FocusedSlot = slot;
        if (slot != null) {
            var cameraDir = Vector3.ProjectOnPlane(Camera.main.transform.position - ShipController.LocalShip.transform.position, Vector3.up).normalized;
            var slotDir = Vector3.ProjectOnPlane(FocusedSlot.transform.localPosition, Vector3.up).normalized;
            _TargetRotation = Quaternion.LookRotation(cameraDir) * Quaternion.Inverse(Quaternion.LookRotation(slotDir));
        }
    }

    public void StartSelecting() {
        SelectingEnabled = true;
    }

    public void StopSelecting() {
        SelectingEnabled = false;
    }
}
