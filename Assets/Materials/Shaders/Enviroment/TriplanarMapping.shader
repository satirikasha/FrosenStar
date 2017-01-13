Shader "Debug/TriplanarMapping"
{
	Properties
	{
		_DiffuseMap("Diffuse Map ", 2D) = "white" {}
		_TextureScale("Texture Scale", Range(0.1, 1)) = 1
		_TriplanarBlendSharpness("Blend Sharpness",float) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert vertex:vert

		sampler2D _DiffuseMap;
		float _TextureScale;
		float _TriplanarBlendSharpness;

		struct Input
		{
			float3 localPos;
			float3 localNormal;
		};

		half4 TriplanarMap(half3 position, half3 normal, sampler2D tex, half scale, half sharpness) {
			position = position / scale;

			half4 xDiff = tex2D(tex, position.zy);
			half4 yDiff = tex2D(tex, position.xz);
			half4 zDiff = tex2D(tex, position.xy);

			half3 blendWeights = pow(abs(normal), sharpness);
			blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z);

			return xDiff * blendWeights.x + yDiff * blendWeights.y + zDiff * blendWeights.z;
		}

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.localPos = v.vertex.xyz;
			o.localNormal = v.normal.xyz;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = TriplanarMap(IN.localPos, IN.localNormal, _DiffuseMap, _TextureScale, _TriplanarBlendSharpness);
		}
		ENDCG
	}
}