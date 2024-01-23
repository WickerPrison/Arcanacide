Shader "Unlit/ScreenDissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
		_DissolveProg ("Dissolve Progression", range(0,1)) = 0
		_Density("Dissolve Density", float) = 0.5
		_EdgeWidth("Edge Width", float) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"= "Transparent"}
        
        Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

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

            sampler2D _DissolveTex;
			float _DissolveProg;
			float _Density;
			float _EdgeWidth;

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

                fixed4 dissolveTex = tex2D(_DissolveTex, i.uv);

                float mask = dissolveTex - _DissolveProg;

                float edgeMask = mask < _EdgeWidth;
                float blackMask = mask > 0;

                float3 outColor = lerp(col.xyz, float3(1,1,1), edgeMask);
                outColor *= blackMask;

                return float4(outColor, 1);
            }
            ENDCG
        }
    }
}
