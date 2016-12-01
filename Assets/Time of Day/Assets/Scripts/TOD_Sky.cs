using UnityEngine;

/// Main sky dome management class.
///
/// Component of the sky dome parent game object.

[ExecuteInEditMode]
[RequireComponent(typeof(TOD_Resources))]
[RequireComponent(typeof(TOD_Components))]
public partial class TOD_Sky : MonoBehaviour
{
	private const float pi  = Mathf.PI;
	private const float tau = Mathf.PI * 2.0f;

	private void UpdateScattering()
	{
		// Phase function
		float g  = -Atmosphere.Directionality;
		float g2 = g * g;

		// Shader paramters
		kBetaMie.x = 1.5f * ((1.0f - g2) / (2.0f + g2));
		kBetaMie.y = 1.0f + g2;
		kBetaMie.z = 2.0f * g;

		const float kWavelength_r = 0.660f;
		const float kWavelength_g = 0.570f;
		const float kWavelength_b = 0.475f;

		const float kWavelength4_r = kWavelength_r * kWavelength_r * kWavelength_r * kWavelength_r;
		const float kWavelength4_g = kWavelength_g * kWavelength_g * kWavelength_g * kWavelength_g;
		const float kWavelength4_b = kWavelength_b * kWavelength_b * kWavelength_b * kWavelength_b;

		const float kInvWavelength4_r = 1.0f / kWavelength4_r;
		const float kInvWavelength4_g = 1.0f / kWavelength4_g;
		const float kInvWavelength4_b = 1.0f / kWavelength4_b;

		float kMie      = 0.0020f * Atmosphere.MieMultiplier;
		float kRayleigh = 0.0020f * Atmosphere.RayleighMultiplier;

		const float kSunBrightness = 40.0f;

		float kKrESun_r = kRayleigh * kSunBrightness * kInvWavelength4_r;
		float kKrESun_g = kRayleigh * kSunBrightness * kInvWavelength4_g;
		float kKrESun_b = kRayleigh * kSunBrightness * kInvWavelength4_b;
		float kKmESun   = kMie * kSunBrightness;

		kSun.x = kKrESun_r;
		kSun.y = kKrESun_g;
		kSun.z = kKrESun_b;
		kSun.w = kKmESun;

		float kKr4PI_r = kRayleigh * 4.0f * pi * kInvWavelength4_r;
		float kKr4PI_g = kRayleigh * 4.0f * pi * kInvWavelength4_g;
		float kKr4PI_b = kRayleigh * 4.0f * pi * kInvWavelength4_b;
		float kKm4PI   = kMie * 4.0f * pi;

		k4PI.x = kKr4PI_r;
		k4PI.y = kKr4PI_g;
		k4PI.z = kKr4PI_b;
		k4PI.w = kKm4PI;

		const float kInnerRadius  = 1.0f;
		const float kInnerRadius2 = kInnerRadius * kInnerRadius;

		const float kOuterRadius  = 1.025f;
		const float kOuterRadius2 = kOuterRadius * kOuterRadius;

		kRadius.x = kInnerRadius;
		kRadius.y = kInnerRadius2;
		kRadius.z = kOuterRadius;
		kRadius.w = kOuterRadius2;

		const float kCameraHeight = 0.0001f;

		const float kScaleFactor = 1.0f / (kOuterRadius - 1.0f);
		const float kScaleDepth = 0.25f;
		const float kScaleOverScaleDepth = kScaleFactor / kScaleDepth;

		kScale.x = kScaleFactor;
		kScale.y = kScaleDepth;
		kScale.z = kScaleOverScaleDepth;
		kScale.w = kCameraHeight;
	}

	private void UpdateCelestials()
	{
		// Celestial computations
		float lst_rad, sun_theta, sun_phi, moon_theta, moon_phi;
		{
			// Local latitude
			float lat_rad = Mathf.Deg2Rad * World.Latitude;
			float lat_sin = Mathf.Sin(lat_rad);
			float lat_cos = Mathf.Cos(lat_rad);

			// Local longitude
			float lon_deg = World.Longitude;

			// Horizon angle
			float horizon_rad = 90f * Mathf.Deg2Rad;

			// Date
			int   year  = Cycle.Year;
			int   month = Cycle.Month;
			int   day   = Cycle.Day;
			float hour  = Cycle.Hour - World.UTC;

			// Time scale
			float d = 367 * year - 7 * (year + (month + 9) / 12) / 4 + 275 * month / 9 + day - 730530 + hour / 24f;

			// Tilt of earth's axis of rotation
			float ecl = 23.4393f - 3.563E-7f * d;
			float ecl_rad = Mathf.Deg2Rad * ecl;
			float ecl_sin = Mathf.Sin(ecl_rad);
			float ecl_cos = Mathf.Cos(ecl_rad);

			// Sun
			{
				// See http://www.stjarnhimlen.se/comp/ppcomp.html#4

				float w = 282.9404f + 4.70935E-5f * d;
				float e = 0.016709f - 1.151E-9f * d;
				float M = 356.0470f + 0.9856002585f * d;

				float M_rad = Mathf.Deg2Rad * M;
				float M_sin = Mathf.Sin(M_rad);
				float M_cos = Mathf.Cos(M_rad);

				// See http://www.stjarnhimlen.se/comp/ppcomp.html#5

				float E_rad = M_rad + e * M_sin * (1f + e * M_cos);
				float E_sin = Mathf.Sin(E_rad);
				float E_cos = Mathf.Cos(E_rad);

				float xv = E_cos - e;
				float yv = Mathf.Sqrt(1f - e*e) * E_sin;

				float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);
				float r = Mathf.Sqrt(xv*xv + yv*yv);

				float l_deg = v + w;
				float l_rad = Mathf.Deg2Rad * l_deg;
				float l_sin = Mathf.Sin(l_rad);
				float l_cos = Mathf.Cos(l_rad);

				float xs = r * l_cos;
				float ys = r * l_sin;

				float xe = xs;
				float ye = ys * ecl_cos;
				float ze = ys * ecl_sin;

				float rasc_rad = Mathf.Atan2(ye, xe);
				float decl_rad = Mathf.Atan2(ze, Mathf.Sqrt(xe*xe + ye*ye));
				float decl_sin = Mathf.Sin(decl_rad);
				float decl_cos = Mathf.Cos(decl_rad);

				// See http://www.stjarnhimlen.se/comp/ppcomp.html#5b

				float Ls = v + w;

				float GMST0_deg = Ls + 180f;
				float GMST_deg  = GMST0_deg + 15f * hour;

				lst_rad = Mathf.Deg2Rad * (GMST_deg + lon_deg);

				// See http://www.stjarnhimlen.se/comp/ppcomp.html#12b

				float HA_rad = lst_rad - rasc_rad;
				float HA_sin = Mathf.Sin(HA_rad);
				float HA_cos = Mathf.Cos(HA_rad);

				float x = HA_cos * decl_cos;
				float y = HA_sin * decl_cos;
				float z = decl_sin;

				float xhor = x * lat_sin - z * lat_cos;
				float yhor = y;
				float zhor = x * lat_cos + z * lat_sin;

				float azimuth  = Mathf.Atan2(yhor, xhor) + Mathf.Deg2Rad * 180f;
				float altitude = Mathf.Atan2(zhor, Mathf.Sqrt(xhor*xhor + yhor*yhor));

				sun_theta = horizon_rad - altitude;
				sun_phi   = azimuth;
			}

			// Moon
			if (Moon.Position == TOD_MoonPositionType.Realistic)
			{
				// See http://www.stjarnhimlen.se/comp/ppcomp.html#4

				float N = 125.1228f - 0.0529538083f * d;
				float i = 5.1454f;
				float w = 318.0634f + 0.1643573223f * d;
				float a = 60.2666f;
				float e = 0.054900f;
				float M = 115.3654f + 13.0649929509f * d;

				float N_rad = Mathf.Deg2Rad * N;
				float N_sin = Mathf.Sin(N_rad);
				float N_cos = Mathf.Cos(N_rad);

				float i_rad = Mathf.Deg2Rad * i;
				float i_sin = Mathf.Sin(i_rad);
				float i_cos = Mathf.Cos(i_rad);

				float M_rad = Mathf.Deg2Rad * M;
				float M_sin = Mathf.Sin(M_rad);
				float M_cos = Mathf.Cos(M_rad);

				// See http://www.stjarnhimlen.se/comp/ppcomp.html#6

				float E_rad = M_rad + e * M_sin * (1f + e * M_cos);
				float E_sin = Mathf.Sin(E_rad);
				float E_cos = Mathf.Cos(E_rad);

				float xv = a * (E_cos - e);
				float yv = a * (Mathf.Sqrt(1f - e*e) * E_sin);

				float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);
				float r = Mathf.Sqrt(xv*xv + yv*yv);

				float l_deg = v + w;
				float l_rad = Mathf.Deg2Rad * l_deg;
				float l_sin = Mathf.Sin(l_rad);
				float l_cos = Mathf.Cos(l_rad);

				// See http://www.stjarnhimlen.se/comp/ppcomp.html#7

				float xh = r * (N_cos * l_cos - N_sin * l_sin * i_cos);
				float yh = r * (N_sin * l_cos + N_cos * l_sin * i_cos);
				float zh = r * (l_sin * i_sin);

				// See http://www.stjarnhimlen.se/comp/ppcomp.html#11

				float xg = xh;
				float yg = yh;
				float zg = zh;

				// See http://www.stjarnhimlen.se/comp/ppcomp.html#12

				float xe = xg;
				float ye = yg * ecl_cos - zg * ecl_sin;
				float ze = yg * ecl_sin + zg * ecl_cos;

				float rasc_rad = Mathf.Atan2(ye, xe);
				float decl_rad = Mathf.Atan2(ze, Mathf.Sqrt(xe*xe + ye*ye));
				float decl_sin = Mathf.Sin(decl_rad);
				float decl_cos = Mathf.Cos(decl_rad);

				// See http://www.stjarnhimlen.se/comp/ppcomp.html#12b

				float HA_rad = lst_rad - rasc_rad;
				float HA_sin = Mathf.Sin(HA_rad);
				float HA_cos = Mathf.Cos(HA_rad);

				float x = HA_cos * decl_cos;
				float y = HA_sin * decl_cos;
				float z = decl_sin;

				float xhor = x * lat_sin - z * lat_cos;
				float yhor = y;
				float zhor = x * lat_cos + z * lat_sin;

				float azimuth  = Mathf.Atan2(yhor, xhor) + Mathf.Deg2Rad * 180f;
				float altitude = Mathf.Atan2(zhor, Mathf.Sqrt(xhor*xhor + yhor*yhor));

				moon_theta = horizon_rad - altitude;
				moon_phi   = azimuth;
			}
			else
			{
				moon_theta = sun_theta - pi;
				moon_phi   = sun_phi;
			}

			SunZenith  = Mathf.Rad2Deg * sun_theta;
			MoonZenith = Mathf.Rad2Deg * moon_theta;
		}

		// Transform updates
		{
			Quaternion spaceRot = Quaternion.Euler(90 - World.Latitude, 0, 0) * Quaternion.Euler(0, World.Longitude, 0) * Quaternion.Euler(0, lst_rad * Mathf.Rad2Deg, 0);

			if (Stars.Position == TOD_StarsPositionType.Rotating)
			{
				#if UNITY_EDITOR
				if (Components.SpaceTransform.localRotation * Vector3.one != spaceRot * Vector3.one)
				#endif
				{
					Components.SpaceTransform.localRotation = spaceRot;
				}
			}
			else
			{
				#if UNITY_EDITOR
				if (Components.SpaceTransform.localRotation != Quaternion.identity)
				#endif
				{
					Components.SpaceTransform.localRotation = Quaternion.identity;
				}
			}

			var sunPos = OrbitalToLocal(sun_theta, sun_phi);

			#if UNITY_EDITOR
			if (Components.SunTransform.localPosition != sunPos)
			#endif
			{
				Components.SunTransform.localPosition = sunPos;
				Components.SunTransform.LookAt(Components.DomeTransform.position, Components.SunTransform.up);
			}

			var moonPos = OrbitalToLocal(moon_theta, moon_phi);

			#if UNITY_EDITOR
			if (Components.MoonTransform.localPosition != moonPos)
			#endif
			{
				var moonFwd = spaceRot * -Vector3.right;
				Components.MoonTransform.localPosition = moonPos;
				Components.MoonTransform.LookAt(Components.DomeTransform.position, moonFwd);
			}

			float sunSize = 2.0f * Mathf.Tan(8.0f * 0.5f * Mathf.Deg2Rad * Sun.MeshSize);
			var sunScale = new Vector3(sunSize, sunSize, sunSize);

			#if UNITY_EDITOR
			if (Components.SunTransform.localScale != sunScale)
			#endif
			{
				Components.SunTransform.localScale = sunScale;
			}

			float moonSize = 2.0f * Mathf.Tan(2.0f * 0.5f * Mathf.Deg2Rad * Moon.MeshSize);
			var moonScale = new Vector3(moonSize, moonSize, moonSize);

			#if UNITY_EDITOR
			if (Components.MoonTransform.localScale != moonScale)
			#endif
			{
				Components.MoonTransform.localScale = moonScale;
			}

			bool sunEnabled = (Components.SunTransform.localPosition.y  > -sunSize);

			#if UNITY_EDITOR
			if (Components.SunRenderer.enabled != sunEnabled)
			#endif
			{
				Components.SunRenderer.enabled = sunEnabled;
			}

			bool moonEnabled = (Components.MoonTransform.localPosition.y > -moonSize);

			#if UNITY_EDITOR
			if (Components.MoonRenderer.enabled != moonEnabled)
			#endif
			{
				Components.MoonRenderer.enabled  = moonEnabled;
			}

			bool cloudEnabled = (Clouds.Density > 0);

			#if UNITY_EDITOR
			if (Components.CloudRenderer.enabled != cloudEnabled)
			#endif
			{
				Components.CloudRenderer.enabled = cloudEnabled;
			}

			bool projEnabled = (Components.ShadowMaterial != null && Clouds.ShadowStrength != 0);

			#if UNITY_EDITOR
			if (Components.ShadowProjector.enabled != projEnabled)
			#endif
			{
				Components.ShadowProjector.enabled = projEnabled;
			}

			bool spaceEnabled = true;

			#if UNITY_EDITOR
			if (Components.SpaceRenderer.enabled != spaceEnabled)
			#endif
			{
				Components.SpaceRenderer.enabled = spaceEnabled;
			}

			bool atmoEnabled = true;

			#if UNITY_EDITOR
			if (Components.AtmosphereRenderer.enabled != atmoEnabled)
			#endif
			{
				Components.AtmosphereRenderer.enabled = atmoEnabled;
			}

			bool clearEnabled = (Components.Rays != null);

			#if UNITY_EDITOR
			if (Components.ClearRenderer.enabled != clearEnabled)
			#endif
			{
				Components.ClearRenderer.enabled = clearEnabled;
			}
		}

		// Color calculations
		{
			// Lerp value
			LerpValue = Mathf.InverseLerp(110f, 80f, SunZenith);

			// Pseudo-time to sample color gradients with
			float time = 1 - LerpValue;

			// Constants
			const float lerpThreshold = 0.1f;
			const float falloffAngle  = 5.0f;

			float dayBrightness   = Day.ColorMultiplier;
			float nightBrightness = Night.ColorMultiplier * 0.25f;

			// Atmospheric condition multipliers
			float clearSky     = (1-Atmosphere.Fogginess);
			float moonAltitude = Mathf.Clamp01((90f - moon_theta * Mathf.Rad2Deg) / falloffAngle);

			// Sun and moon visibility
			float sunVisibility  = Mathf.Clamp01(clearSky * (LerpValue - lerpThreshold) / (1 - lerpThreshold));
			float moonVisibility = Mathf.Clamp01(clearSky * moonAltitude * (lerpThreshold - LerpValue) / lerpThreshold);

			// Sun and moon light colors
			float sunLightColorMult = dayBrightness * sunVisibility;
			SunLightColor = TOD_Util.MulRGB(Day.LightColor.Evaluate(time), sunLightColorMult);

			float moonLightColorMult = nightBrightness * moonVisibility;
			MoonLightColor = TOD_Util.MulRGB(Night.LightColor.Evaluate(time), moonLightColorMult);

			// Ray color
			float sunRayColorMult = dayBrightness * sunVisibility;
			SunRayColor = TOD_Util.MulRGB(Day.RayColor.Evaluate(time), sunRayColorMult);

			float moonRayColorMult = nightBrightness * moonVisibility;
			MoonRayColor = TOD_Util.MulRGB(Night.RayColor.Evaluate(time), moonRayColorMult);

			// Sky color
			float sunSkyColorMult = dayBrightness;
			SunSkyColor = TOD_Util.MulRGB(Day.SkyColor.Evaluate(time), sunSkyColorMult);

			float moonSkyColorMult = nightBrightness;
			MoonSkyColor = TOD_Util.MulRGB(Night.SkyColor.Evaluate(time), moonSkyColorMult);

			// Mesh color
			float sunMeshColorMult = dayBrightness;
			SunMeshColor = TOD_Util.MulRGB(Sun.MeshColor.Evaluate(time), sunMeshColorMult);

			float moonMeshColorMult = nightBrightness;
			MoonMeshColor = TOD_Util.MulRGB(Moon.MeshColor.Evaluate(time), moonMeshColorMult);

			// Cloud color
			float dayCloudColorMult = dayBrightness * dayBrightness * Clouds.Brightness;
			Color dayCloudColor = TOD_Util.MulRGB(Day.CloudColor.Evaluate(time), dayCloudColorMult);

			float nightCloudColorMult = nightBrightness * nightBrightness * Clouds.Brightness;
			Color nightCloudColor = TOD_Util.MulRGB(Night.CloudColor.Evaluate(time), nightCloudColorMult);

			CloudColor = Color.Lerp(nightCloudColor, dayCloudColor, LerpValue);

			// Ambient color
			float dayAmbientColorMult = dayBrightness * Day.AmbientMultiplier;
			Color dayAmbientColor = TOD_Util.MulRGB(Day.AmbientColor.Evaluate(time), dayAmbientColorMult);

			float nightAmbientColorMult = nightBrightness * Night.AmbientMultiplier;
			Color nightAmbientColor = TOD_Util.MulRGB(Night.AmbientColor.Evaluate(time), nightAmbientColorMult);

			AmbientColor = Color.Lerp(nightAmbientColor, dayAmbientColor, LerpValue);

			// Moon halo color
			float moonHaloMult = nightBrightness * moonAltitude;
			MoonHaloColor = TOD_Util.MulRGB(Moon.HaloColor.Evaluate(time), moonHaloMult);

			// Light source parameters
			float lightIntensity;
			float lightShadowStrength;
			Color lightColor;

			if (LerpValue > lerpThreshold)
			{
				IsDay = true; IsNight = false;

				lightShadowStrength = Day.ShadowStrength;
				lightIntensity = Mathf.Lerp(0, Day.LightIntensity, sunVisibility);
				lightColor = SunLightColor;
				RayColor = SunRayColor;
			}
			else
			{
				IsDay = false; IsNight = true;

				lightShadowStrength = Night.ShadowStrength;
				lightIntensity = Mathf.Lerp(0, Night.LightIntensity, moonVisibility);
				lightColor = MoonLightColor;
				RayColor = MoonRayColor;
			}

			#if UNITY_EDITOR
			if (Components.LightSource.color != lightColor)
			#endif
			{
				Components.LightSource.color = lightColor;
			}

			#if UNITY_EDITOR
			if (Components.LightSource.intensity != lightIntensity)
			#endif
			{
				Components.LightSource.intensity = lightIntensity;
			}

			#if UNITY_EDITOR
			if (Components.LightSource.shadowStrength != lightShadowStrength)
			#endif
			{
				Components.LightSource.shadowStrength = lightShadowStrength;
			}
		}

		// Light source position
		if (!Application.isPlaying || timeSinceLightUpdate >= Light.UpdateInterval)
		{
			timeSinceLightUpdate = 0;

			var position = IsNight
			             ? OrbitalToLocal(Mathf.Min(moon_theta, (1 - Light.MinimumHeight) * pi/2), moon_phi)
			             : OrbitalToLocal(Mathf.Min(sun_theta,  (1 - Light.MinimumHeight) * pi/2), sun_phi);

			#if UNITY_EDITOR
			if (Components.LightTransform.localPosition != position)
			#endif
			{
				Components.LightTransform.localPosition = position;
				Components.LightTransform.LookAt(Components.DomeTransform.position);
			}
		}
		else
		{
			timeSinceLightUpdate += Time.deltaTime;
		}

		// Direction vectors
		{
			SunDirection = -Components.SunTransform.forward;
			LocalSunDirection = Components.DomeTransform.InverseTransformDirection(SunDirection);

			MoonDirection = -Components.MoonTransform.forward;
			LocalMoonDirection = Components.DomeTransform.InverseTransformDirection(MoonDirection);

			LightDirection = Vector3.Lerp(MoonDirection, SunDirection, LerpValue * LerpValue);
			LocalLightDirection = Components.DomeTransform.InverseTransformDirection(LightDirection);
		}
	}
}
