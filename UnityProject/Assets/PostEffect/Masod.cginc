#ifndef MASOD_INCLUDED
#define MASOD_INCLUDED  
#include "UnityCG.cginc"

inline float Random (float seed) {
    return frac(sin(dot(seed, float2(12.9898,78.233)))*43758.5453123);
}

inline float3 rgb2hsv(float3 c)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
	float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

inline float3 hsv2rgb( float3 c ){
        float3 rgb = clamp( abs(fmod(c.x*6.0+float3(0.0,4.0,2.0),6)-3.0)-1.0, 0, 1);
        rgb = rgb*rgb*(3.0-2.0*rgb);
        return c.z * lerp( float3(1,1,1), rgb, c.y);
}



inline float4 test( in float3 pos )
{
    return mul(UNITY_MATRIX_VP, float4(pos, 1.0));
}




inline float4 WorldPosRebuildVertex(float2 uv,float4x4 Corners){
        int index = 0;
        if(uv.x<0.5 && uv.y <0.5){
            index=0;
        }
        else if ((uv.x>0.5 && uv.y <0.5)){
            index=1;
        }

        else if ((uv.x>0.5 && uv.y >0.5)){
            index=2;
        }

        else{
            index =3;
        }

        return float4 (Corners[index]);
}

inline float3 WorldPosRebuildFrag(float2 uv,float4 interpolatedRay,float depth){
        float linerDepth=LinearEyeDepth(depth);
        float3 worldPos = _WorldSpaceCameraPos + linerDepth*interpolatedRay.xyz;
        return worldPos;
}


#endif  