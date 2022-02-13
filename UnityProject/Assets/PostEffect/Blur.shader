Shader "Masod/post/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize("BlurSize",Float)=1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            NAME "GAUSSIAN_BLUR_H"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            sampler2D _MainTex;
            half4 _MainTex_TexelSize;
            float _BlurSize;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                half2 uv[5]:TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos=UnityObjectToClipPos(v.vertex);
                half2 uv=v.uv;
                o.uv[0]=uv;
                o.uv[1]=uv+float2(_MainTex_TexelSize.x*1.0,0.0)*_BlurSize;
                o.uv[2]=uv-float2(_MainTex_TexelSize.x*1.0,0.0)*_BlurSize;
                o.uv[3]=uv+float2(_MainTex_TexelSize.x*2.0,0.0)*_BlurSize;
                o.uv[4]=uv-float2(_MainTex_TexelSize.x*2.0,0.0)*_BlurSize;
                return o;
            }

      

            fixed4 frag (v2f i) : SV_Target
            {
                float weight[3]={0.4026,0.2442,0.0545};
                fixed3 sum = tex2D(_MainTex,i.uv[0]).rgb*weight[0];
                for(int t=1;t<3;t++){
                    sum += tex2D(_MainTex,i.uv[t]).rgb*weight[t];
                    sum +=tex2D(_MainTex,i.uv[2*t]).rgb*weight[t];
                }
                return fixed4(sum,1.0);
            }
            ENDCG
        }

        Pass
        {
            NAME "GAUSSIAN_BLUR_V"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            sampler2D _MainTex;
            half4 _MainTex_TexelSize;
            float _BlurSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                half2 uv[5]:TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos=UnityObjectToClipPos(v.vertex);
                half2 uv=v.uv;
                o.uv[0]=uv;
                o.uv[1]=uv+float2(0.0,_MainTex_TexelSize.y*1.0)*_BlurSize;
                o.uv[2]=uv-float2(0.0,_MainTex_TexelSize.y*1.0)*_BlurSize;
                o.uv[3]=uv+float2(0.0,_MainTex_TexelSize.y*2.0)*_BlurSize;
                o.uv[4]=uv-float2(0.0,_MainTex_TexelSize.y*2.0)*_BlurSize;
                return o;
            }

      

            fixed4 frag (v2f i) : SV_Target
            {
                float weight[3]={0.4026,0.2442,0.0545};
                fixed3 sum = tex2D(_MainTex,i.uv[0]).rgb*weight[0];
                for(int t=1;t<3;t++){
                    sum += tex2D(_MainTex,i.uv[t]).rgb*weight[t];
                    sum +=tex2D(_MainTex,i.uv[2*t]).rgb*weight[t];
                }
                return fixed4(sum,1.0);
            }
            ENDCG
        }
    }
}
