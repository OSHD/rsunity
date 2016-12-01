// Amplify Color - Advanced Color Grading for Unity Pro
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

#ifndef AMPLIFY_COLOR_COMMON3D_INCLUDED
#define AMPLIFY_COLOR_COMMON3D_INCLUDED

#include "Common.cginc"

uniform sampler3D _RgbTex3d;
uniform sampler3D _LerpRgbTex3d;

inline half4 apply3d( half4 color )
{
	return tex3D( _RgbTex3d, color.rgb * 0.96875 + 0.015625 );
}

inline half4 apply3d_blend( half4 color )
{	
	half3 coord = color.rgb * 0.96875 + 0.015625;
	half4 lut1 = tex3D( _RgbTex3d, coord );
	half4 lut2 = tex3D( _LerpRgbTex3d, coord );
	return lerp( lut1, lut2, _lerpAmount );
}

#endif