using UnityEngine;
using System.Collections;
using Tools;
using System.Collections.Generic;
using System;

public class PrefabRegistry : SingletonScriptableObject<PrefabRegistry> {
    [SerializeField]
    private List<RegisteredPrefab> Registry;

#if UNITY_EDITOR
    public void RefreshRegistry() {
        Registry = new List<RegisteredPrefab>();
        //UnityEditor.PrefabUtility.
    }
#endif

    [Serializable]
    private class RegisteredPrefab {
        [ReadOnly]
        public string ID;
        public GameObject Prefab;
    }
}
