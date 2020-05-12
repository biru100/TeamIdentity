Shader "Custom/DepthVertexlit"
{
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _SpriteRect("Sprite Rect", Vector) = (1, 1, 1, 1)
		_DepthTex("Depth Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_ZTileLength("Z Tile Length", Float) = 0
		_SpriteYSize("Sprite Y Size", Float) = 0
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 80

			// Non-lightmapped
			Pass {
				Tags { "LightMode" = "Vertex" }

				Material {
					Diffuse(1,1,1,1)
					Ambient(1,1,1,1)
				}
				Lighting On
				SetTexture[_MainTex] {
					constantColor(1,1,1,1)
					Combine texture * primary DOUBLE, constant // UNITY_OPAQUE_ALPHA_FFP
				}
			}

			// Lightmapped
			Pass
			{
				Tags{ "LIGHTMODE" = "VertexLM" "RenderType" = "Transparent" }

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#include "UnityCG.cginc"
				#pragma multi_compile_fog
				#define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

				float4 _MainTex_ST;
				
				sampler2D _MainTex;
				sampler2D _AlphaTex;
				sampler2D _DepthTex;
				float4 _MainTex_TexelSize;
				float4 _SpriteRect;
				float _AlphaSplitEnabled;
				float _ZTileLength;
				float _SpriteYSize;

				struct appdata
				{
					float3 pos : POSITION;
					float3 uv1 : TEXCOORD1;
					float3 uv0 : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				#if USING_FOG
					fixed fog : TEXCOORD2;
				#endif
					float4 pos : SV_POSITION;

					UNITY_VERTEX_OUTPUT_STEREO
				};

				v2f vert(appdata IN)
				{
					v2f o;

					float2 offset = { _SpriteRect.x / _MainTex_TexelSize.z, _SpriteRect.y / _MainTex_TexelSize.w };
					float2 range = { _SpriteRect.z / _MainTex_TexelSize.z, _SpriteRect.w / _MainTex_TexelSize.w };
					float4 lodTexcoord = { (IN.uv1.x - offset.x) / range.x, (IN.uv1.y - offset.y) / range.y, 0, 0 };
					float4 depth = tex2Dlod(_DepthTex, lodTexcoord);
					IN.pos.z = IN.pos.z + _ZTileLength * (depth.r - 0.5) * _SpriteRect.w / _SpriteYSize;

					UNITY_SETUP_INSTANCE_ID(IN);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


					o.uv0 = IN.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					o.uv1 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;

				#if USING_FOG
					float3 eyePos = UnityObjectToViewPos(IN.pos);
					float fogCoord = length(eyePos.xyz);
					UNITY_CALC_FOG_FACTOR_RAW(fogCoord);
					o.fog = saturate(unityFogFactor);
				#endif

					o.pos = UnityObjectToClipPos(IN.pos);
					return o;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 col;
					fixed4 tex = UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.uv0.xy);
					half3 bakedColor = DecodeLightmap(tex);

					tex = tex2D(_MainTex, IN.uv1.xy);
					col.rgb = tex.rgb * bakedColor;
					col.a = 1.0f;

					#if USING_FOG
					col.rgb = lerp(unity_FogColor.rgb, col.rgb, IN.fog);
					#endif

					return col;
				}

				ENDCG
			}

			// Pass to render object as a shadow caster
			Pass
			{
				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }

				ZWrite On ZTest LEqual Cull Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_shadowcaster
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _DepthTex;
				float4 _MainTex_TexelSize;
				float4 _SpriteRect;
				float _ZTileLength;
				float _SpriteYSize;

				struct v2f {
					V2F_SHADOW_CASTER;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				v2f vert(appdata_base v)
				{
					v2f o;

					float2 offset = { _SpriteRect.x / _MainTex_TexelSize.z, _SpriteRect.y / _MainTex_TexelSize.w };
					float2 range = { _SpriteRect.z / _MainTex_TexelSize.z, _SpriteRect.w / _MainTex_TexelSize.w };
					float4 lodTexcoord = { (v.texcoord.x - offset.x) / range.x, (v.texcoord.y - offset.y) / range.y, 0, 0 };
					float4 depth = tex2Dlod(_DepthTex, lodTexcoord);
					v.vertex.z = v.vertex.z + _ZTileLength * (depth.r - 0.5) * _SpriteRect.w / _SpriteYSize;

					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					SHADOW_CASTER_FRAGMENT(i)
				}
				ENDCG
			}
		}
}