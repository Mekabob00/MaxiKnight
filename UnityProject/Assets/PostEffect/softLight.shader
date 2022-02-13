Shader "Masod/post/softLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightPower("LightPower",Float)=1
        _BlurSize("BlurSize",Float)=1
        _SoftLight("SoftLight",2D)="black"{}
        _Blend("Blend",Float)=1

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        UsePass "Masod/post/Blur/GAUSSIAN_BLUR_H"


        UsePass "Masod/post/Blur/GAUSSIAN_BLUR_V"



        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            sampler2D _MainTex;
            sampler2D _SoftLight;
            float _BlurSize;
            float _LightPower;
            float _Blend;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 softLight = tex2D(_SoftLight,i.uv);
                col=lerp(col,softLight * _LightPower,_Blend);
                return col;
            }
            ENDCG
        }
    }
}
