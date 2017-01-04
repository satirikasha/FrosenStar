using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEffectsManager : MonoBehaviour {

	void Update () {
        this.transform.position = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
	}
}
