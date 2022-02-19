// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "explosionFire"
{
	Properties
	{
		_Gradient("Gradient", 2D) = "white" {}
		_RampMap("RampMap", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (21.11213,21.11213,21.11213,1)
		_Select("Select", Float) = 1
		_Float4("Float 4", Float) = 0
		_RampOffset("RampOffset", Float) = 0
		_lightOffset("lightOffset", Float) = 0
		_MinLumin("MinLumin", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "black" {}
		_NoiseScale("NoiseScale", Range( 0 , 5)) = 1

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Off
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _RampMap;
			uniform sampler2D _Gradient;
			uniform sampler2D _TextureSample0;
			uniform float _NoiseScale;
			uniform float _RampOffset;
			uniform float _Select;
			uniform float _Float4;
			uniform float4 _Color0;
			uniform float _lightOffset;
			uniform float _MinLumin;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_texcoord2 = v.vertex;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 texCoord2 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float4 unityObjectToClipPos44 = UnityObjectToClipPos( i.ase_texcoord2.xyz );
				float4 computeScreenPos46 = ComputeScreenPos( unityObjectToClipPos44 );
				float4 appendResult52 = (float4((computeScreenPos46).x , ( (computeScreenPos46).y / ( _ScreenParams.x / _ScreenParams.y ) ) , 0.0 , 0.0));
				float4 tex2DNode41 = tex2D( _TextureSample0, ( appendResult52 * _NoiseScale ).xy );
				float4 appendResult42 = (float4(tex2DNode41.r , tex2DNode41.r , 0.0 , 0.0));
				float4 tex2DNode1 = tex2D( _Gradient, ( float4( texCoord2, 0.0 , 0.0 ) + ( appendResult42 * i.ase_color.b ) ).xy );
				float4 appendResult15 = (float4(( tex2DNode1.r + _RampOffset ) , _Select , 0.0 , 0.0));
				float4 tex2DNode11 = tex2D( _RampMap, appendResult15.xy );
				float4 appendResult16 = (float4(tex2DNode11.r , tex2DNode11.g , tex2DNode11.b , 0.0));
				float smoothstepResult8 = smoothstep( 0.04 , 0.16 , ( tex2DNode1.a - i.ase_color.a ));
				float4 appendResult21 = (float4(( ( appendResult16 * pow( ( tex2DNode1.r + 1.0 ) , _Float4 ) ) * _Color0 ).rgb , ( saturate( ( abs( ( tex2DNode1.r - _lightOffset ) ) + _MinLumin ) ) * smoothstepResult8 )));
				
				
				finalColor = appendResult21;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18933
154;141.3333;1251.333;717.6667;2703.463;72.65274;1;True;True
Node;AmplifyShaderEditor.PosVertexDataNode;45;-3191.753,-26.5061;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.UnityObjToClipPosHlpNode;44;-2974.43,-9.452755;Inherit;False;1;0;FLOAT3;0,0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComputeScreenPosHlpNode;46;-2721.909,28.90585;Inherit;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenParams;47;-2726.368,209.1744;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;48;-2485.371,216.4413;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;50;-2512.392,99.23112;Inherit;False;False;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;49;-2506.393,-37.80938;Inherit;False;True;False;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;53;-2297.157,180.137;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-2259.923,331.1445;Inherit;False;Property;_NoiseScale;NoiseScale;9;0;Create;True;0;0;0;False;0;False;1;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;52;-2192.955,10.85336;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1909.037,81.33398;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;41;-1658.173,0.9964404;Inherit;True;Property;_TextureSample0;Texture Sample 0;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;4;-1436.218,407.0569;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;42;-1328.886,-0.2507625;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-1121.651,102.4358;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1830.012,-236.8754;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-1081,-110.8726;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-653.319,68.96432;Inherit;False;Property;_RampOffset;RampOffset;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-889.4767,-311.9045;Inherit;True;Property;_Gradient;Gradient;0;0;Create;True;0;0;0;False;0;False;-1;None;bbda5b96b1d9e3b4d9f69447e6067af9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;34;396.7523,-237.4327;Inherit;False;Property;_lightOffset;lightOffset;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-526.3574,630.8212;Inherit;False;Property;_Select;Select;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-431.1678,-72.43503;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;35;581.6161,-320.226;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-237.2816,-71.19662;Inherit;False;Constant;_Float3;Float 3;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-312.389,522.4759;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-53.15674,-184.0929;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;20.89605,-14.64941;Inherit;False;Property;_Float4;Float 4;4;0;Create;True;0;0;0;False;0;False;0;2.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-131.806,368.6779;Inherit;True;Property;_RampMap;RampMap;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;36;608.5593,-179.4019;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;444.8799,-100.0143;Inherit;False;Property;_MinLumin;MinLumin;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;30;223.9294,-182.2186;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-635.9774,414.0013;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;0;False;0;False;0.04;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;193.5741,346.5775;Inherit;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-647.2252,506.041;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;0;False;0;False;0.16;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;6;-793.5582,217.0788;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;620.3776,-84.87218;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;430.8265,287.6685;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;38;793.3116,-29.43398;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;8;-74.14516,228.439;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;397.3563,452.5147;Inherit;False;Property;_Color0;Color 0;2;1;[HDR];Create;True;0;0;0;False;0;False;21.11213,21.11213,21.11213,1;1.498039,1.498039,1.498039,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;731.594,348.8812;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;813.2968,223.2707;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1499.338,243.5846;Inherit;False;Property;_NoisePower;NoisePower;10;0;Create;True;0;0;0;False;0;False;1;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;1056.126,218.2132;Inherit;False;COLOR;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1387.368,133.7033;Float;False;True;-1;2;ASEMaterialInspector;100;1;explosionFire;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;2;5;False;-1;10;False;-1;0;4;False;-1;1;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;True;True;2;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Transparent=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
WireConnection;44;0;45;0
WireConnection;46;0;44;0
WireConnection;48;0;47;1
WireConnection;48;1;47;2
WireConnection;50;0;46;0
WireConnection;49;0;46;0
WireConnection;53;0;50;0
WireConnection;53;1;48;0
WireConnection;52;0;49;0
WireConnection;52;1;53;0
WireConnection;54;0;52;0
WireConnection;54;1;55;0
WireConnection;41;1;54;0
WireConnection;42;0;41;1
WireConnection;42;1;41;1
WireConnection;56;0;42;0
WireConnection;56;1;4;3
WireConnection;43;0;2;0
WireConnection;43;1;56;0
WireConnection;1;1;43;0
WireConnection;32;0;1;1
WireConnection;32;1;33;0
WireConnection;35;0;1;1
WireConnection;35;1;34;0
WireConnection;15;0;32;0
WireConnection;15;1;14;0
WireConnection;28;0;1;1
WireConnection;28;1;29;0
WireConnection;11;1;15;0
WireConnection;36;0;35;0
WireConnection;30;0;28;0
WireConnection;30;1;31;0
WireConnection;16;0;11;1
WireConnection;16;1;11;2
WireConnection;16;2;11;3
WireConnection;6;0;1;4
WireConnection;6;1;4;4
WireConnection;39;0;36;0
WireConnection;39;1;40;0
WireConnection;19;0;16;0
WireConnection;19;1;30;0
WireConnection;38;0;39;0
WireConnection;8;0;6;0
WireConnection;8;1;9;0
WireConnection;8;2;10;0
WireConnection;26;0;19;0
WireConnection;26;1;23;0
WireConnection;37;0;38;0
WireConnection;37;1;8;0
WireConnection;21;0;26;0
WireConnection;21;3;37;0
WireConnection;0;0;21;0
ASEEND*/
//CHKSM=0EC8EA4A57FC883B9DECF7FCF24CA5FC8CEC5505