Shader "Enviroment/Atmosphere" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_Power("Power", Range(0,10)) = 5
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200
		Cull Front
		
		CGPROGRAM
		#pragma surface surf Atmosphere alpha:fade 
		#pragma target 3.0

		struct Input {
			float3 viewDir;
			float3 worldNormal;
		};

		fixed4 _Color;
		fixed _Power;

		half4 LightingAtmosphere (SurfaceOutput s, half3 lightDir, half atten) {
			half NdotL = clamp(dot(normalize(s.Normal), normalize(lightDir)), 0, 1);
			half4 c;
			c.rgb = s.Emission * _LightColor0.rgb * (NdotL * atten);
			c.a = s.Alpha;
			return c;
              //half NdotL = clamp(dot(normalize(s.Normal), normalize(lightDir)), 0, 1);
              //half4 c;
              //c.rgb = s.Emission * _LightColor0.rgb * (NdotL * atten);
              //c.a = /*s.Alpha * (NdotL * atten) * 0.75 +*/ s.Alpha;
              //return c;
        }

		half Fresnel (float3 viewDir, float3 worldNormal) {
			return dot(normalize(viewDir), worldNormal);
		}

		void surf (Input IN, inout SurfaceOutput o) {
			o.Alpha = _Color.a * pow(Fresnel(IN.viewDir, -o.Normal), _Power);
			o.Emission = _Color.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
