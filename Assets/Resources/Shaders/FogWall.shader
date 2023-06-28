Shader "Unlit/FogWall"
{
 	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

		//My Properties
		_PerlinTex ("Perlin Texture", 2D) = "white" {}
		_PerlinSpeed ("Perlin Speed", float) = 0
		_PerlinSize( "Perlin Size", float) = 0
		_FadeIn ("Fade In", range(0,1)) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 uv  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color * _Color;
				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap (o.vertex);
				#endif

				return o;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			sampler2D _PerlinTex;
			float _PerlinSpeed;
			float _PerlinSize;
			float _FadeIn;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (i.uv) * i.color;

				float2 perlinUV1 = float2(i.uv.x + _Time.y * _PerlinSpeed * 1.1, i.uv.y + _Time.y * _PerlinSpeed);
				float4 perlin1 = tex2D(_PerlinTex, perlinUV1 / _PerlinSize);

				float2 perlinUV2 = float2(i.uv.x - _Time.y * _PerlinSpeed, i.uv.y - _Time.y * _PerlinSpeed);
				float4 perlin2 = tex2D(_PerlinTex, perlinUV2 / _PerlinSize);
				c.a *= (perlin1 + perlin2) / 2 - .3;
				c.a *= _FadeIn;

				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
