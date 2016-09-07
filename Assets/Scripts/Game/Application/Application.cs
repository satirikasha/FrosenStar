using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ApplicationManager { 

    public static bool GameMode {
        get
        {
            // TODO: Set up this value when Game scene is loaded
            return SceneManager.GetActiveScene().name == "Game";
        }
    }

    public static string[] GetHangerNames() {
        SceneManager.GetAllScenes
    }
}
