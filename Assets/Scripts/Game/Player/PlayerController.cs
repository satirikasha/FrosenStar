using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public static PlayerController LocalPlayer { get; private set; }

	void Awake () {
        LocalPlayer = this;
	}
}
