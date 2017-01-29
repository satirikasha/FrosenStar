using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using System;

public static class ApplicationManager {

    public static bool GameMode {
        get {
            return SceneManager.GetActiveScene().name == "Game";
        }
    }

    public static bool NewGame {
        get {
            return !GameData.SavedDataExists;
        }
    }

    public static void EnterGame() {
        GameData.Save();
        SceneManager.LoadScene("Game");
    }

    public static void EnterHangar(HangarPort port) {
        GameData.Current.PlayerData.ShipData.Position = port.PlayerStart.transform.position;
        GameData.Current.PlayerData.ShipData.Rotation = port.PlayerStart.transform.rotation;
        GameData.Save();
        SceneManager.LoadScene(port.HangarName + "Hangar");
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
