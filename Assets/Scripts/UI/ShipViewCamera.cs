using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipViewCamera : MonoBehaviour {

    public Transform FocusPoint;
    public RawImage TargetImage;

    private Camera _Camera;

    void Start() {
        _Camera = this.GetComponent<Camera>();
    }

    void Update() {
        if (TargetImage != null) {
            if (_Camera.pixelWidth != (int)TargetImage.rectTransform.rect.width || _Camera.pixelHeight != (int)TargetImage.rectTransform.rect.height) {
                var oldRT = _Camera.targetTexture;
                _Camera.targetTexture = RenderTexture.GetTemporary((int)TargetImage.rectTransform.rect.width, (int)TargetImage.rectTransform.rect.height, 16);
                TargetImage.texture = _Camera.targetTexture;
                RenderTexture.ReleaseTemporary(oldRT);
            }
        }
    }
}
