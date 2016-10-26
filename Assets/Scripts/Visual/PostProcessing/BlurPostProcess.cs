using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    public class BlurPostProcess : PostProcessBase {

        [SerializeField]
        private Shader _MobileShader;
        [Range(0, 1)]
        public float Intensity;

        private const int MaxRadius = 32;

        private Dictionary<int, float[]> _KernelCache = new Dictionary<int, float[]>();

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
            var radius = (int)(MaxRadius * Intensity);
            var rt = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            material.SetInt("_Radius", radius);
            material.SetFloatArray("_Kernel", GetKernel(radius));
            Graphics.Blit(source, rt, material, 0);
            Graphics.Blit(rt, destination, material, 1);
            RenderTexture.ReleaseTemporary(rt);
        }

        private float[] GetKernel(int radius) {
            if (!_KernelCache.ContainsKey(radius)) {
                _KernelCache.Add(radius, GenerateKernel(radius));
            }
            return _KernelCache[radius];
        }

        private float[] GenerateKernel(int radius) {
            var result = new float[MaxRadius + 1];
            var sum = 0.0f;
            for (int i = 0; i < radius + 1; i++) {
                result[i] = CalculateKernelElement(i, 0, radius + 1);
                for (int j = 1; j < radius + 1; j++) {
                    result[i] += CalculateKernelElement(i, j, radius + 1) * 2;
                }
                sum += result[i] * ((i == 0) ? 1 : 2);
            }
            for (int i = 0; i < radius + 1; i++) {
                result[i] = result[i] / sum;
            }
            return result;
        }

        private float CalculateKernelElement(int x, int y, float sigma) {
            return 1/(2 * Mathf.PI * sigma * sigma) * Mathf.Exp(-(x * x + y * y) / (2 * sigma * sigma));
        }
    }
}
