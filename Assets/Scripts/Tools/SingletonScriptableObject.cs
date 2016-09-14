using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tools {


    public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject {

        public static T Instance {
            get {
                if (_Instance == null)
                    Load();
                return _Instance;
            }
        }
        private static T _Instance = null;

        private static string Path {
            get {
#if UNITY_EDITOR
                return "Assets/Resources/Config/" + typeof(T).Name + ".asset";
#else
                return "Config/" + typeof(T).Name;
#endif
            }
        }

        private static void Load() {
#if UNITY_EDITOR
            var asset = AssetDatabase.LoadAssetAtPath(Path, typeof(ScriptableObject)) as T;
            if (asset == null) {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, Path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            _Instance = asset;
#else
            _Instance = Resources.Load<T>(Path);
#endif
        }

#if UNITY_EDITOR
        public void Save() {
            EditorUtility.SetDirty(Instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}
