Shader "Unlit/ObjectColorChange" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}

    // My Properties 
    _OldColor ("existing color", color) = (1,1,1,1)
    _NewColor ("desired color", color) = (1,1,1,1)
    _Threshold ("color threshold", float) = 1
    _BlackToWhite ("Black to White", float) = 0
}

SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 100

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            //My Properties
            float4 _NewColor;
            float4 _OldColor;
            float _Threshold;
            bool _BlackToWhite;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord);

                // My additions
                float3 threshold = float3(_Threshold, _Threshold, _Threshold);
                float3 diff = length(abs((normalize(col.xyz) - normalize(_OldColor.xyz)))) < length(threshold);
                float3 outColor = lerp(col.xyz, _NewColor.xyz, diff);
                // End my additions

                UNITY_OPAQUE_ALPHA(col.a);
                return float4(outColor, col.a);
            }
        ENDCG
    }
}

}
