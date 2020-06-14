// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Sprites/DepthSprite-Lit"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _SpriteRect("Sprite Rect", Vector) = (1, 1, 1, 1)
		[PerRendererData] _BumpMap("Normalmap", 2D) = "bump" {}
		[PerRendererData] _DepthTex("Depth Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[PerRendererData] _TileLength("Z Tile Length", Float) = 0
		[PerRendererData] _SpriteYSize("Sprite Y Size", Float) = 0
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.0
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "AlphaTest"
				"IgnoreProjector" = "True"
				"RenderType" = "TransparentCutOut"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting On
			ZWrite On
			ZTest On
			Blend One OneMinusSrcAlpha

			CGPROGRAM
			#pragma target 3.0
			#pragma surface surf Lambert alpha vertex:vert  alphatest:_Cutoff fullforwardshadows
			#pragma multi_compile DUMMY PIXELSNAP_ON 
			#pragma exclude_renderers flash

			//struct appdata_full
			//{
			//	float4 vertex   : POSITION;
			//	float4 color    : COLOR;
			//	float2 texcoord : TEXCOORD0;
			//};

			//struct v2f
			//{
			//	float4 vertex   : SV_POSITION;
			//	fixed4 color : COLOR;
			//	float2 texcoord  : TEXCOORD0;
			//};

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float3 originNormal;
				fixed4 color;
			};

			fixed4 _Color;

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			sampler2D _DepthTex;
			sampler2D _BumpMap;


			float4 _MainTex_TexelSize;
			float4 _SpriteRect;
			float _AlphaSplitEnabled;
			float _TileLength;
			float _SpriteYSize;


			void vert(inout appdata_full IN, out Input o)
			{
				float2 offset = float2( _SpriteRect.x / _MainTex_TexelSize.z, _SpriteRect.y / _MainTex_TexelSize.w );
				float2 range = float2( _SpriteRect.z / _MainTex_TexelSize.z, _SpriteRect.w / _MainTex_TexelSize.w );
				float4 lodTexcoord = float4( clamp((IN.texcoord.x - offset.x) / range.x, 0.005, 0.995), 
					clamp((IN.texcoord.y - offset.y) / range.y, 0.001, 0.999), 
					0, 
					0 );
				float4 depth = tex2Dlod(_DepthTex, lodTexcoord);
				IN.vertex.z = IN.vertex.z + _TileLength * (depth.r - 0.5) * _SpriteRect.w / _SpriteYSize;
				#ifdef PIXELSNAP_ON
				IN.vertex = UnityPixelSnap(IN.vertex);
				#endif

				IN.normal = normalize(float3(0, 1, - 1));
				IN.tangent = float4(1, 0, 0, 1);

				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.color = IN.color * _Color;
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;

				clip(ceil(c.a) * 2 - 1);

				o.Albedo = c.rgb;
				o.Alpha = c.a;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Normal.z = o.Normal.z;
				o.Normal = normalize((half3)o.Normal);

			}

		ENDCG

		}
		Fallback "Transparent/Cutout/Diffuse"
}
