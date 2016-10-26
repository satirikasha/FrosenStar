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

        private const int MaxRadius = 30;
        private const float MaxDownsample = 0.15f;
        // Debug: Remove downsampling
        //private const float MaxDownsample = 1f;

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
            if (radius == 0) {
                Graphics.Blit(source, destination);
                return;
            }
            var downsample = MaxDownsample + (1 - MaxDownsample) * (1 - Intensity) * (1 - Intensity);
            //var rt = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            var rt1 = RenderTexture.GetTemporary((int)(source.width * downsample), (int)(source.height * downsample), 0, source.format);
            var rt2 = RenderTexture.GetTemporary((int)(source.width * downsample), (int)(source.height * downsample), 0, source.format);
            material.SetInt("_Radius", radius);
            material.SetFloatArray("_Kernel", GetKernel(radius));
            Graphics.Blit(source, rt1);
            Graphics.Blit(rt1, rt2, material, 0);
            Graphics.Blit(rt2, rt1, material, 1);
            Graphics.Blit(rt1, destination);
            //RenderTexture.ReleaseTemporary(rt);
            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
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
                result[i] = CalculateKernelElement(i, 0, radius);
                for (int j = 1; j < radius + 1; j++) {
                    result[i] += CalculateKernelElement(i, j, radius);
                }
                sum += result[i] * ((i == 0) ? 1 : 2);
            }
            for (int i = 0; i < radius + 1; i++) {
                result[i] = result[i] / sum;
            }
            //Debug.Log(radius);
            //Debug.Log(String.Join(" ; ", result.Select(_ => _.ToString()).ToArray())); 
            return result;
        }

        private float CalculateKernelElement(int x, int y, int radius) {
            float sigma = (float)radius / 3;
            return 1 / (2 * Mathf.PI * sigma * sigma) * Mathf.Exp(-(x * x + y * y) / (2 * sigma * sigma));
        }
    }
}
