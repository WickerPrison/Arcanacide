Shader "Unlit/Beam2"
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
                float sigma = 0.2 + 0.03 * cos(_Time.y * 3);
                float mu = 0.5;
                float amp = sigma * 2.5;
                float gauss = (1/(sigma * sqrt(2 * 3.14)) * exp(-pow(i.uv.y - mu, 2)/(2 * pow(sigma,2)))) * amp;
                float centerMask = gauss > .8;
                gauss += centerMask;
                i.color.a = gauss;
                
                return i.color;
            }
            ENDCG
        }
    }
}
