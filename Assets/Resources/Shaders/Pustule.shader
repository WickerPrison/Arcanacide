Shader "Unlit/Pustule"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _EdgeColor("Edge Color", Color) = (1,1,1,1)
        _Edge("Edge", float) = 0
        _CameraAngle("CameraAngle", Vector) = (0, .77, -.64)
        _FadeawayOffset("Fadeaway Offset", float) = 0

        _PerlinTex("Perlin Noise", 2D) = "white" {}
        _PerlinSize("Perlin Size", float) = 0
        _PerlinSpeed("Perlin Speed", float) = 0
        _PerlinTime("Perlin Time", float) = 0
        _PerlinIntensity("Perlin Intensity", float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque"}// "Queue" = "Transparent" }
        //Blend One OneMinusSrcAlpha
        Cull Back
        Lighting Off
        ZWrite On
        ZTest On

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

            float4 _Color;
            float _Edge;
            float3 _CameraAngle;
            float4 _EdgeColor;
            float _FadeawayOffset;

            sampler2D _PerlinTex;
            float _PerlinSize;
            float _PerlinSpeed;
            float _PerlinTime;
            float _PerlinIntensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float viewNormal = 1 - dot(i.normal, _CameraAngle);
                float edgeMask = viewNormal < _Edge;

           
                float time = _Time.y + _PerlinTime;
                float2 perlinUV1 = float2(i.uv.x + time * 1.1 * _PerlinSpeed, i.uv.y + time * _PerlinSpeed);
                float4 perlin1 = 1 - tex2D(_PerlinTex, perlinUV1 / _PerlinSize);

                float2 perlinUV2 = float2(i.uv.y - time * 1.1 * _PerlinSpeed, i.uv.x - time * _PerlinSpeed);
                float4 perlin2 = 1 - tex2D(_PerlinTex, perlinUV2 / _PerlinSize);
                float colorMod = (perlin1 + perlin2) / 2 * _PerlinIntensity + 0.1;
                colorMod = colorMod > 0.34;

                //* (viewNormal + _FadeawayOffset)
                _Color.xyz = lerp(_EdgeColor, _Color.xyz * colorMod, edgeMask);
                return float4(_Color.xyz, 1);
            }
            ENDCG
        }
    }
}
