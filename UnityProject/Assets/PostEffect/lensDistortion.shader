Shader "Masod/post/test"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Power("Power",Float)=1
        _Fade("Fade",Float)=1
        _Zoom("Zoom",Float)=1
        _EffectSelect("EffectSelect",Int)=1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
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
            float _Power;
            float _Fade;
            float _Zoom;
            int _EffectSelect;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 trans=i.uv-float2(0.5,0.5);
                float2 dir = normalize(trans);
                float scale = length(trans);
                float tt = scale/0.7071;
               float warp = pow(tt,_Fade);
               warp = lerp(_Power,1,warp);
               scale=pow(scale,_Fade);
               scale *= 0.7071;
               scale=scale*_Zoom;
               float2 newUv=dir * scale+float2(0.5,0.5);

               warp = warp;
               warp *=_Zoom;
               float2 newUv2=trans * warp+float2(0.5,0.5);

               float2 Uv = lerp(newUv2,newUv,_EffectSelect);


               fixed4 col = tex2D(_MainTex, Uv);
               return col;
               //return float4(scale,scale,scale,1);
            }
            ENDHLSL
        }
    }
}
