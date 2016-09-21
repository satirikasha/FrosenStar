using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(RectTransform))]
public class SelectionHandler : UIBehaviour, ISelectHandler, IDeselectHandler {

    public Vector3 PositionOffset;
    public float RotationOffset;

    public float Damping;

    private float _SelectedFactor;
    private float _SelectedTarget;
    private Vector3 _SelectedPosition;
    private Vector3 _DeselectedPosition;
    private Quaternion _SelectedRotation;
    private Quaternion _DeselectedRotation;

    protected override void Start() {
        base.Start();
        _SelectedFactor = 0;
        _SelectedTarget = 0;
        _DeselectedPosition = this.transform.localPosition;
        _DeselectedRotation = this.transform.localRotation;
        _SelectedPosition = _DeselectedPosition + PositionOffset;
        _SelectedRotation = Quaternion.Euler(0, RotationOffset, 0) * _DeselectedRotation;
    }

    void Update() {
        _SelectedFactor = Mathf.Lerp(_SelectedFactor, _SelectedTarget, Time.unscaledDeltaTime * Damping);
  
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
