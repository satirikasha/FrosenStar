using UnityEngine;
using System.Collections;

public class GameManager : SingletonBehaviour<GameManager> {

    public bool Paused { get; private set; }

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp(KeyCode.Escape)) {

        }
	}

    public static void TogglePause() {

    }

    public static void Pause() {

    }

    public static void Resume() {

    }
}
