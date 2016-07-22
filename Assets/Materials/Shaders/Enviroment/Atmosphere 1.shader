Shader "Example/Diffuse Texture" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200
		Cull Front

		CGPROGRAM
#pragma surface surf SimpleLambert alpha:fade

		half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten) {
		half NdotL = dot(s.Normal, lightDir);
		half4 c;
		c.rgb = s.Emission * _LightColor0.rgb * (NdotL * atten);
		c.a = s.Alpha * (NdotL * atten) + s.Alpha;
		return c;
	}

	struct Input {
		float3 viewDir;
		float3 worldNormal;
	};

	fixed4 _Color;

	half Fresnel(float3 viewDir, float3 worldNormal) {
		return dot(normalize(viewDir), worldNormal);
	}

	void surf(Input IN, inout SurfaceOutput o) {
		o.Alpha = _Color.a * pow(Fresnel(IN.viewDir, -o.Normal), 5);
		o.Emission = _Color.rgb;
	}
	ENDCG
	}
		Fallback "Diffuse"
}