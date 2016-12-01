Shader "Time of Day/Cloud Shadow"
{
	Properties
	{
		_NoiseTexture1 ("Noise Texture 1 (A)", 2D) = "white" {}
		_NoiseTexture2 ("Noise Texture 2 (A)", 2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile FASTEST DENSITY BUMPED
			#include "UnityCG.cginc"
			#include "TOD_Base.cginc"

			uniform sampler2D _NoiseTexture1;
			uniform sampler2D _NoiseTexture2;
			uniform float4x4 _Projector;

			struct v2f {
				float4 position : SV_POSITION;
#if FASTEST
				float3 cloudUV  : TEXCOORD0;
#else
				float4 cloudUV  : TEXCOORD0;
#endif
			};

			v2f vert(appdata_base v)
			{
				v2f o;

				o.position = TOD_TRANSFORM_VERT(v.vertex);

				float3 vertnorm = TOD_LocalLightDirection;
				float2 vertuv   = vertnorm.xz / pow(vertnorm.y + 0.1, 0.75);

				float4 projPos  = mul(_Projector, v.vertex);
				float2 uvoffset = 0.5 + projPos.xy / projPos.w;

#if FASTEST
				o.cloudUV.xy = uvoffset + (vertuv + TOD_CloudUV.xy) / TOD_CloudScale.xy;
				o.cloudUV.z  = TOD_CloudSharpness * 0.15 - max(0, 1-TOD_CloudSharpness) * 0.3;
#else
				o.cloudUV.xy = uvoffset + (vertuv + TOD_CloudUV.xy) / TOD_CloudScale.xy;
				o.cloudUV.zw = uvoffset + (vertuv + TOD_CloudUV.zw) / TOD_CloudScale.zw;
#endif

				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
#if FASTEST
				fixed noise1 = tex2D(_NoiseTexture1, i.cloudUV.xy).a;
				fixed a = TOD_CloudDensity * (noise1 - i.cloudUV.z);
#else
				fixed noise1 = tex2D(_NoiseTexture1, i.cloudUV.xy).a;
				fixed noise2 = tex2D(_NoiseTexture2, i.cloudUV.zw).a;
				fixed a = TOD_CloudDensity * pow(noise1 * noise2, TOD_CloudSharpness);
#endif

				return fixed4(0, 0, 0, saturate(a) * TOD_CloudShadow);
			}

			ENDCG
		}
	}

	Fallback Off
}
