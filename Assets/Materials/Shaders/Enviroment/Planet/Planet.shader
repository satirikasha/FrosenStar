Shader "Enviroment/Planet" {
	Properties {		
		_MainTex ("Height Map (RGB)", 2D) = "gray" {}
		_ColorRamp("Color Ramp", 2D) = "white" {}
        _DataRamp ("Data Ramp", 2D) = "white" {}
        _Specular ("Specular", Range(0, 1)) = 0.5
		_Detail("Detail", 2D) = "white" {}
		_DetailIntensity ("Detail Intensity", Range(0.001, 1)) = 0.25
		_DetailScale("Detail Scale", Range(0.1, 1)) = 0.275
		_DetailBlendSharpness("Blend Sharpness", Range(0.1, 125)) = 1
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
		sampler2D _ColorRamp;
        sampler2D _DataRamp;
		sampler2D _Detail;
		sampler2D _Normal;

        fixed4 _FresnelColor;
        half _Specular;
		half _DetailIntensity;
		half _DetailScale;
		half _DetailBlendSharpness;
        half _NormalIntensity;
        half _NormalFade;
        half _FresnelIntensity;
        half _FresnelPower;

		struct Input {
			float2 uv_MainTex;
            float3 Normal;
			float3 Position;
            float3 WorldNormal;
            float3 viewDir;
		};

        struct SurfaceOutputPlanet {
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 SurfaceNormal;
            fixed3 Emission;
            fixed Specular;
            fixed Gloss;
            fixed Alpha;
        };

        half4 LightingPlanet (SurfaceOutputPlanet s, half3 lightDir, half3 viewDir, half atten) {
            fixed diff = max(0, dot((s.SurfaceNormal), lightDir)/* + pow(s.Height, _NormalFade) * _NormalIntensity*/);
            fixed3 n = normalize (lerp(s.Normal, s.SurfaceNormal, pow(diff, _NormalFade)));
            diff = max (0, dot (n, lightDir));
            
            half3 h = normalize (lightDir + viewDir);
            float nh = max (0, dot (s.Normal, h));
            float spec = pow (nh, s.Specular * 128.0) * s.Gloss;

            fixed4 c;
            c.rgb = (s.Albedo) * _LightColor0.rgb * diff * atten + _LightColor0.rgb * spec;
            c.a = 1;
            return c;
        }

		half4 TriplanarMap(half3 position, half3 normal, sampler2D tex, half scale, half sharpness) {
			position = position / scale;

			half4 xDiff = tex2D(tex, position.zy);
			half4 yDiff = tex2D(tex, position.xz);
			half4 zDiff = tex2D(tex, position.xy);

			half3 blendWeights = pow(abs(normal), sharpness);
			blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z);

			return xDiff * blendWeights.x + yDiff * blendWeights.y + zDiff * blendWeights.z;
		}

        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT (Input, o);
            o.Normal = v.normal;
			o.Position = v.vertex.xyz;
            o.WorldNormal = normalize(mul (unity_ObjectToWorld, v.normal).xyz);
        }

		void surf (Input IN, inout SurfaceOutputPlanet o) {
			fixed4 h = tex2D(_MainTex, IN.uv_MainTex) + (TriplanarMap(IN.Position, IN.Normal, _Detail, _DetailScale, _DetailBlendSharpness) - 0.5) * _DetailIntensity;
            fixed height = saturate(h);
			fixed4 c = tex2D(_ColorRamp, fixed2(height,0));
            fixed4 data = tex2D(_DataRamp, fixed2 (height, 0));
            //fixed3 n = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
            fixed3 n = UnpackNormal (TriplanarMap (IN.Position, IN.Normal, _Normal, _DetailScale, _DetailBlendSharpness));
            n.b /= _NormalIntensity;
            n = normalize(n);

            fixed ocean = height < 0.3;

            o.SurfaceNormal = IN.WorldNormal;
            o.Normal = lerp(n, fixed3(0,0,1), 1 - data.r);
            o.Specular = 0.5;
            o.Gloss = data.b * _Specular;
            o.Alpha = height;
			o.Albedo = c.rgb + lerp(c.rgb, _FresnelColor.rgb, _FresnelColor.a) * (pow(1 - dot (normalize (IN.viewDir), o.SurfaceNormal), _FresnelPower) * _FresnelIntensity);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
