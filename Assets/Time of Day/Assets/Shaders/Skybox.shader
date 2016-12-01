Shader "Time of Day/Skybox"
{
	Properties
	{
	}

	SubShader
	{
		Tags
		{
			"Queue"="Background"
			"RenderType"="Background"
			"PreviewType"="Skybox"
		}

		Pass
		{
			Cull Off
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile LDR HDR
			#pragma multi_compile GAMMA LINEAR
			#include "UnityCG.cginc"
			#include "TOD_Base.cginc"
			#include "TOD_Scattering.cginc"

			struct v2f {
				float4 position : SV_POSITION;
				float4 color    : TEXCOORD0;
			};

			v2f vert(appdata_base v) {
				v2f o;

				o.position = TOD_TRANSFORM_VERT(v.vertex);

				o.color = (v.vertex.y < 0) ? half4(pow(TOD_AmbientColor, TOD_Contrast), 1) : ScatteringColor(v.vertex.xyz, 1);

				return o;
			}

			float4 frag(v2f i) : COLOR {
				float4 color = i.color;

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
