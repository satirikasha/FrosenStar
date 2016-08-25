using UnityEngine;
using System.Collections;

public class Worldmap : MonoBehaviour {
	void Update() {
		transform.Rotate(Vector3.up*3 * Time.deltaTime);
	//	transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
	}
}