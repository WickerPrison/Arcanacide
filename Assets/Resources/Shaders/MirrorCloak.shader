Shader "Unlit/MirrorCloak"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PerlinTex ("Perlin Texture", 2D) = "white" {}
        _HexTex ("Hexagon Texture", 2D) = "white" {}
        _RadialOffset ("Radial Offset", float) = -0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _PerlinTex;
            sampler2D _HexTex;
            float _RadialOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvCentered = i.uv * 2 - 1;
                float radialDist = 1 - length(uvCentered) - 0.2;
                float mask = radialDist;

				float2 perlinUV = float2(i.uv.x + _Time.y / 50, i.uv.y - _Time.y / 20);
				// subtracting 0.47 centers the perlin noise around 0 intensity. This should be 0.5 but 0.47 ended up looking better
				float perlin = tex2D(_PerlinTex, perlinUV / 2) - 0.47;
				float4 hexTex = tex2D(_HexTex, i.uv + perlin);
                hexTex = hexTex > 0;

                hexTex.a *= radialDist + _RadialOffset;

                return hexTex;

            }
            ENDCG
        }
    }
}
