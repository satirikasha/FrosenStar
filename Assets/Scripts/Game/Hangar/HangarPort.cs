using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class HangarPort : MonoBehaviour {

    
    public string HangarName;

    private Coroutine _PortActivation;

    public void OnTriggerEnter(Collider other) {
        if(other.attachedRigidbody.transform == PlayerController.LocalPlayer.transform) {
            _PortActivation = StartCoroutine(ActivatePort());
        }
    }

    public void OnTriggerExit(Collider other) {
        if (other.attachedRigidbody.transform == PlayerController.LocalPlayer.transform) {
            StopCoroutine(_PortActivation);
        }
    }

    private IEnumerator ActivatePort() {
        yield return new WaitForSeconds(3);
        Debug.Log("Port activated");
    }
}
