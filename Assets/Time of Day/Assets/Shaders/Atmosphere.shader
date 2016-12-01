Shader "Time of Day/Atmosphere"
{
	Properties
	{
		_DitheringTexture ("Dithering Lookup Texture (A)", 2D) = "black" {}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent-490"
			"RenderType"="Transparent"
			"IgnoreProjector"="True"
		}

		Pass
		{
			Cull Front
			ZWrite Off
			ZTest LEqual
			Blend One One
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile LDR HDR
			#pragma multi_compile GAMMA LINEAR
			#pragma multi_compile PER_VERTEX PER_PIXEL
			#include "UnityCG.cginc"
			#include "TOD_Base.cginc"
			#include "TOD_Scattering.cginc"

			#define BAYER_DIM 8.0

			uniform sampler2D _DitheringTexture;

			struct v2f {
				float4 position   : SV_POSITION;
#if PER_VERTEX
				float4 color      : TEXCOORD0;
				float2 frag       : TEXCOORD1;
#else
				float3 inscatter  : TEXCOORD0;
				float3 outscatter : TEXCOORD1;
				float3 viewDir    : TEXCOORD2;
				float2 frag       : TEXCOORD3;
#endif
			};

			v2f vert(appdata_base v) {
				v2f o;

				o.position = TOD_TRANSFORM_VERT(v.vertex);

#if PER_VERTEX
				o.color = ScatteringColor(v.vertex.xyz, 1);
#else
				o.viewDir = v.vertex.xyz;
				ScatteringCoefficients(o.viewDir, 1, o.inscatter, o.outscatter);
#endif

				float4 projPos = ComputeScreenPos(o.position);
				o.frag = projPos.xy / projPos.w * _ScreenParams.xy * (1.0 / BAYER_DIM);

				return o;
			}

			float4 frag(v2f i) : COLOR {
				float dither = tex2D(_DitheringTexture, i.frag).a * (1.0 / (BAYER_DIM * BAYER_DIM + 1.0));

#if PER_VERTEX
				float4 color = dither + i.color;
#else
				float4 color = dither + ScatteringColor(normalize(i.viewDir), i.inscatter, i.outscatter);
#endif

#if LDR
				color = TOD_HDR2LDR(color);
#endif

#if GAMMA
				color = TOD_LINEAR2GAMMA(color);
#endif

				return color;
			}

			ENDCG
		}
	}

	Fallback Off
}
