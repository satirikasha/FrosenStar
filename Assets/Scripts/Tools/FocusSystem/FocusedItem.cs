using System.Collections;
using System.Collections.Generic;
using Tools.EQS;
using UI.Markers;
using UnityEngine;

[RequireComponent(typeof(EQSItem))]
public class FocusedItem : FocusMarkerProvider {

    public static FocusedItem CurrentFousedItem { get; set; }

    public static FocusedItem CurrentLookAtItem {
        get {
            return _CurrentLookAtItem;
        }
        set {
            if (_CurrentLookAtItem != value) {
                if (_CurrentLookAtItem != null) {
                    _CurrentLookAtItem.ResetFocus();
                }
                _CurrentLookAtItem = value;
            }
        }
    }
    public static FocusedItem _CurrentLookAtItem;

    public float FocusWeight = 10;

    public float CurrentFocusAmmount { get; private set; }

    public bool Focused {
        get {
            return this == CurrentFousedItem;
        }
    }

    public bool LookAt {
        get {
            return this == CurrentLookAtItem;
        }
    }

    public float NormalizedFocus {
        get {
            return CurrentFocusAmmount / FocusWeight;
        }
    }


    public void ApplyFocus(float ammount) {
        if (CurrentFousedItem != this) {
            CurrentFocusAmmount += ammount;
            if (CurrentFocusAmmount >= FocusWeight) {
                CurrentFousedItem = this;
                CurrentFocusAmmount = FocusWeight;
            }
        }
    }

    private void ResetFocus() {
        CurrentFocusAmmount = 0;
    }
}
