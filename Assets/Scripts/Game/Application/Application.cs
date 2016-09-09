using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using System;

public class ApplicationManager {

    public static bool GameMode {
        get {
            // TODO: Set up this value when Game scene is loaded
            return SceneManager.GetActiveScene().name == "Game";
        }
    }

    public static void EnterHangar(string name) {
        SceneManager.LoadScene(name + "Hangar");
    }

#if UNITY_EDITOR
    public static List<string> GetHangarNames() {
        var names = new List<string>();
        for (int i = 0; i < UnityEditor.EditorBuildSettings.scenes.Length; i++) {
            if (UnityEditor.EditorBuildSettings.scenes[i].path.EndsWith("Hangar.unity")) {
                var name = UnityEditor.EditorBuildSettings.scenes[i].path.Split('/').Last();
                names.Add(name.Replace("Hangar.unity", ""));
            }
        }
        return names;
    }
#endif
}
