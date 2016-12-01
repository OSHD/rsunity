Shader "Time of Day/Space"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "black" {}
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

			uniform sampler2D _MainTex;

			struct v2f {
				float4 position : SV_POSITION;
				float3 viewdir  : TEXCOORD0;
				float3 texcoord : TEXCOORD1;
			};

			v2f vert(appdata_base v) {
				v2f o;

				o.position = TOD_TRANSFORM_VERT(v.vertex);

				float3 worldNormal = normalize(mul((float3x3)_Object2World, v.normal));

				float height   = abs(v.normal.y);
				float heightWS = max(0, worldNormal.y);

				o.texcoord.xy = v.normal.xz / (height + 1) * TOD_SpaceTiling;
				o.texcoord.z  = height * heightWS * TOD_SpaceBrightness;

				o.viewdir = v.normal;

				return o;
			}

			half4 frag(v2f i) : COLOR {
				return half4(tex2D(_MainTex, sign(i.viewdir.y) * i.texcoord.xy).rgb * i.texcoord.z, 1);
			}

			ENDCG
		}
	}

	Fallback Off
}
