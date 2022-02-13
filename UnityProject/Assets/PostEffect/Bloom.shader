Shader "Masod/post/Bloom"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BloomPower("BloomPower",Float)=1
        _BlurSize("BlurSize",Float)=1
        _Bloom("Bloom",2D)="black"{}
        _Blend("Blend",Float)=1
        _Threshold("Threshold",Float)=1

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float _Threshold;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed average = (col.r+col.g+col.b)/3;
                fixed   bloomarea = step(_Threshold,average);
                fixed4 bloomTex = bloomarea * col;
                return bloomTex;
            }
            ENDCG
        }

        UsePass "Masod/post/Blur/GAUSSIAN_BLUR_H"


        UsePass "Masod/post/Blur/GAUSSIAN_BLUR_V"

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float  _BloomPower;
            sampler2D _Bloom;
            float _Blend;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 bloom = tex2D(_Bloom,i.uv);
                fixed4 bloomAfter = col+ bloom * _BloomPower ;
                fixed4 fincol =lerp(col,bloomAfter,_Blend);

                return fincol;




            }
            ENDCG
        }









    }
}
