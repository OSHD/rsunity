// Amplify Color - Advanced Color Grading for Unity Pro
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

Shader "Hidden/Amplify Color/Blend" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
	}
	
	Subshader {
		ZTest Always Cull Off ZWrite Off Blend Off
		Fog { Mode off }
		
		// LDR
		Pass {
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "Common.cginc"

				half4 frag( v2f i ) : COLOR
				{
					return apply_blend( tex2D( _MainTex, i.uv ) );
				}
			ENDCG
		}
	
		// HDR
		Pass {
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "Common.cginc"
				
				half4 frag( v2f i ) : COLOR
				{
				#if SHADER_API_D3D9
					return apply_blend( min( tex2D( _MainTex, i.uv ), 1.0 ) );
				#else
					return apply_blend( saturate( tex2D( _MainTex, i.uv ) ) );
				#endif
					
				}
			ENDCG
		}
	}

Fallback off
	
} // shader