
half4 PolarMap(half3 position, half3 normal, sampler2D poleTex, sampler2D wrapTex, half2 uv, half scale, half sharpness) {
	position = (position - 0.5) * scale;
	uv = uv * scale;
	uv.x *= 2;

	half4 poleDiff = tex2D(poleTex, position.xz);
	half4 wrapDiff = tex2D(wrapTex, uv);

	half3 blendWeights = pow(abs(normal), sharpness);

	blendWeights = blendWeights / (blendWeights.x + blendWeights.y + blendWeights.z);

	return poleDiff * blendWeights.y + wrapDiff * (1 - blendWeights.y);
}

half4 PolarMap(half3 position, half3 normal, sampler2D tex, half2 uv, half scale, half sharpness) {
	return PolarMap(position, normal, tex, tex, uv, scale, sharpness);
}

half PoleMask(half3 position, half ammount, half sharpness) {
	ammount = 1 - ammount;
	half frac = (ammount / 2);
	return lerp(saturate((abs(position.y) - frac) * sharpness * ammount), 1, 1 - ammount);
}

half2 ScatterUV(float3 normal, float3 lightDir, float3 viewDir) {
    return half2(dot (normal, lightDir) / 2.0 + 0.5, dot (normal, viewDir));
}