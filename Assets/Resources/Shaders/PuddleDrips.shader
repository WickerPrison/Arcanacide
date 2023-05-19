Shader "Unlit/PuddleDrips"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MyTime ("Time", float) = 0
        _CosSpeed ("Cos Speed", float) = 1
        _Speed ("Ripple Speed", float) = 5
        _XScale ("X Scale", float) = 1
        _YScale ("Y Scale", float) = 1
        _XShift ("X Shift", float) = 0
        _YShift ("Y Shift", float) = 0
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
            float _MyTime;
            float _CosSpeed;
            float _Speed;
            float _XScale;
            float _YScale;
            float _XShift;
            float _YShift;

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

                i.uv.x = (i.uv.x * _XScale - _XScale/2) * 2;
                i.uv.y = (i.uv.y * _YScale - _YScale/2) * 2;

                i.uv.x += _XShift;
                i.uv.y += _YShift;

                float radialDist = length(i.uv);

                float wave = cos(pow(radialDist,0.3) * 80 - _MyTime * _CosSpeed);
                wave = wave * 0.2;

                float sigma = 0.2;
                float mu = _MyTime * _Speed - 0.1;
                float amp = sigma * 2.5;
                float gauss = (1/(sigma * sqrt(2 * 3.14)) * exp(-pow(radialDist - mu, 2)/(2 * pow(sigma,2)))) * amp;
                wave += -gauss + 1;

                wave += radialDist/2;
                return float4(col.xyz + wave, col.a);
            }
            ENDCG
        }
    }
}
