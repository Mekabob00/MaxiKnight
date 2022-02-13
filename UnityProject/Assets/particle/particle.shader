// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33217,y:32585,varname:node_4795,prsc:2|emission-2393-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32071,y:32552,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1c48d772e5f020f4a8f92000c2e7815e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2393,x:33008,y:32711,varname:node_2393,prsc:2|A-9173-OUT,B-2053-RGB,C-797-RGB,D-9248-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32003,y:32792,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32153,y:32951,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:32235,y:33081,varname:node_9248,prsc:2,v1:2;n:type:ShaderForge.SFN_Lerp,id:5449,x:32404,y:32398,varname:node_5449,prsc:2|A-889-RGB,B-7587-RGB,T-6074-RGB;n:type:ShaderForge.SFN_Color,id:889,x:32238,y:32173,ptovrint:False,ptlb:node_889,ptin:_node_889,varname:node_889,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4056604,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:7587,x:32198,y:32367,ptovrint:False,ptlb:node_7587,ptin:_node_7587,varname:node_7587,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.75622,c3:0.4402515,c4:1;n:type:ShaderForge.SFN_Multiply,id:282,x:32526,y:32566,varname:node_282,prsc:2|A-5449-OUT,B-6074-RGB;n:type:ShaderForge.SFN_Multiply,id:9173,x:32966,y:32454,varname:node_9173,prsc:2|A-2817-OUT,B-9322-OUT;n:type:ShaderForge.SFN_Power,id:2817,x:32769,y:32510,varname:node_2817,prsc:2|VAL-282-OUT,EXP-7671-OUT;n:type:ShaderForge.SFN_Slider,id:7671,x:32459,y:32259,ptovrint:False,ptlb:pow,ptin:_pow,varname:node_7671,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.088332,max:4;n:type:ShaderForge.SFN_Slider,id:9322,x:32796,y:32266,ptovrint:False,ptlb:mul,ptin:_mul,varname:node_9322,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:6.840205,max:10;proporder:6074-797-889-7587-7671-9322;pass:END;sub:END;*/

Shader "Shader Forge/particle" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (1,1,1,1)
        _node_889 ("node_889", Color) = (0.4056604,0,0,1)
        _node_7587 ("node_7587", Color) = (1,0.75622,0.4402515,1)
        _pow ("pow", Range(0, 4)) = 1.088332
        _mul ("mul", Range(0, 10)) = 6.840205
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _TintColor)
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_889)
                UNITY_DEFINE_INSTANCED_PROP( float4, _node_7587)
                UNITY_DEFINE_INSTANCED_PROP( float, _pow)
                UNITY_DEFINE_INSTANCED_PROP( float, _mul)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
////// Lighting:
////// Emissive:
                float4 _node_889_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_889 );
                float4 _node_7587_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_7587 );
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float _pow_var = UNITY_ACCESS_INSTANCED_PROP( Props, _pow );
                float _mul_var = UNITY_ACCESS_INSTANCED_PROP( Props, _mul );
                float4 _TintColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TintColor );
                float3 emissive = ((pow((lerp(_node_889_var.rgb,_node_7587_var.rgb,_MainTex_var.rgb)*_MainTex_var.rgb),_pow_var)*_mul_var)*i.vertexColor.rgb*_TintColor_var.rgb*2.0);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
