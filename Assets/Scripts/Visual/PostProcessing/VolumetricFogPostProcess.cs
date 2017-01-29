using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
    [ExecuteInEditMode]
    public class VolumetricFogPostProcess : PostProcessBase {

        [Range(0, 1)]
        public float Intensity;

        private const int MaxRadius = 30;
        private const float MaxDownsample = 0.15f;
        // Debug: Remove downsampling
        //private const float MaxDownsample = 1f;

        private Dictionary<int, Kernel> _KernelCache = new Dictionary<int, Kernel>();

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
            var kernel = GetKernel(radius);
            material.SetInt("_Iterations", kernel.Iterations);
            material.SetFloatArray("_Weight", kernel.Weights);
            material.SetFloatArray("_Offset", kernel.Offsets);
            Graphics.Blit(source, rt1);
            Graphics.Blit(rt1, rt2, material, 0);
            Graphics.Blit(rt2, rt1, material, 1);
            Graphics.Blit(rt1, destination);
            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
        }

        private Kernel GetKernel(int radius) {
            if (!_KernelCache.ContainsKey(radius)) {
                _KernelCache.Add(radius, GenerateKernel(radius));
            }
            return _KernelCache[radius];
        }

        private Kernel GenerateKernel(int radius) {
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

            return OptimizeKernel(result, radius);
        }

        private float CalculateKernelElement(int x, int y, int radius) {
            float sigma = (float)radius / 3;
            return 1 / (2 * Mathf.PI * sigma * sigma) * Mathf.Exp(-(x * x + y * y) / (2 * sigma * sigma));
        }

        private Kernel OptimizeKernel(float[] kernel, float radius) {
            var iterations = Mathf.CeilToInt((float)(radius) / 2) + 1;
            var weights = new float[MaxRadius + 1];
            var offsets = new float[MaxRadius + 1];
            var index = 1;
            weights[0] = kernel[0];
            offsets[0] = 0;
            if (radius % 2 != 0) {
                weights[1] = kernel[1];
                offsets[1] = 1;
                index = 2;
            }

            if (radius > 1) {
                var j = index;
                for (int i = index; i < iterations; i ++) {
                    weights[i] = kernel[j] + kernel[j + 1];
                    offsets[i] = (j * kernel[j] + (j + 1) * kernel[j + 1]) / weights[i];
                    j += 2;
                }
            }

            // Debug: info
            //Debug.Log(radius);
            //Debug.Log(iterations);
            //Debug.Log(String.Join(" ; ", kernel.Select(_ => _.ToString()).ToArray()));
            //Debug.Log(String.Join(" ; ", weights.Select(_ => _.ToString()).ToArray()));
            //Debug.Log(String.Join(" ; ", offsets.Select(_ => _.ToString()).ToArray()));

            return new Kernel() { Iterations = iterations, Weights = weights, Offsets = offsets };
        }

        private class Kernel {
            public int Iterations;
            public float[] Weights;
            public float[] Offsets;
        }
    }
}
