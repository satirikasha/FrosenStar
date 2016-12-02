

#include "UnityCG.cginc"

sampler2D _Noise;

inline float Noise(float3 position)
{
	float3 p = floor(position);
	float3 f = frac(position);
	f = f*f*(3.0 - 2.0 * f);

	float2 uv = (p.xy + float2(37.0, 17.0)*p.z) + f.xy;
	float2 rg = tex2D(_Noise, (uv + 0.5) / 256.0).yx;
	return lerp(rg.x, rg.y, f.z);
}

inline float Noise(float3 position, int octaves) 
{ 
	float f = 0; 
	for (int i = 0; i < octaves; i++) 
	{ 
		f += Noise(position) / pow(2., i + 1);
		position *= 2.01;
	} 
	f /= 1 - 1 / pow(2., octaves + 1); 
	return f; 
}