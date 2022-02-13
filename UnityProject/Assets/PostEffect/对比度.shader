Shader "Masod/post/Contrast"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Brightness("Brightness",Float)=1
        _Saturation("Saturation",Float)=1
        _Contrast("Contrast",Float)=1
        _Hue("_Hue",Float)=0
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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float  _Brightness;
            float  _Saturation;
            float  _Contrast;
            float _Hue;
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 render = tex2D(_MainTex, i.uv);
                fixed4 finalCol=render*_Brightness;      
                fixed blackWhite=0.2125*render.r+0.7154*render.g+0.0721*render.b;
                fixed4 blackWhiteCol = fixed4(blackWhite,blackWhite,blackWhite,1);
                finalCol=lerp(blackWhiteCol,finalCol,_Saturation);
                fixed4 avgCol=fixed4(0.5,0.5,0.5,1);
                finalCol = lerp(avgCol,finalCol,_Contrast);
                fixed3 hsv = rgb2hsv(finalCol.rgb);
                hsv.x =frac( hsv.x+_Hue);
                hsv=hsv2rgb(hsv);
                finalCol = float4(hsv,finalCol.a);





                return finalCol;
            }
            ENDCG
        }
    }
}
