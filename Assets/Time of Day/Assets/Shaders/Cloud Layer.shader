Shader "Time of Day/Cloud Layer"
{
	Properties
	{
		_NoiseTexture1 ("Noise Texture 1 (A)", 2D) = "white" {}
		_NoiseTexture2 ("Noise Texture 2 (A)", 2D) = "white" {}
		_NoiseNormals1 ("Noise Normals 1",     2D) = "bump"  {}
		_NoiseNormals2 ("Noise Normals 2",     2D) = "bump"  {}
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent-470"
			"RenderType"="Transparent"
			"IgnoreProjector"="True"
		}

		Pass
		{
			Cull Front
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

			uniform sampler2D _NoiseTexture1;
			uniform sampler2D _NoiseTexture2;
			uniform sampler2D _NoiseNormals1;
			uniform sampler2D _NoiseNormals2;

			struct v2f {
				float4 position : SV_POSITION;
				float4 color    : TEXCOORD0;
#if FASTEST
				float3 cloudUV  : TEXCOORD1;
#else
				float4 cloudUV  : TEXCOORD1;
#endif
#if BUMPED
				float3 sunDir   : TEXCOORD2;
				float3 moonDir  : TEXCOORD3;
#endif
			};

			v2f vert(appdata_tan v) {
				v2f o;

				o.position = TOD_TRANSFORM_VERT(v.vertex);

				// Vertex position and uv coordinates
				float3 vertnorm = normalize(v.vertex.xyz);
				float2 vertuv   = vertnorm.xz / pow(vertnorm.y + 0.1, 0.75);
				float  vertfade = saturate(100 * vertnorm.y * vertnorm.y);

				// Base color
				o.color.rgb = TOD_CloudColor;
				o.color.a   = TOD_CloudDensity * vertfade;

#if FASTEST
				// UVs
				o.cloudUV.xy = (vertuv + TOD_CloudUV.xy) / TOD_CloudScale.xy;
				o.cloudUV.z  = TOD_CloudSharpness * 0.15 - max(0, 1-TOD_CloudSharpness) * 0.3;
#else
				// UVs
				o.cloudUV.xy = (vertuv + TOD_CloudUV.xy) / TOD_CloudScale.xy;
				o.cloudUV.zw = (vertuv + TOD_CloudUV.zw) / TOD_CloudScale.zw;
#endif

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
				o.color.rgb = TOD_LINEAR2GAMMA(o.color.rgb);
#endif
#endif

				return o;
			}

			fixed4 frag(v2f i) : COLOR {
				fixed4 color = i.color;

#if FASTEST
				// Sample texture
				fixed noise1 = tex2D(_NoiseTexture1, i.cloudUV.xy).a;
				fixed a = (noise1 - i.cloudUV.z);
#else
				// Sample texture
				fixed noise1 = tex2D(_NoiseTexture1, i.cloudUV.xy).a;
				fixed noise2 = tex2D(_NoiseTexture2, i.cloudUV.zw).a;
				fixed a = pow(noise1 * noise2, TOD_CloudSharpness);
#endif

				// Apply texture
				color.a *= saturate(a);

#if BUMPED
				// Sample normals
				fixed4 noiseNormal1 = tex2D(_NoiseNormals1, i.cloudUV.xy);
				fixed4 noiseNormal2 = tex2D(_NoiseNormals2, i.cloudUV.zw);
				fixed3 cloudNormal = UnpackNormal(0.5 * (noiseNormal1 + noiseNormal2));

				// Apply shading
				color.rgb += (0.5 + 0.5 * dot(cloudNormal, i.sunDir))  * TOD_SunCloudColor;
				color.rgb += (0.5 + 0.5 * dot(cloudNormal, i.moonDir)) * TOD_MoonCloudColor;

#if GAMMA
				// Adjust output color
				color.rgb = TOD_LINEAR2GAMMA(color.rgb);
#endif
#endif

				return color;
			}

			ENDCG
		}
	}

	Fallback Off
}
