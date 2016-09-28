using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SelectionHandler : UIBehaviour, ISelectHandler, IDeselectHandler {

    public Image Image;
    public Color Color;
    public Vector3 PositionOffset;
    public float RotationOffset;

    public float Damping;

    private float _SelectedFactor;
    private float _SelectedTarget;
    private Color _SelectedColor;
    private Color _DeselectedColor;

    protected override void Awake() {
        base.Awake();
        if (Image == null)
        Image = this.GetComponent<Image>();
    }

    protected override void Start() {
        base.Start();
        _SelectedFactor = 0;
        _SelectedTarget = EventSystem.current.currentSelectedGameObject == this.gameObject ? 1 : 0;
        if (Image != null) {
            _DeselectedColor = Image.color;
            _SelectedColor = Color;
        }
    }

    void Update() {

        var oldSelectedFactor = _SelectedFactor;
        var newSelectedFactor = Mathf.Lerp(_SelectedFactor, _SelectedTarget, Time.unscaledDeltaTime * Damping);
        var deltaSelectedFactor = newSelectedFactor - oldSelectedFactor;

        _SelectedFactor = newSelectedFactor;

        if (Image != null) {
            Image.color = Color.Lerp(_DeselectedColor, _SelectedColor, _SelectedFactor);
        }
        this.transform.localPosition += Vector3.LerpUnclamped(Vector3.zero, PositionOffset, deltaSelectedFactor);
        this.transform.localRotation = Quaternion.LerpUnclamped(Quaternion.identity, Quaternion.Euler(0, RotationOffset, 0), deltaSelectedFactor) * this.transform.localRotation;
    }

    public void OnSelect(BaseEventData eventData) {
        _SelectedTarget = 1;
    }

    public void OnDeselect(BaseEventData eventData) {
        _SelectedTarget = 0;
    }
}
