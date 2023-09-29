// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Bubble"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Edge ("Edge", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Blend One OneMinusSrcAlpha
        Cull Back
        Lighting Off
        ZWrite off

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
                //float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };

            float4 _Color;
            float _Edge;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normals;
                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //float3 viewDir = _WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld).xyz;
                float viewNormal = 1 - dot(i.normal, i.viewDir);
                float edgeMask = viewNormal < _Edge;
                _Color.xyz = lerp(float3(1, 1, 1), _Color.xyz, edgeMask);
                return float4(_Color.xyz, 1);
            }
            ENDCG
        }
    }
}
