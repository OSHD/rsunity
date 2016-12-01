Shader "Time of Day/Cloud Billboard"
{
	Properties
	{
		_MainTex ("Alpha Atlas (RGB)", 2D) = "white" {}
		_BumpMap ("Normal Map",        2D) = "bump"  {}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent-460"
			"RenderType"="Transparent"
			"IgnoreProjector"="True"
		}

		Pass
		{
			Cull Back
			ZWrite Off
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile GAMMA LINEAR
			#pragma multi_compile FASTEST DENSITY BUMPED
			#include "UnityCG.cginc"
			#include "TOD_Base.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _BumpMap;

			uniform float4 _MainTex_ST;
			uniform float4 _BumpMap_ST;

			struct v2f {
				float4 position : SV_POSITION;
				half4  color    : TEXCOORD0;
				half4  cloudUV  : TEXCOORD1;
#if BUMPED
				float3 sunDir   : TEXCOORD2;
				float3 moonDir  : TEXCOORD3;
#endif
			};

			v2f vert(appdata_tan v) {
				v2f o;

				o.position = TOD_TRANSFORM_VERT(v.vertex);

				// Base color
				o.color.rgb = TOD_CloudColor;
				o.color.a   = TOD_CloudDensity;

				// UVs
				o.cloudUV.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.cloudUV.zw = TRANSFORM_TEX(v.texcoord, _BumpMap);

#if BUMPED
				// Light directions
				TANGENT_SPACE_ROTATION;
				o.sunDir  = mul(rotation, TOD_LocalSunDirection);
				o.moonDir = mul(rotation, TOD_LocalMoonDirection);
#else
				// Apply shading
				o.color.rgb += (0.5 + 0.5 * dot(v.normal, TOD_LocalSunDirection))  * TOD_SunCloudColor;
				o.color.rgb += (0.5 + 0.5 * dot(v.normal, TOD_LocalMoonDirection)) * TOD_MoonCloudColor;

#if GAMMA
				// Adjust output color
				o.color.rgb = TOD_GAMMA2LINEAR(o.color.rgb);
#endif
#endif

				return o;
			}

			half4 frag(v2f i) : COLOR {
				half4 color = i.color;

#if FASTEST
				// Sample texture
				half3 cloud_alphas = tex2D(_MainTex, i.cloudUV.xy).xyz;
				half a = cloud_alphas.x * cloud_alphas.y;
#else
				// Sample texture
				half3 cloud_alphas = tex2D(_MainTex, i.cloudUV.xy).xyz;
				half a = pow(lerp(lerp(cloud_alphas.x, cloud_alphas.y, saturate(_SinTime.x)), cloud_alphas.z, saturate(-_SinTime.x)), max(1, TOD_CloudSharpness));
#endif

				// Apply texture
				color.a *= saturate(a);

#if BUMPED
				// Sample normals
				half3 cloudNormal = UnpackNormal(tex2D(_BumpMap, i.cloudUV.zw));

				// Apply shading
				color.rgb += (0.5 + 0.5 * dot(cloudNormal, normalize(i.sunDir)))  * TOD_SunCloudColor;
				color.rgb += (0.5 + 0.5 * dot(cloudNormal, normalize(i.moonDir))) * TOD_MoonCloudColor;

#if GAMMA
				// Adjust output color
				color.rgb = TOD_GAMMA2LINEAR(color.rgb);
#endif
#endif

				return color;
			}

			ENDCG
		}
	}

	Fallback Off
}
