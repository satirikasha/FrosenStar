// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Raymarching/Test"
{
	Properties
	{
		// How many iterations we should step through space
		_Iterations("Iterations", Range(0, 200)) = 100
		// How long through space we should step
		_ViewDistance("View Distance", Range(0, 25)) = 2
		// Essentially the background color
		_SkyColor("Sky Color", Color) = (0.176, 0.478, 0.871, 0)
		// Cloud color
		_CloudColor("Cloud Color", Color) = (1, 1, 1, 1)
		// How dense our clouds should be
		_CloudDensity("Cloud Density", Range(0, 1)) = 0.5

		// Note that the longer your view distance is, the more steps are required. And the smaller your clouds are, the bigger a render target is needed.
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite Off
		ZTest Off
		Blend SrcAlpha One

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Assets/Materials/Shaders/Noise/Noise.cginc"

			int _Iterations;
			float3 _SkyColor;
			float4 _CloudColor;
			float _ViewDistance;
			float _CloudDensity;

			float4 _CameraPos;
			float4 _CameraForward;
			float4 _CameraRight;
			float4 _CameraUp;
			float _AspectRatio;
			float _FieldOfView;

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
			};

			float distFunc(float3 pos)
			{
				const float sphereRadius = 1;
				return length(pos) - sphereRadius;
			}

			fixed4 renderSurface(float3 pos)
			{
				const float2 eps = float2(0.0, 0.01);

				float ambientIntensity = 0.1;
				float3 lightDir = float3(0, -0.5, 0.5);

				float3 normal = normalize(float3(
					distFunc(pos + eps.yxx) - distFunc(pos - eps.yxx),
					distFunc(pos + eps.xyx) - distFunc(pos - eps.xyx),
					distFunc(pos + eps.xxy) - distFunc(pos - eps.xxy)));

				float diffuse = ambientIntensity + max(dot(-lightDir, normal), 0);

				return fixed4(diffuse, diffuse, diffuse, 1);
			}

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 pos = i.worldPos.xyz;
				float3 ray = normalize(pos - _WorldSpaceCameraPos);

				float3 p = pos;

				float density = 0;

				for (float i = 0; i < _Iterations; i++)
				{
					// f gives a number between 0 and 1.
					// We use that to fade our clouds in and out depending on how far and close from our camera we are.
					float f = i / _Iterations;
					// And here we do just that:
					float alpha = smoothstep(0, _Iterations * 0.2, i) * (1 - f) * (1 - f);
					// Note that smoothstep here doesn't do the same as Mathf.SmoothStep() in Unity C# - which is frustrating btw. Get a grip Unity!
					// Smoothstep in shader languages interpolates between two values, given t, and returns a value between 0 and 1. 
					// To get a bit of variety in our clouds we collect two different samples for each iteration.
					float denseClouds = smoothstep(_CloudDensity, 0.75, Noise(p, 5));
					float lightClouds = (smoothstep(-0.2, 1.2, Noise(p * 2, 2)) - 0.5) * 0.5;
					// Note that I smoothstep again to tell which range of the noise we should consider clouds.
					// Here we add our result to our density variable
					density += (lightClouds + denseClouds) * alpha;
					// And then we move one step further away from the camera.
					p = pos + ray * f * _ViewDistance;
				}
				// And here i just melted all our variables together with random numbers until I had something that looked good.
				// You can try playing around with them too.
				float3 color = _SkyColor + (_CloudColor.rgb - 0.5) * (density / _Iterations) * 20 * _CloudColor.a;
				density = clamp(density, 0, 1);

				return fixed4(color, density);
			}
			ENDCG
		}
	}
}