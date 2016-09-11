using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameData: IData {

    public static GameData Current {
        get {
            if (_Current == null)
                Load();
            return _Current;
        }
    }
    private static GameData _Current;

    public PlayerData PlayerData { get; set; }
    public WorldData WorldData { get; set; }

    public static void Save() {

    }

    public static void Load() {

    }

    public void RefreshData() {
        PlayerData.RefreshData();
        WorldData.RefreshData();
    }
}

public interface IData {
    void RefreshData();
}

