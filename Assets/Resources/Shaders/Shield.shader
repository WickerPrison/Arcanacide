Shader "Unlit/DefaultSprite"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

		// my Properties
		_HexTex ("Hexagon Texture", 2D) = "white" {}
		_Perlin ("Perlin Noise", 2D) = "white" {}
		_PerlinSize ("Perlin Size", float) = 2
		_EdgeDecay ("Edge Decay", float) = 0.6
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

			//my Properties
			sampler2D _HexTex;
			sampler2D _Perlin;
			float _PerlinSize;
			float _EdgeDecay;

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
				float2 perlinUV = float2(i.uv.x + _Time.y / 50, i.uv.y - _Time.y / 20);
				// subtracting 0.47 centers the perlin noise around 0. This should be 0.5 instead but 0.47 ended up looking better
				float perlin = tex2D(_Perlin, perlinUV / _PerlinSize) - 0.47;
				float4 hexTex = tex2D(_HexTex, i.uv + perlin);
				c.rgb *= 1 - hexTex;

				float2 perlinUV2 = float2(i.uv.x + _Time.y /10, i.uv.y - _Time.y / 2);
				float perlin2 = tex2D(_Perlin, perlinUV2);

				float2 uvCentered = i.uv * 2 - 1;
                float radialDist = 1 - length(uvCentered);

				float mask = perlin2 + radialDist > _EdgeDecay;
				c.a *= mask * 1.5;
				c.rgb *= c.a;

				float fadeaway = 1 - length(i.uv.x * 2 - 1) + .2;
				float fadeaway2 = 1 - length(i.uv.y * 2 - 1) + .2;
				return c * fadeaway * fadeaway2;
				return c;
			}
		ENDCG
		}
	}
}
