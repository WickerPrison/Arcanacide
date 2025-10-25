float3 ColorChange(float4 col, float3 oldColor, float3 newColor, float threshold){
	float3 threshold3 = float3(threshold, threshold, threshold);
	float3 diff = length(abs((normalize(col.xyz) - normalize(oldColor.xyz)))) < length(threshold3);
	return lerp(col.xyz, newColor.xyz, diff);
}