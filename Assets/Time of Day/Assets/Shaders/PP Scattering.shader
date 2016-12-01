Shader "Hidden/Time of Day/Scattering"
{
	Properties
	{
		_DitheringTexture ("Dithering Lookup Texture (A)", 2D) = "black" {}
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0
			#include "UnityCG.cginc"

			#include "TOD_Base.cginc"
			#include "TOD_Scattering.cginc"

			#define BAYER_DIM 8.0

			uniform sampler2D _DitheringTexture;
			uniform sampler2D _MainTex;
			uniform sampler2D _CameraDepthTexture;

			uniform float4x4 _FrustumCornersWS;
			uniform float4 _MainTex_TexelSize;

			struct v2f {
				float4 position : SV_POSITION;
				float3 viewDir  : TEXCOORD0;
				float4 uv       : TEXCOORD1;
				#if UNITY_UV_STARTS_AT_TOP
				float2 uv1      : TEXCOORD2;
				#endif
			};

			v2f vert(appdata_img v) {
				v2f o;

				int index = (int)v.vertex.z;
				v.vertex.z = 0.1;

				o.position = mul(UNITY_MATRIX_MVP, v.vertex);

				o.uv.xy = v.texcoord.xy;
				o.uv.zw = v.texcoord.xy * _ScreenParams.xy * (1.0 / BAYER_DIM);

				#if UNITY_UV_STARTS_AT_TOP
				o.uv1 = v.texcoord.xy;
				if (_MainTex_TexelSize.y < 0) o.uv1.y = 1-o.uv1.y;
				#endif

				o.viewDir = mul((float3x3)TOD_World2Sky, _FrustumCornersWS[index].xyz);

				return o;
			}

			half4 frag(v2f i) : COLOR {
				#if UNITY_UV_STARTS_AT_TOP
				float depthSample = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv1.xy));
				#else
				float depthSample = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv.xy));
				#endif

				half4 tex = tex2D(_MainTex, i.uv.xy);

				depthSample = Linear01Depth(depthSample);

				float dither = tex2D(_DitheringTexture, i.uv.zw).a * (1.0 / (BAYER_DIM * BAYER_DIM + 1.0));

				half4 scattering = ScatteringColor(normalize(i.viewDir), depthSample);

				if (depthSample == 1)
				{
					return half4(tex.rgb + scattering.rgb + dither, tex.a);
				}
				else
				{
					return half4(lerp(tex.rgb, scattering.rgb + dither, depthSample), tex.a);
				}
			}
			ENDCG
		}
	}

	Fallback Off
}
