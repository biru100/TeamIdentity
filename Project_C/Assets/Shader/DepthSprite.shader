// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/DepthSprite"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _SpriteRect("Sprite Rect", Vector) = (1, 1, 1, 1)
		[PerRendererData] _DepthTex("Depth Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[PerRendererData] _TileLength("Z Tile Length", Float) = 0
		[PerRendererData] _SpriteYSize("Sprite Y Size", Float) = 0
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "AlphaTest"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Back
			Lighting On
			ZWrite On
			ZTest On
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
				};

				fixed4 _Color;

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				sampler2D _DepthTex;
				float4 _MainTex_TexelSize;
				float4 _SpriteRect;
				float _AlphaSplitEnabled;
				float _TileLength;
				float _SpriteYSize;


				v2f vert(appdata_t IN)
				{
					v2f OUT;
					float2 offset = float2( _SpriteRect.x / _MainTex_TexelSize.z, _SpriteRect.y / _MainTex_TexelSize.w );
					float2 range = float2( _SpriteRect.z / _MainTex_TexelSize.z, _SpriteRect.w / _MainTex_TexelSize.w );
					float4 lodTexcoord = float4( clamp((IN.texcoord.x - offset.x) / range.x, 0.005, 0.995), 
						clamp((IN.texcoord.y - offset.y) / range.y, 0.001, 0.999), 
						0, 
						0 );
					float4 depth = tex2Dlod(_DepthTex, lodTexcoord);
					IN.vertex.z = IN.vertex.z + _TileLength * (depth.r - 0.5) * _SpriteRect.w / _SpriteYSize;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color * _Color;
					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif


					return OUT;
				}


				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					//IN.texcoord.y = round((IN.texcoord.y * _SpriteYSize)) / _SpriteYSize;
					
					fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
					c.rgb *= c.a;
					clip(ceil(c.a) * 2 - 1);
					return c;
				}



			ENDCG
			}
		}
}
