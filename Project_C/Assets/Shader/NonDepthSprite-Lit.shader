Shader "Sprites/NonDepthSprite-Lit"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _BumpMap("Normalmap", 2D) = "bump" {}
		_Color("Tint", Color) = (1,1,1,1)
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
			sampler2D _BumpMap;

			float _AlphaSplitEnabled;


			void vert(inout appdata_full IN, out Input o)
			{
				#ifdef PIXELSNAP_ON
				IN.vertex = UnityPixelSnap(IN.vertex);
				#endif

				IN.normal = float3(0, 0, -1);
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
				o.Emission = 1;

			}

		ENDCG

		}
			Fallback "Transparent/Cutout/Diffuse"
}
