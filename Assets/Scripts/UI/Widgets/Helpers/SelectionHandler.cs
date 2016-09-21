using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SelectionHandler : UIBehaviour, ISelectHandler, IDeselectHandler {

    public Color Color;
    public Vector3 PositionOffset;
    public float RotationOffset;

    public float Damping;

    private float _SelectedFactor;
    private float _SelectedTarget;
    private Color _SelectedColor;
    private Color _DeselectedColor;
    private Vector3 _SelectedPosition;
    private Vector3 _DeselectedPosition;
    private Quaternion _SelectedRotation;
    private Quaternion _DeselectedRotation;

    private Image _Image;

    protected override void Awake() {
        base.Awake();
        _Image = this.GetComponent<Image>();
    }

    protected override void Start() {
        base.Start();
        _SelectedFactor = 0;
        _SelectedTarget = EventSystem.current.currentSelectedGameObject == this.gameObject ? 1 : 0;
        _DeselectedColor = _Image.color;
        _DeselectedPosition = this.transform.localPosition;
        _DeselectedRotation = this.transform.localRotation;
        _SelectedColor = Color;
        _SelectedPosition = _DeselectedPosition + PositionOffset;
        _SelectedRotation = Quaternion.Euler(0, RotationOffset, 0) * _DeselectedRotation;
    }

    void Update() {
        _SelectedFactor = Mathf.Lerp(_SelectedFactor, _SelectedTarget, Time.unscaledDeltaTime * Damping);

        _Image.color = Color.Lerp(_DeselectedColor, _SelectedColor, _SelectedFactor);
        this.transform.localPosition = Vector3.Lerp(_DeselectedPosition, _SelectedPosition, _SelectedFactor);
        this.transform.localRotation = Quaternion.Lerp(_DeselectedRotation, _SelectedRotation, _SelectedFactor);
    }

    public void OnSelect(BaseEventData eventData) {
        _SelectedTarget = 1;
    }

    public void OnDeselect(BaseEventData eventData) {
        _SelectedTarget = 0;
    }
}
