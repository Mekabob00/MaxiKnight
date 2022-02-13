Shader "Masod/post/Depth"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Depth("Depth",Float)=0
        _BlurSize("BlurSize",Float)=1
        _Blur ("Blur", 2D) = "white" {}
        _Power("Power",Float)=1
        _Power2("Power2",Float)=1
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
            sampler2D _CameraDepthTexture;
            float _Depth;
            sampler2D _Blur;
            float _Power;
            float _Power2;


            fixed4 frag (v2f i) : SV_Target
            {
                float pow1 = _Power;
                float dep = _Depth;
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 blur = tex2D(_Blur,i.uv);
                float posDepth = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv))).r;
                fixed absf =saturate( abs(posDepth-dep)/pow1);
                absf=lerp(0,1,absf);
                absf=pow(absf,_Power2);
                fixed4 finCol = lerp(col,blur,absf);

                
           
                return finCol;
            }
            ENDCG
        }
    }
}
