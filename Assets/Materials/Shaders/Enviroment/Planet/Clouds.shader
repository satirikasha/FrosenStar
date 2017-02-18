Shader "Enviroment/Planet/Clouds" {
	Properties {
		_PoleClouds ("Pole Clouds", 2D) = "black" {}
		_WrapClouds ("Wrap Clouds", 2D) = "black" {}
        _SunsetMap ("Sunset Map", 2D) = "black" {}
        _CloudsColor ("Clouds Color", color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "RenderQueue"="Transparent" }
        ZTest Off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Clouds alpha:fade vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		#include "PlanetTools.cginc"

        sampler2D _PoleClouds;
		sampler2D _WrapClouds;
        sampler2D _SunsetMap;

        half4 _CloudsColor;

		struct Input {
            float2 uv_WrapClouds;
            float3 viewDir;
			float3 Position;
			float3 Normal;
		};

		half4 LightingClouds(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			fixed diff = clamp(dot((s.Normal), lightDir) / (1 - s.Alpha), 0, 1.25);

            fixed3 sunset = tex2D(_SunsetMap, ScatterUV(s.Normal, lightDir, viewDir));

			fixed4 c;
			c.rgb = (s.Albedo) * sunset * _LightColor0.rgb * diff * atten;
			c.a = s.Alpha;
			return c;
		}

        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT (Input, o);
            o.Position = v.vertex.xyz;
            o.Normal = v.normal;
        }

		void surf (Input IN, inout SurfaceOutput o) {

            float cloudMap = PolarMap(IN.Position, IN.Normal, _PoleClouds, _WrapClouds, IN.uv_WrapClouds, 1, 15);

			o.Albedo = cloudMap * _CloudsColor.rgb;
            o.Alpha = cloudMap * _CloudsColor.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
