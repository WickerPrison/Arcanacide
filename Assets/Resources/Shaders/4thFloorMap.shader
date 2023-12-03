Shader "Unlit/4thFloorMap"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Speed("Speed", float) = 1
		_Amplitude("Amplitude", float) = 1
		_Amount("Amount", range(0.0, 1.0)) = 1
		_ResX ("X Resolution", float) = 100
		_ResY ("Y Resolution", float) = 200
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
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
				fixed4 color : COLOR;
				float2 uv  : TEXCOORD0;
				float3 viewDir : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
			};

			fixed4 _Color;
			float _Speed;
			float _Amplitude;
			float _Amount;

			v2f vert(appdata_t v)
			{
				v2f o;
				v.vertex.x += sin(_Time.y * _Speed + v.vertex.y) * _Amplitude * _Amount;
				o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color * _Color;
				o.viewDir = WorldSpaceViewDir(v.vertex);
				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap(o.vertex);
				#endif

				return o;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			float _ResX;
			float _ResY;

			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D(_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			//This produces random values between 0 and 1
			float rand(float2 co)
			{
				return frac((sin(dot(co.xy, float2(12.345 * _Time.w, 67.890 * _Time.w))) * 12345.67890 + _Time.w));
			}

			fixed4 frag(float4 screenPos : SV_POSITION, v2f i) : SV_Target
			{
				float2 inputUV = float2(i.uv.x, i.uv.y);
				fixed4 c = SampleSpriteTexture(inputUV) * i.color;

				fixed4 sc = fixed4((screenPos.xy), 0.0, 1.0);
				sc *= 0.001;
				sc.x = round(sc.x * _ResX) / _ResX;
				sc.y = round(sc.y * _ResY) / _ResY;

				float noise = rand(sc.xy);
				float4 stat = lerp(float4(1, 1, 1, 1), float4(0, 0, 0, 0), noise.x);

				c.rgb *= c.a;
				return lerp(c, fixed4(stat.xyz, 1), _Amount);
			}
		ENDCG
		}
	}
}
