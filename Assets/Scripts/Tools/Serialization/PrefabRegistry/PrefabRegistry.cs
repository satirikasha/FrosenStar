using UnityEngine;
using System.Collections;
using Tools;
using System.Collections.Generic;
using System;
using System.Linq;

public class PrefabRegistry : SingletonScriptableObject<PrefabRegistry> {
    [SerializeField]
    private List<RegisteredPrefab> Registry;

    public static GameObject GetPrefab(int id) {
        return Instance.Registry.FirstOrDefault(_ => _.ID == id).Prefab;
    }

    public static int GetPrefabID(GameObject prefab) {
        //Instance.Registry.ForEach(_ => Debug.Log(_.ID));
        return Instance.Registry.First(_ => _.Prefab == prefab).ID;
    }

#if UNITY_EDITOR
    public void RefreshRegistry() {
        var paths = UnityEditor.AssetDatabase.GetAllAssetPaths();
        foreach (var path in paths) {
            if (path.Contains("Prefabs/Ship") && path.EndsWith(".prefab")) {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (Registry.Any(_ => _.Prefab == asset))
                    continue;

                var id = UnityEditor.AssetDatabase.AssetPathToGUID(path).GetHashCode();
                Registry.Add(new RegisteredPrefab() { ID = id, Prefab = asset });
            }
        }
        Save();
    }
#endif

    [Serializable]
    private class RegisteredPrefab {
        [ReadOnly]
        public int ID;
        public GameObject Prefab;
    }
}
