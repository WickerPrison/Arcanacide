Shader "Unlit/Skybeam"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PerlinNoise("Perlin Noise", 2D) = "white" {}
        _FloorColor ("Floor Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}

        Cull back
        Lighting Off
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
                float3 normals : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _PerlinNoise;
            float4 _FloorColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normals;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 perlinUV = float2(i.uv.x + _Time.y + i.uv.y * 4, i.uv.y - _Time.y / 5);
                float3 perlin1 = tex2D(_PerlinNoise, perlinUV);
                float2 perlinUV2 = float2(i.uv.x + _Time.y * 1.1 + i.uv.y * 4, i.uv.y - _Time.y / 5.5);
                float3 perlin2 = tex2D(_PerlinNoise, perlinUV2);
                
                float3 perlin = (perlin1 + perlin2) / 2;

                float mask = perlin < 0.53;


                float4 color = lerp(_FloorColor, float4(1, 1, 1, 0), mask);


                return float4(color) * (abs(i.normal.y) < 0.9);



                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
