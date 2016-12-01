#ifndef TOD_BASE_INCLUDED
#define TOD_BASE_INCLUDED

#include "UnityCG.cginc"

uniform float4x4 TOD_World2Sky;
uniform float4x4 TOD_Sky2World;

uniform float3 TOD_SunSkyColor;
uniform float3 TOD_MoonSkyColor;

uniform float3 TOD_SunCloudColor;
uniform float3 TOD_MoonCloudColor;

uniform float3 TOD_SunMeshColor;
uniform float3 TOD_MoonMeshColor;

uniform float3 TOD_CloudColor;
uniform float3 TOD_AmbientColor;
uniform float3 TOD_MoonHaloColor;

uniform float3 TOD_SunDirection;
uniform float3 TOD_MoonDirection;
uniform float3 TOD_LightDirection;

uniform float3 TOD_LocalSunDirection;
uniform float3 TOD_LocalMoonDirection;
uniform float3 TOD_LocalLightDirection;

uniform float TOD_Contrast;
uniform float TOD_Brightness;
uniform float TOD_Fogginess;
uniform float TOD_MoonHaloPower;

uniform float TOD_CloudDensity;
uniform float TOD_CloudSharpness;
uniform float TOD_CloudShadow;
uniform float4 TOD_CloudScale;
uniform float4 TOD_CloudUV;

uniform float TOD_SpaceTiling;
uniform float TOD_SpaceBrightness;

uniform float TOD_SunMeshContrast;
uniform float TOD_SunMeshBrightness;

uniform float TOD_MoonMeshContrast;
uniform float TOD_MoonMeshBrightness;

uniform float3 TOD_kBetaMie;
uniform float4 TOD_kSun;
uniform float4 TOD_k4PI;
uniform float4 TOD_kRadius;
uniform float4 TOD_kScale;

#define TOD_TRANSFORM_VERT(vert) mul(UNITY_MATRIX_MVP, vert)

// Fast and simple tonemapping
#define TOD_HDR2LDR(color) (1.0 - exp2(-TOD_Brightness * color))

// Approximates gamma by 2.0 instead of 2.2
#define TOD_GAMMA2LINEAR(color) (color * color)
#define TOD_LINEAR2GAMMA(color) sqrt(color)

#endif
