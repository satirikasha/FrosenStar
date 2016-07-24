Shader "Enviroment/Nebula (old)" {
	Properties{
		_DetailMask("Detail Mask", 2D) = "white" {}
		_Stars("Stars", 2D) = "white" {}
		_NebulaBase("Nebula Base", 2D) = "white" {}
		_NebulaBaseSize("Nebula Base Size", Range(1, 500)) = 50
		_NebulaBaseDepth("Nebula Base Depth", Range(0, 1)) = 1
		_NebulaDetail("Nebula Detail", 2D) = "white" {}
		_ColorMix ("Color Mix", 2D) = "white"
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 500

		CGPROGRAM
#pragma surface surf Lambert
#pragma target 3.0

		sampler2D _DetailMask;
		sampler2D _Stars;
		sampler2D _NebulaBase;
		sampler2D _NebulaDetail;
		sampler2D _ColorMix;

		float _NebulaBaseSize;
		float _NebulaBaseDepth;

		struct Input {
			float3 viewDir;
			float3 worldPos;
		};

		float2 DepthParallaxOffset(float depth, float3 viewDir) {
			return -viewDir.xz * depth;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			half3 nebulaBase = tex2D(_NebulaBase, IN.worldPos.xz / _NebulaBaseSize + DepthParallaxOffset(_NebulaBaseDepth, IN.viewDir)).rgb;

			o.Emission = nebulaBase;
	}
	ENDCG
	}
		FallBack "Legacy Shaders/Self-Illumin/Bumped Diffuse"
		CustomEditor "LegacyIlluminShaderGUI"

}
