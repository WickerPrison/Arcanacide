Shader "Unlit/FloorResetStencil"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
		Tags
		{
			"Queue" = "Background"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ColorMask 0

		Stencil
		{
			Comp Always
            Pass Zero
		}

        Pass
        {

        }
    }
}
