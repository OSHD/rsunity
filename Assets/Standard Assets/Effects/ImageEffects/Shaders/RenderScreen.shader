Shader "Hidden/RenderScreen" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;

fixed4 frag (v2f_img i) : SV_Target
{	
	fixed4 original = tex2D(_MainTex, i.uv);
	
	// get intensity value (Y part of YIQ color space)
	fixed Y = fixed4(255,255,255,255);//original.rgb;

	fixed4 output = Y;
	
	return output;
}
ENDCG

	}
}

Fallback off

}
