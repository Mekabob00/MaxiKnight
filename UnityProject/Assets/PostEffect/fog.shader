Shader "Masod/post/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _High("High",Float)=0
        _Blend("Blend",Float)=1
        _DirChange("DirChange",Int)=0
        _Fade("Fade",Float)=1
        _Color("FogColor",Color)=(1,1,1,1)
        _FadeDistance("FadeDistance",Float)=1

        _HighFogOn("HighFogOn",Int)=1
        _DepthBlend("DepthBlend",Float)=1
        _DepthFade("DepthFade",Float)=1
        _Depth("Depth",Float)=1
        _DepthFadeDistance("DepthFadeDistance",Float)=1
        _DepthFogOn("DepthFogOn",Int)=1

    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Masod.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 uv_depth : TEXCOORD1;
                float4 interpolatedRay : TEXCOORD2;
            };


            float4x4 _FrustunCorners; 
            sampler2D _MainTex;
            half4 _MainTex_TexelSize;
            sampler2D _CameraDepthTexture;
            float _High;
            float _Blend;
            int _DirChange;
            float _Fade;
            float4 _Color;
            float _FadeDistance;
            int _HighFogOn;
            float _DepthBlend;
            float _DepthFade;
            float _Depth;
            float _DepthFadeDistance;
            int _DepthFogOn;




            v2f vert (appdata v)
            {
                float4x4 kk=_FrustunCorners;
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv=v.uv;
                o.interpolatedRay = WorldPosRebuildVertex(o.uv,_FrustunCorners);

                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv);
                float3  worldPos = WorldPosRebuildFrag(i.uv,i.interpolatedRay,depth);
                float highAbs =abs( worldPos.y  -  _High);
                float dirCheck = step(worldPos.y,_High);
                dirCheck =abs( dirCheck-_DirChange);
                float lerp1=saturate( highAbs/_FadeDistance);
                float fogDensity = lerp(0,1,lerp1);
                fogDensity = pow(fogDensity,_Fade);
                fogDensity = fogDensity*dirCheck*_HighFogOn*_Blend;

 
                depth = LinearEyeDepth(depth);
                float depthAbs = abs( depth  -  _Depth);
                float DepthDirCheck = step(_Depth,depth);
                float lerp2=saturate( depthAbs/_DepthFadeDistance);
                float fogDensity2 = lerp(0,1,lerp2);
                fogDensity2=pow(fogDensity2,_DepthFade);
                fogDensity2=fogDensity2*DepthDirCheck*_DepthFogOn*_DepthBlend;

                fogDensity=max(fogDensity,fogDensity2);
                
                col = lerp(col,_Color,fogDensity);


                return  col;
            }
            ENDCG
        }

    }
}
