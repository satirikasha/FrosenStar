Shader "Enviroment/Planet/Clouds" {
	Properties {		
		_PoleClouds ("Pole Clouds", 2D) = "white" {}
		_WrapClouds("Wrap Clouds", 2D) = "white" {}
        _CloudsColor ("Clouds Color", color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "RenderQueue"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Clouds alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		#include "PlanetTools.cginc"

		sampler2D _PoleClouds;
		sampler2D _WrapClouds;

        fixed4 _Color;
        half _FresnelIntensity;
        half _ScatteringIntensity;

		struct Input {
			float2 uv_WrapClouds;
            float3 viewDir;
			float3 Position;
			float3 Normal;
		};

		half4 LightingClouds(SurfaceOutput s, half3 lightDir, half atten) {
			fixed diff = max(0, dot((s.Normal), lightDir));

			fixed4 c;
			c.rgb = (s.Albedo)/* * _LightColor0.rgb * diff * atten*/;
			c.a = 1;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			//float2 shadowUVBelly = p.LightDirTS.xy * 0.005;
			//float2 shadowUVPole = p.LightDir.xy * 0.005;

			//
			//float2 poleUV = RotateUV(p.UV.zw, _Time, - _CloudsAnimation);
			//float2 wrapUV = p.UV.xy;
			//bellyUV.x += _CloudsAnimation * 0.15 * _Time;

			//float cloudBelly = tex2D(_CloudsMap, bellyUV * float2(2, 1)).r;
			//float cloudPole = tex2D(_CloudsCapMap, poleUV).r;
			//float cloudMix = lerp(cloudPole, cloudBelly, p.GradientMap + 0.1);

			//float cloudBellyShadow = tex2D(_CloudsMap, (bellyUV + shadowUVBelly) * float2(2, 1)).r;
			//float cloudPoleShadow = tex2D(_CloudsCapMap, poleUV + shadowUVPole).r;
			//float cloudMixShadow = lerp(cloudPoleShadow, cloudBellyShadow, p.GradientMap + 0.1);

			//cloudMixShadow = saturate(pow(1 - cloudMixShadow, _CloudsShadows));

			//color *= cloudMixShadow;
			//color += max(p.shadow, 0.005) * saturate(cloudMix * (0.002 + pow(p.SunsetMap, _CloudsSunset)) * _CloudsBrightness * _CloudsColor.rgb);

			//return color;

			float clouds = PolarMap(IN.Position, IN.Normal, _PoleClouds, _WrapClouds, IN.uv_WrapClouds, 10, 13).r;

			o.Albedo = clouds;
            o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
