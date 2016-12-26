using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisualEffect : MonoBehaviour {

    private static Dictionary<Type, VisualEffect> _ResourcesCache = new Dictionary<Type, VisualEffect>();

    public static T GetEffectResource<T>() where T : VisualEffect {
        if (!_ResourcesCache.ContainsKey(typeof(T))) {
            _ResourcesCache.Add(typeof(T), Resources.LoadAll<T>("Prefabs/Effects")[0]);
        }
        return (T)_ResourcesCache[typeof(T)];
    }
}
