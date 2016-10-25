using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    public class BlurPostProcess : PostProcessBase {

        [SerializeField]
        private Shader _MobileShader;
        [Range(0.1f, 1)]
        public float Intensity;

        public override Shader Shader {
            get {
#if UNITY_IOS || UNITY_ANDROID
                return _MobileShader;
#else
                return _Shader;
#endif
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            var downsample = Intensity * Intensity;
            var rt = RenderTexture.GetTemporary((int)(source.width * downsample), (int)(source.height * downsample), 0, source.format);
            rt.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, rt);
            Graphics.Blit(rt, destination);
            RenderTexture.ReleaseTemporary(rt);
            //Graphics.Blit(source, destination, material);
        }
    }
}
