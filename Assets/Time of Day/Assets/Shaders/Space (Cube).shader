Shader "Time of Day/Space (Cube)"
{
	Properties
	{
		_CubeTex ("Cube (RGB)", Cube) = "black" {}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent-530"
			"RenderType"="Transparent"
			"IgnoreProjector"="True"
		}

		Pass
		{
			Cull Front
			ZWrite Off
			ZTest LEqual
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "TOD_Base.cginc"

			uniform samplerCUBE _CubeTex;

			struct v2f {
				float4 position : SV_POSITION;
				float4 viewdir  : TEXCOORD0;
			};

			v2f vert(appdata_base v) {
				v2f o;

				o.position = TOD_TRANSFORM_VERT(v.vertex);

				float3 worldNormal = normalize(mul((float3x3)_Object2World, v.normal));

				o.viewdir.xyz = v.normal;
				o.viewdir.w   = saturate(worldNormal.y * TOD_SpaceBrightness);

				return o;
			}

			half4 frag(v2f i) : COLOR {
				return half4(texCUBE(_CubeTex, i.viewdir.xyz).rgb * i.viewdir.w, 1);
			}

			ENDCG
		}
	}

	Fallback Off
}
