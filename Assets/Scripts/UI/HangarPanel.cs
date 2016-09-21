using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HangarPanel : MonoBehaviour {

    public PanelStack Stack {
        get {
            return _Stack;
        }
        set {
            if (value != _Stack) {
                _Stack = value;
                OnStackChanged();
            }
        }
    }
    private PanelStack _Stack;

    public float Damping = 3;

    private RectTransform _RectTransform;
    private CanvasGroup _CanvasGroup;
    private Selectable _DefaultSelectable;

    private float _TargetAlpha;
    private float _TargetPivot;

    void Awake() {
        _RectTransform = (RectTransform)this.transform;
        _CanvasGroup = this.GetComponent<CanvasGroup>();
        _DefaultSelectable = this.GetComponentInChildren<Selectable>();
        OnStackChanged();
    }

    void Start() {
        RefreshValues(1);
    }

    void Update() {
        RefreshValues(Time.unscaledDeltaTime * Damping);
    }

    private void RefreshValues(float fraction) {
        _CanvasGroup.alpha = Mathf.Lerp(_CanvasGroup.alpha, _TargetAlpha, fraction);
        _RectTransform.pivot = new Vector2(Mathf.Lerp(_RectTransform.pivot.x, _TargetPivot, fraction), 0.5f);
    }

    private void OnStackChanged() {
        switch (Stack) {
            case PanelStack.Next:
                _TargetAlpha = 0;
                _TargetPivot = -0.5f;
                break;
            case PanelStack.Current:
                if (EventSystem.current.currentSelectedGameObject != _DefaultSelectable.gameObject)
                    EventSystem.current.SetSelectedGameObject(_DefaultSelectable.gameObject);
                _TargetAlpha = 1;
                _TargetPivot = 0.5f;
                break;
            case PanelStack.Previous:
                _TargetAlpha = 0;
                _TargetPivot = 1.5f;
                break;
            default:
                break;
        }
    }
}

public enum PanelStack {
    Next,
    Current,
    Previous
}
