Shader "Unlit/EnemySprite"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

		// These are the properties I added
		_FlashWhite ("Flash White", float) = 0
		_DissolveTex ("Dissolve Texture", 2D) = "white" {}
		_DissolveProg ("Dissolve Progression", range(0,1)) = 0
		_Density("Dissolve Density", float) = 0.5
		_EdgeWidth("Edge Width", float) = 0.05

		_ColorChange("Color Change", float) = 0
		_OriginalColor("Original Color", Color) = (1,1,1,1)
		_NewColor("New Color", Color) = (1,1,1,1)
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
			#include "./Modules/ColorChange.hlsl"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
			};
			
			fixed4 _Color;
			// my variables
			float _FlashWhite;
			sampler2D _DissolveTex;
			float _DissolveProg;
			float _Density;
			float _EdgeWidth;
			bool _ColorChange;
			fixed4 _OriginalColor;
			fixed4 _NewColor;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;

				// this is the stuff I added                       
				clip(c.a - 0.5);
                float2 projection = IN.worldPos.xy * _Density;
                fixed4 dissolveTex = tex2D(_DissolveTex, projection);

				if(_ColorChange){
					c.xyz = ColorChange(c, _OriginalColor, _NewColor, 0.1);
				}

                float mask = dissolveTex - _DissolveProg;
                clip(mask);

                float edgeMask = mask < _EdgeWidth;

                float3 outColor = lerp(c.xyz, float3(1,1,1), edgeMask);

                return float4(outColor + _FlashWhite, 1);
			}
		ENDCG
		}
	}
}
