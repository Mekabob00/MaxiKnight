Shader "Unlit/garb"
{
    Properties
    {
        _Noise ("Noise", 2D) = "white" {}
        _Power("Power",Float)=1
        _Mask("Mask",2D)="white" {}
        _NoiseScale("NoiseScale",Float)=1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+100" }
        ZTest Off
        ZWrite Off
        LOD 100


        GrabPass
        {
            "_Texture"
        }




        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 Vc:COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 Vc : TEXCOORD1;
                float4 grabPos:TEXCOORD2;
                float2 OriginalUv : TEXCOORD3;
                float Dep : TEXCOORD4;
            };

            sampler2D _Noise;
            float4 _Noise_ST;
            float _Power;
            sampler2D _Texture;
            sampler2D _Mask;
            float _NoiseScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.OriginalUv=v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _Noise);
                o.Vc=v.Vc;
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.grabPos = ComputeScreenPos(o.vertex);
                o.Dep = o.grabPos.w;
                o.grabPos = o.grabPos/o.grabPos.w;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed Mask = tex2D(_Noise,i.uv).b;
                i.uv.x+=i.Vc.g*0.5;
                fixed4 noise = tex2D(_Noise,i.uv*_NoiseScale);
               // noise.xy=noise.xy*2-float2(1,1);
               // fixed2 Pos = i.grabPos/i.grabPos.w+noise.xy*_Power;
               fixed2 Pos =lerp( i.grabPos/i.grabPos.w,noise.xy,_Power);
                i.OriginalUv=(i.OriginalUv-float2(0.5,0.5))/i.Vc.w+float2(0.5,0.5);
               // i.OriginalUv= i.OriginalUv/i.Vc.w;

                //fixed4 Mask = tex2D(_Mask,i.OriginalUv);
                Mask = Mask * i.Vc.x;

                Pos= lerp( i.grabPos,Pos,Mask);

                fixed4 col = tex2D(_Texture,Pos);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
