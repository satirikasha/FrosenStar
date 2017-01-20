Shader "Enviroment/Planet/Planet" {
	Properties {		
		_MainTex ("Height Map (RGB)", 2D) = "gray" {}
        _Specular ("Specular Size", Range(0.1, 2)) = 0.5
		_Gloss("Specular Intensity", Range(0, 1)) = 0.5
		_Detail("Detail", 2D) = "white" {}
		_DetailIntensity ("Detail Intensity", Range(0.001, 1)) = 0.25
		_DetailScale("Detail Scale", Range(1, 10)) = 1
		_DetailBlendSharpness("Blend Sharpness", Range(0.1, 125)) = 1
        _Normal ("Normal", 2D) = "blue" {}
        _NormalIntensity ("Normal Intensity", Range(0, 1)) = 1
        _NormalFade ("Normal Fade", Range (0.1, 2)) = 1
		_WaterRamp("Color Ramp", 2D) = "white" {}
		_WaterLevel("Water Level", Range(0, 1)) = 0.5
		_DesertLevel("Desert Level", Range(0, 1)) = 0.6
		_DesertColor("Desert Color", Color) = (1,1,0,1)
		_VegetationLevel("Vegetation Level", Range(0, 1)) = 0.7
		_VegetationColor("Vegetation Color", Color) = (0,1,0,1)
		_MountainLevel("Mountain Level", Range(0, 1)) = 0.7
		_MountainColor("Mountain Color", Color) = (0.5,0.5,0.5,1)
		_SnowLevel("Snow Level", Range(0, 1)) = 0.2
		_SnowColor("Snow Color", Color) = (1,1,1,1)
		_AlbedoMultiplier("Albedo Multiplier",  Range(0, 3)) = 1
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _FresnelIntensity ("Fresnel Intensity", Range (0, 1)) = 1
        _FresnelPower ("Fresnel Power", Range (0.001, 5)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Planet vertex:vert

		#pragma target 5.0
		#include "PlanetTools.cginc"
		#include "PlanetData.cginc"
		#include "PlanetSurfaces.cginc"
		#include "PlanetLighting.cginc"

        fixed4 _FresnelColor;
        half _Specular;
		half _Gloss;
        half _NormalFade;
        half _FresnelIntensity;
        half _FresnelPower;
		half _AlbedoMultiplier;

        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT (Input, o);
            o.Normal = v.normal;
			o.Position = v.vertex.xyz;
        }

		void surf (Input IN, inout SurfaceOutputPlanet o) {
			PlanetData p = GetPlanetData(IN);

			
			ApplyMountain(p);
			ApplyVegetation(p);
			ApplyDesert(p);
			ApplyWater(p);
			ApplySnow(p);

			o.Normal = lerp(fixed3(0, 0, 1), p.NormalMap, p.NormalIntensity);
			o.Specular = p.Specular * _Specular;
			o.Gloss = p.Gloss * _Gloss;
			o.Albedo = p.Albedo * _AlbedoMultiplier + lerp(p.Albedo, _FresnelColor.rgb, _FresnelColor.a) * (pow(1 - dot (normalize (IN.viewDir), o.Normal), _FresnelPower) * _FresnelIntensity);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
