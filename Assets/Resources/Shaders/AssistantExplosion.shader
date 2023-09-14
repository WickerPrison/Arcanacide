Shader "Unlit/AssistantExplosion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ExTime("Explosion Time", range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        ZWrite Off
        ZTest Off
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

            float _ExTime;

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

                float2 uvsCentered = i.uv * 2 - 1;

                float radialDist = length(uvsCentered);

                float peak = abs(radialDist - _ExTime) < 0.1;

                float4 outColor = float4(col.x,col.y,col.z, peak);

                return outColor * peak;
            }
            ENDCG
        }
    }
}
