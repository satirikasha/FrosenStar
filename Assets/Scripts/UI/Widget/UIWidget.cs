using UnityEngine;
using System.Collections;

public class UIWidget : MonoBehaviour {

    public RectTransform RectTransform {
        get {
            return this.transform as RectTransform;
        }
    }

}
