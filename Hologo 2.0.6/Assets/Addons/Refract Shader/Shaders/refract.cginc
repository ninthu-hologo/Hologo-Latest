float rand(float2 co){
    return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453)*2.0-1.0;
}

float2 rand2(float2 co){
    float x = rand(co);
    return float2( x, rand(co+x) );
}

float3 rand3( float2 co ){
    float x = rand(co);
    float y = rand(co+x);
    return float3( x, y, rand(co+x+y) );
}

fixed3 getRefracted( sampler2D tex, float3 worldViewDir, float3 worldNormal, float3 worldPos, float roughness, float ior, float ca, int samples ){
    fixed3 result = fixed3(0.0,0.0,0.0);
    for(int j = 0; j < samples; j++){
        float loopOffset = j*0.1;
        float3 v = rand3( worldPos.xy + loopOffset ) * roughness*0.1;
        float3 worldRefractionDir = refract( worldViewDir, worldNormal + v, ior );
        float3 worldRefractionPos = worldPos.xyz + worldRefractionDir * ior;
        float4 clipRefractionPos = mul( UNITY_MATRIX_VP, float4( worldRefractionPos,1.0) );
        float4 coord = ComputeGrabScreenPos( clipRefractionPos);
        if(ca < 0.001){
            result += tex2Dproj( tex, coord );
        }else{
            result += fixed3(
                tex2Dproj( tex, coord+float4( -ca, 0.0 ,0.0,0.0) ).r,
                tex2Dproj( tex, coord+float4( 0.0, 0.0 ,0.0,0.0)  ).g,
                tex2Dproj( tex, coord+float4( ca, 0.0,0.0,0.0)  ).b
            );
        }
    }
    result /= samples;
    return result;
}