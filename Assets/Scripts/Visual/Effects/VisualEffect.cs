using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class VisualEffect : MonoBehaviour {

    private static List<VisualEffect> _ExistingResources;
    private static Dictionary<string, VisualEffect> _ResourcesCache = new Dictionary<string, VisualEffect>();
    private static Dictionary<string, List<VisualEffect>> _EffectsCache = new Dictionary<string, List<VisualEffect>>();

    public static Transform EffectsHost {
        get {
            if (_EffectsHost == null)
                _EffectsHost = new GameObject("Effects").transform;
            return _EffectsHost;
        }
    }
    private static Transform _EffectsHost;

    private void Register() {
        if (!_EffectsCache.ContainsKey(this.name)) {
            _EffectsCache.Add(this.name, new List<VisualEffect>());
        }
        _EffectsCache[this.name].Add(this);
    }

    protected virtual void OnDestroy() {
        _EffectsCache[this.name].Remove(this);
    }

    public virtual void Play() {
        this.gameObject.SetActive(true);
        StartCoroutine(PlayTask());
    }

    protected virtual IEnumerator PlayTask() {
        yield return null;
    }

    public static T GetEffect<T>(string name) where T : VisualEffect {
        T result = null;
        if (_EffectsCache.ContainsKey(name)) {
            result = _EffectsCache[name].FirstOrDefault(_ => !_.gameObject.activeSelf) as T;
        }
        if (result == null) {
            result = Instantiate(GetEffectResource<T>(name), EffectsHost, false);
            result.name = name;
            result.gameObject.SetActive(false);
            result.Register();
        }
        return result;
    }

    private static VisualEffect GetEffectResource(string name){
        if(_ExistingResources == null) {
            _ExistingResources = Resources.LoadAll<VisualEffect>("Prefabs/Effects").ToList();
        }
        if (!_ResourcesCache.ContainsKey(name)) {
            _ResourcesCache.Add(name, _ExistingResources.FirstOrDefault(_ => _.name == name));
        }
        return _ResourcesCache[name];
    }

    private static T GetEffectResource<T>(string name) where T : VisualEffect {
        return GetEffectResource(name) as T;
    }
}
