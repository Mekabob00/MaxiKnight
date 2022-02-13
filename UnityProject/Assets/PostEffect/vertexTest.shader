Shader "Maosd/post/vertexTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Buffer0("Texture",2D)="white" {}
        _Buffer1("Texture",2D)="white" {}

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

                return  float4(worldPos,1);
            }
            ENDCG
        }

    }
}
