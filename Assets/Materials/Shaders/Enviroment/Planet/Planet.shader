Shader "Enviroment/Planet" {
	Properties {		
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Normal ("Normal", 2D) = "blue" {}
        _NormalIntensity ("Normal Intensity", Range(0.001, 1)) = 1
        _NormalFade ("Normal Fade", Range (0.1, 2)) = 1
        _FresnelColor ("Color", Color) = (1,1,1,1)
        _FresnelIntensity ("Fresnel Intensity", Range (0.001, 1)) = 1
        _FresnelPower ("Fresnel Power", Range (0.001, 5)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Planet vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.0

		sampler2D _MainTex;
        sampler2D _Normal;

        fixed4 _FresnelColor;
        half _NormalIntensity;
        half _NormalFade;
        half _FresnelIntensity;
        half _FresnelPower;

		struct Input {
			float2 uv_MainTex;
            float3 Normal;
            float3 WorldNormal;
            float3 viewDir;
		};

        struct SurfaceOutputPlanet {
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 SurfaceNormal;
            fixed3 Emission;
            fixed Alpha;
        };

        half4 LightingPlanet (SurfaceOutputPlanet s, half3 lightDir, half atten) {
            fixed diff = max (0, dot ((s.SurfaceNormal), lightDir));
            fixed3 normal = normalize (lerp(s.Normal, s.SurfaceNormal, pow(diff, _NormalFade)));
            diff = max (0, dot (normal, lightDir));
            
            fixed4 c;
            c.rgb = (s.Albedo) * _LightColor0.rgb * diff * atten;
            c.a = 1;
            return c;
        }

        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT (Input, o);
            o.Normal = v.normal;
            o.WorldNormal = normalize(mul (unity_ObjectToWorld, v.normal).xyz);
        }

		void surf (Input IN, inout SurfaceOutputPlanet o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            fixed3 n = UnpackNormal(tex2D (_Normal, IN.uv_MainTex));
            n.b /= _NormalIntensity;
            n = normalize(n);
            o.SurfaceNormal = IN.WorldNormal;
            o.Normal = n;
			o.Albedo = c.rgb + lerp(c.rgb, _FresnelColor.rgb, _FresnelColor.a) * (pow(1 - dot (normalize (IN.viewDir), o.Normal), _FresnelPower) * _FresnelIntensity);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
