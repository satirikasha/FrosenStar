using UnityEngine;
using System.Collections;

public class ShipPort : MonoBehaviour {

    [Range(1,100)]
    public float Sensitivity = 25;
    public float Damping = 1;
    public float MaxSpeed = 100; 

    private float _Velocity;
    private Vector3 _PreviousMousePosition;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        _Velocity = Mathf.Lerp(_Velocity, 0, Damping * Time.deltaTime);

        if (Input.GetMouseButton(0)) {
            _Velocity = -(Input.mousePosition - _PreviousMousePosition).x * Sensitivity;
        }

        _Velocity = Mathf.Clamp(_Velocity, -MaxSpeed, MaxSpeed);

        PlayerController.LocalPlayer.Ship.Rigidbody.rotation *= Quaternion.Euler(Vector3.up * _Velocity * Time.deltaTime);

        _PreviousMousePosition = Input.mousePosition;
    }
}
