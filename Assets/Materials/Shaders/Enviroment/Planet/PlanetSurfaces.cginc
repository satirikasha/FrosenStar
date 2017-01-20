

void ApplySnow(inout PlanetData p) {
	float3 snowColor = lerp(1, (p.ColorMap.r + p.ColorMap.b) / 2, 0.5)  * _SnowColor.rgb;
	float height = saturate(1 - p.HeightMap + p.DetailMap.r);
	float mask = saturate((PoleMask(p.Position, _SnowLevel, 5) - height) * 5);
	p.NormalIntensity = lerp(p.NormalIntensity, 0.5, mask);
	p.Gloss = lerp(p.Gloss, 1, mask);
	p.Specular = lerp(p.Specular, 1, mask);
	p.Albedo = lerp(p.Albedo, snowColor, mask);
}

void ApplyWater(inout PlanetData p) {
	float height = saturate(p.HeightMap + p.DetailMap.r);
	float gradient = saturate(_WaterLevel - height);
	float mask = saturate(gradient * 10);
	p.NormalIntensity = lerp(p.NormalIntensity, 0, mask);
	p.Gloss = lerp(p.Gloss, 1 + (p.ColorMap.g), mask);
	p.Specular = lerp(p.Specular, 0.5, mask);
	p.Albedo = lerp(p.Albedo, tex2D(_WaterRamp, float2(1 - gradient,0)), mask);
}

void ApplyDesert(inout PlanetData p) {
	float3 desColor = lerp(1, p.ColorMap.b, 0.75)  * _DesertColor.rgb;
	float height = saturate(p.HeightMap + p.DetailMap.b * p.DetailMap.r);
	float mask = saturate((_DesertLevel - height) * 5);
	p.NormalIntensity = lerp(p.NormalIntensity, 0.1, mask);
	p.Gloss = lerp(p.Gloss, 0.3, mask);
	p.Specular = lerp(p.Specular, 0.5, mask);
	p.Albedo = lerp(p.Albedo, desColor, mask);
}

void ApplyVegetation(inout PlanetData p) {
	float3 vegColor = lerp(1, p.ColorMap.g, 0.75)  * _VegetationColor.rgb;
	float height = saturate(p.HeightMap + p.DetailMap.r);
	float mask = saturate((_VegetationLevel - height) * 10);
	p.NormalIntensity = lerp(p.NormalIntensity, 0.4, mask);
	p.Gloss = lerp(p.Gloss, 0.2, mask);
	p.Specular = lerp(p.Specular, 0.5, mask);
	p.Albedo = lerp(p.Albedo, vegColor, mask);
}

void ApplyMountain(inout PlanetData p) {
	float3 mountColor = lerp(1, p.ColorMap.r, 1)  * _MountainColor.rgb;
	float height = saturate(p.HeightMap + p.DetailMap.r);
	float mask = saturate((_MountainLevel - height) * 15);
	p.NormalIntensity = lerp(p.NormalIntensity, 1, mask);
	p.Gloss = lerp(p.Gloss, 0.2, mask);
	p.Specular = lerp(p.Specular, 0.7, mask);
	p.Albedo = lerp(p.Albedo, mountColor, mask);
}
