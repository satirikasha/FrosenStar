using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class VisualEffect : MonoBehaviour {

    private static Dictionary<Type, List<VisualEffect>> _ResourcesCache = new Dictionary<Type, List<VisualEffect>>();
    private static Dictionary<Type, List<VisualEffect>> _EffectsCache = new Dictionary<Type, List<VisualEffect>>();

    public static Transform EffectsHost {
        get {
            if (_EffectsHost == null)
                _EffectsHost = new GameObject("Effects").transform;
            return _EffectsHost;
        }
    }
    private static Transform _EffectsHost;

    protected virtual void Awake() {
        var type = this.GetType();
        if (!_EffectsCache.ContainsKey(type)) {
            _EffectsCache.Add(type, new List<VisualEffect>());
        }
        _EffectsCache[type].Add(this);
    }

    protected virtual void OnDestroy() {
        _EffectsCache[this.GetType()].Remove(this);
    }

    public virtual void Play() {
        this.gameObject.SetActive(true);
        StartCoroutine(PlayTask());
    }

    protected virtual IEnumerator PlayTask() {
        yield return null;
    }

    public static T GetEffect<T>(string name = null) where T : VisualEffect {
        T result = null;
        if (_EffectsCache.ContainsKey(typeof(T))) {
            result = (T)_EffectsCache[typeof(T)].FirstOrDefault(_ => !_.gameObject.activeSelf && (string.IsNullOrEmpty(name) || name == _.name));
        }
        if (result == null) {
            result = Instantiate(GetEffectResource<T>(name), EffectsHost, false);
            result.gameObject.SetActive(false);
        }
        return result;
    }

    public static T GetEffectResource<T>(string name = null) where T : VisualEffect {
        if (!_ResourcesCache.ContainsKey(typeof(T))) {
            _ResourcesCache.Add(typeof(T), Resources.LoadAll<T>("Prefabs/Effects").Cast<VisualEffect>().ToList());
        }
        if (string.IsNullOrEmpty(name)) {
            return (T)_ResourcesCache[typeof(T)].FirstOrDefault();
        }
        else {
            return (T)_ResourcesCache[typeof(T)].FirstOrDefault(_ => _.name == name);
        }
    }
}
