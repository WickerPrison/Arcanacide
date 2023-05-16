Shader "Unlit/Beam"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                fixed4 color : COLOR0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 spiral =  cos((i.uv.x * 5 - i.uv.y - _Time.y * 5) * 5) * 0.5 + 0.5;
                float4 mask = spiral > 0.05;
                float sigma = 0.2;
                float mu = 0.5;
                float gauss = (1/(sigma * sqrt(2 * 3.14)) * exp(-pow(i.uv.y - mu, 2)/(2 * pow(sigma,2)))) * 0.5;
                i.color.a = gauss * mask;
                return i.color;
            }
            ENDCG
        }
    }
}
