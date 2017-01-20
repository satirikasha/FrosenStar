

half4 LightingPlanet(SurfaceOutputPlanet s, half3 lightDir, half3 viewDir, half atten) {
	fixed diff = max(0, dot((s.Normal), lightDir));

	half3 h = normalize(lightDir + viewDir);
	float nh = max(0, dot(s.Normal, h));
	float spec = pow(nh, s.Specular * 128.0) * s.Gloss;

	fixed4 c;
	c.rgb = (s.Albedo) * _LightColor0.rgb * diff * atten + _LightColor0.rgb * spec;
	c.a = 1;
	return c;
}


