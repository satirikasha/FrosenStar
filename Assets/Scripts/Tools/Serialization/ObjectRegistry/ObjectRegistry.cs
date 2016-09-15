using UnityEngine;
using System.Collections;
using Tools;
using System.Collections.Generic;
using System;
using System.Linq;
using Object = UnityEngine.Object;

public class ObjectRegistry : SingletonScriptableObject<ObjectRegistry> {
    [SerializeField]
    private List<RegisteredObject> PrefabRegistry;
    [SerializeField]
    private List<RegisteredObject> TextureRegistry;

    public static GameObject GetPrefab(int id) {
        return Instance.PrefabRegistry.FirstOrDefault(_ => _.ID == id).Object as GameObject;
    }

    public static int GetPrefabID(GameObject prefab) {
        //foreach (var item in Instance.PrefabRegistry) {
        //    Debug.Log(item.Object.name);
        //}
        return Instance.PrefabRegistry.First(_ => _.Object == prefab).ID;
    }

    public static Texture GetTexture(int id) {
        return Instance.TextureRegistry.FirstOrDefault(_ => _.ID == id).Object as Texture;
    }

    public static int GetTextureID(Texture texture) {
        return Instance.TextureRegistry.First(_ => _.Object == texture).ID;
    }

#if UNITY_EDITOR
    public void RefreshRegistry() {
        var paths = UnityEditor.AssetDatabase.GetAllAssetPaths();
        foreach (var path in paths) {
            if (path.Contains("Prefabs/Ship") && path.EndsWith(".prefab")) {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (PrefabRegistry.Any(_ => _.Object == asset))
                    continue;
              
                PrefabRegistry.Add(new RegisteredObject() { ID = GenerateID(path), Object = asset });
                continue;
            }

            if(path.Contains("Textures/Inventory/Icons") && (path.EndsWith(".png") || path.EndsWith(".jpg"))) {
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture>(path);
                if (TextureRegistry.Any(_ => _.Object == asset))
                    continue;

                TextureRegistry.Add(new RegisteredObject() { ID = GenerateID(path), Object = asset });
                continue;
            }
        }
        Save();
    }

    public void CleanUpRegistry() {
        PrefabRegistry.RemoveAll(_ => _.Object == null);
        TextureRegistry.RemoveAll(_ => _.Object == null);
    }

    private int GenerateID(string path) {
        return UnityEditor.AssetDatabase.AssetPathToGUID(path).GetHashCode();
    }
#endif

    [Serializable]
    private class RegisteredObject {
        [ReadOnly]
        public int ID;
        public Object Object;
    }
}
