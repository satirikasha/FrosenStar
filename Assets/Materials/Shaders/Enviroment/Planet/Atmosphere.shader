Shader "Enviroment/Planet/Atmosphere" {
	Properties {		
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", color) = (1,1,1,1)
        _FresnelIntensity ("Fresnel Intensity", Range (0.001, 5)) = 1
        _ScatteringIntensity ("Scattering Intensity", Range (0.001, 10)) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" "RenderQueue"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Atmosphere alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

        fixed4 _Color;
        half _FresnelIntensity;
        half _ScatteringIntensity;

		struct Input {
			float2 uv_MainTex;
            float3 viewDir;
		};

		half4 LightingAtmosphere(SurfaceOutput s, half3 lightDir, half atten) {
			fixed diff = max(0, dot((s.Normal), lightDir));

			fixed4 c;
			c.rgb = (s.Albedo) * _LightColor0.rgb * diff * atten;
			c.a = diff * atten;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) {
            float fresnel = pow(1 - dot (normalize (IN.viewDir), o.Normal), _FresnelIntensity);
            float scattering = tex2D (_MainTex, fresnel.xx) * _ScatteringIntensity;
			o.Albedo = scattering * _Color.rgb;
            o.Alpha = scattering * _Color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
