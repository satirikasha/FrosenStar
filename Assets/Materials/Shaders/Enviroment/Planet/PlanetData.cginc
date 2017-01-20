
sampler2D _MainTex;
sampler2D _Detail;
sampler2D _Normal;
sampler2D _WaterRamp;

half3 _DesertColor;
half3 _VegetationColor;
half3 _MountainColor;
half3 _SnowColor;
half _DetailIntensity;
half _DetailScale;
half _DetailBlendSharpness;
half _NormalIntensity;
half _WaterLevel;
half _DesertLevel;
half _VegetationLevel;
half _MountainLevel;
half _SnowLevel;

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
	fixed3 Emission;
	fixed Specular;
	fixed Gloss;
	fixed Alpha;
};

struct PlanetData {
	float HeightMap;
	float3 DetailMap;
	float3 ColorMap;
	fixed3 NormalMap;
	fixed NormalIntensity;
	fixed Gloss;
	fixed Specular;
	fixed3 Position;
	fixed3 Albedo;
};

PlanetData GetPlanetData(Input IN) {
	PlanetData p;
	p.HeightMap = tex2D(_MainTex, IN.uv_MainTex);
	p.ColorMap = PolarMap(IN.Position, IN.Normal, _Detail, IN.uv_MainTex, _DetailScale, _DetailBlendSharpness);
	p.DetailMap = (p.ColorMap - 0.5) * _DetailIntensity;
	p.NormalMap = UnpackNormal(PolarMap(IN.Position, IN.Normal, _Normal, IN.uv_MainTex, _DetailScale, _DetailBlendSharpness));
	p.NormalIntensity = _NormalIntensity;
	p.Gloss = 0;
	p.Specular = 1;
	p.Position = IN.Position;
	p.Albedo = p.HeightMap;
	return p;
}
