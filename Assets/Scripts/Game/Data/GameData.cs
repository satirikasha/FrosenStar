using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using Tools.Serialization;

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

    public static bool SavedDataExists {
        get {
            return File.Exists(DataPath);
        }
    }

    public static string DataPath {
        get {
            return Application.persistentDataPath + "/LastSave.dat";
        }
    }

    public PlayerData PlayerData = new PlayerData();
    public WorldData WorldData = new WorldData();

    public void GatherData() {
        PlayerData.GatherData();
        WorldData.GatherData();
    }

    public void ScatterData() {
        PlayerData.ScatterData();
        WorldData.ScatterData();
    }

    public static void Save() {
        Debug.Log("Save");
        _Current = new GameData();
        _Current.GatherData();

        var bf = new BinaryFormatter();
        bf.SurrogateSelector = SerializationSurrogate.SurrogateSelector;

        using (var fs = File.Open(DataPath, FileMode.OpenOrCreate)) {
            bf.Serialize(fs, _Current);
            fs.Close();
        }
    }

    public static void Load() {
        Debug.Log("Load");
        if (SavedDataExists) {
            Debug.Log("SaveGame exists");

            var bf = new BinaryFormatter();
            bf.SurrogateSelector = SerializationSurrogate.SurrogateSelector;

            using (var fs = File.Open(DataPath, FileMode.Open)) {
                _Current = (GameData)bf.Deserialize(fs);
                fs.Close();
            }
            _Current.ScatterData();
        }
        else {
            _Current = new GameData();
        }
    }
}

public interface IData {
    void GatherData();
    void ScatterData();
}

