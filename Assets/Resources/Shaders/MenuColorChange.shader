Shader "Unlit/CharacterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OldColor ("Existing Color", Color) = (1,1,1,1)
        _NewColor ("Desired Color", Color) = (1,1,1,1)
        _Threshold ("Color Threshold", float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        ZWrite Off

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
            float4 _NewColor;
            float4 _OldColor;
            float _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                clip(col.a - 0.1);

                col *= col > 0.1;

                float3 threshold = float3(_Threshold, _Threshold,_Threshold);

                float3 diff = length(abs((normalize(col.xyz) - normalize(_OldColor.xyz)))) < length(threshold);

                float3 outColor = lerp(col.xyz, _NewColor.xyz, diff);

                return float4(outColor,1);
            }
            ENDCG
        }
    }
}
