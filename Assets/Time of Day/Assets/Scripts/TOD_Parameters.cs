#if UNITY_4_0||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
#define UNITY_4
#endif

using UnityEngine;
#if !UNITY_4
using UnityEngine.Rendering;
#endif
using System;

/// All parameters of the sky dome.
[Serializable] public class TOD_Parameters
{
	public TOD_CycleParameters      Cycle;
	public TOD_WorldParameters      World;
	public TOD_AtmosphereParameters Atmosphere;
	public TOD_DayParameters        Day;
	public TOD_NightParameters      Night;
	public TOD_SunParameters        Sun;
	public TOD_MoonParameters       Moon;
	public TOD_LightParameters      Light;
	public TOD_StarParameters       Stars;
	public TOD_CloudParameters      Clouds;
	public TOD_FogParameters        Fog;
	public TOD_AmbientParameters    Ambient;
	public TOD_ReflectionParameters Reflection;

	public TOD_Parameters()
	{
	}

	public TOD_Parameters(TOD_Sky sky)
	{
		Cycle      = sky.Cycle;
		World      = sky.World;
		Atmosphere = sky.Atmosphere;
		Day        = sky.Day;
		Night      = sky.Night;
		Sun        = sky.Sun;
		Moon       = sky.Moon;
		Light      = sky.Light;
		Stars      = sky.Stars;
		Clouds     = sky.Clouds;
		Fog        = sky.Fog;
		Ambient    = sky.Ambient;
		Reflection = sky.Reflection;
	}

	public void ToSky(TOD_Sky sky)
	{
		sky.Cycle      = Cycle;
		sky.World      = World;
		sky.Atmosphere = Atmosphere;
		sky.Day        = Day;
		sky.Night      = Night;
		sky.Sun        = Sun;
		sky.Moon       = Moon;
		sky.Light      = Light;
		sky.Stars      = Stars;
		sky.Clouds     = Clouds;
		sky.Fog        = Fog;
		sky.Ambient    = Ambient;
		sky.Reflection = Reflection;
	}
}

/// Parameters of the day and night cycle.
[Serializable] public class TOD_CycleParameters
{
	/// [0, 24]
	/// Current hour of the day.
	[Tooltip("Current hour of the day.")]
	public float Hour = 12;

	/// [1, 31]
	/// Current day of the month.
	[Tooltip("Current day of the month.")]
	public int Day = 15;

	/// [1, 12]
	/// Current month of the year.
	[Tooltip("Current month of the year.")]
	public int Month = 6;

	/// [1, 9999]
	/// Current year.
	[Tooltip("Current year.")]
	[TOD_Range(1, 9999)] public int Year = 2000;

	/// All time information as a System.DateTime instance.
	public System.DateTime DateTime
	{
		get
		{
			var res = new DateTime(0, DateTimeKind.Utc);
			return res.AddYears(Year-1).AddMonths(Month-1).AddDays(Day-1).AddHours(Hour);
		}
		set
		{
			Year  = value.Year;
			Month = value.Month;
			Day   = value.Day;
			Hour  = value.Hour + value.Minute / 60f + value.Second / 3600f + value.Millisecond / 3600000f;
		}
	}

	/// All time information as a single long.
	/// Value corresponds to the System.DateTime.Ticks property.
	public long Ticks
	{
		get
		{
			return DateTime.Ticks;
		}
		set
		{
			DateTime = new System.DateTime(value, DateTimeKind.Utc);
		}
	}
}

/// Parameters of the world.
[Serializable] public class TOD_WorldParameters
{
	/// [-90, +90]
	/// Latitude of the current location in degrees.
	[Tooltip("Latitude of the current location in degrees.")]
	[Range(-90f, +90f)] public float Latitude = 0;

	/// [-180, +180]
	/// Longitude of the current location in degrees.
	[Tooltip("Longitude of the current location in degrees.")]
	[Range(-180f, +180f)] public float Longitude = 0;

	/// [-14, +14]
	/// UTC/GMT time zone of the current location in hours.
	[Tooltip("UTC/GMT time zone of the current location in hours.")]
	[Range(-14f, +14f)] public float UTC = 0;
}

/// Parameters of the atmosphere.
[Serializable] public class TOD_AtmosphereParameters
{
	/// [0, &infin;]
	/// Intensity of the atmospheric Rayleigh scattering.
	[Tooltip("Intensity of the atmospheric Rayleigh scattering.")]
	[TOD_Min(0f)] public float RayleighMultiplier = 1.0f;

	/// [0, &infin;]
	/// Intensity of the atmospheric Mie scattering.
	[Tooltip("Intensity of the atmospheric Mie scattering.")]
	[TOD_Min(0f)] public float MieMultiplier = 1.0f;

	/// [0, &infin;]
	/// Overall brightness of the atmosphere.
	[Tooltip("Overall brightness of the atmosphere.")]
	[TOD_Min(0f)] public float Brightness = 1.5f;

	/// [0, &infin;]
	/// Overall contrast of the atmosphere.
	[Tooltip("Overall contrast of the atmosphere.")]
	[TOD_Min(0f)] public float Contrast = 1.5f;

	/// [0, 1]
	/// Directionality factor that determines the size and sharpness of the glow around the sun.
	[Tooltip("Directionality factor that determines the size and sharpness of the glow around the light source.")]
	[TOD_Range(0f, 1f)] public float Directionality = 0.7f;

	/// [0, 1]
	/// Density of the fog covering the sky.
	[Tooltip("Density of the fog covering the sky.")]
	[TOD_Range(0f, 1f)] public float Fogginess = 0.0f;
}

/// Parameters that are unique to the day.
[Serializable] public class TOD_DayParameters
{
	/// Color of the light that hits the atmosphere.
	[Tooltip("Color of the light that hits the atmosphere.\nInterpolates from left (day) to right (night).")]
	public Gradient SkyColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(255, 243, 234, 255), 0.0f),
			new GradientColorKey(new Color32(255, 243, 234, 255), 1.0f)
		}
	};

	/// Color of the light that hits the ground.
	[Tooltip("Color of the light that hits the ground.\nInterpolates from left (day) to right (night).")]
	public Gradient LightColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(255, 243, 234, 255), 0.0f),
			new GradientColorKey(new Color32(255, 107, 000, 255), 1.0f)
		}
	};

	/// Color of the god rays.
	[Tooltip("Color of the god rays.\nInterpolates from left (day) to right (night).")]
	public Gradient RayColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(255, 243, 234, 255), 0.0f),
			new GradientColorKey(new Color32(255, 107, 000, 255), 1.0f)
		}
	};

	/// Color of the clouds.
	[Tooltip("Color of the clouds.\nInterpolates from left (day) to right (night).")]
	public Gradient CloudColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(255, 255, 255, 255), 0.0f),
			new GradientColorKey(new Color32(255, 200, 100, 255), 1.0f)
		}
	};

	/// Color of the ambient light.
	[Tooltip("Color of the ambient light.\nInterpolates from left (day) to right (night).")]
	public Gradient AmbientColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(094, 089, 087, 255), 0.0f),
			new GradientColorKey(new Color32(094, 089, 087, 255), 1.0f)
		}
	};

	/// [0, &infin;]
	/// Intensity of the light source.
	[Tooltip("Intensity of the light source.")]
	[TOD_Min(0f)] public float LightIntensity = 1.0f;

	/// [0, 1]
	/// Opacity of the shadows dropped by the light source.
	[Tooltip("Opacity of the shadows dropped by the light source.")]
	[TOD_Range(0f, 1f)] public float ShadowStrength = 1.0f;

	/// [0, 1]
	/// Brightness of colors.
	[Tooltip("Brightness of colors.")]
	[Range(0f, 1f)] public float ColorMultiplier = 1.0f;

	/// [0, 1]
	/// Brightness of ambient light.
	[Tooltip("Brightness of ambient light.")]
	[Range(0f, 1f)] public float AmbientMultiplier = 1.0f;

	/// [0, 1]
	/// Brightness of reflected light.
	[Tooltip("Brightness of reflected light.")]
	[Range(0f, 1f)] public float ReflectionMultiplier = 1.0f;
}

/// Parameters that are unique to the night.
[Serializable] public class TOD_NightParameters
{
	/// Color of the light that hits the atmosphere.
	[Tooltip("Color of the light that hits the atmosphere.\nInterpolates from left (day) to right (night).")]
	public Gradient SkyColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(025, 040, 065, 255), 0.0f),
			new GradientColorKey(new Color32(025, 040, 065, 255), 1.0f)
		}
	};

	/// Color of the light that hits the ground.
	[Tooltip("Color of the light that hits the ground.\nInterpolates from left (day) to right (night).")]
	public Gradient LightColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(025, 040, 065, 255), 0.0f),
			new GradientColorKey(new Color32(025, 040, 065, 255), 1.0f)
		}
	};

	/// Color of the god rays.
	[Tooltip("Color of the god rays.\nInterpolates from left (day) to right (night).")]
	public Gradient RayColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(025, 040, 065, 255), 0.0f),
			new GradientColorKey(new Color32(025, 040, 065, 255), 1.0f)
		}
	};

	/// Color of the clouds.
	[Tooltip("Color of the clouds.\nInterpolates from left (day) to right (night).")]
	public Gradient CloudColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
			},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(025, 040, 065, 255), 0.0f),
			new GradientColorKey(new Color32(025, 040, 065, 255), 1.0f)
		}
	};

	/// Color of the ambient light.
	[Tooltip("Color of the ambient light.\nInterpolates from left (day) to right (night).")]
	public Gradient AmbientColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(025, 040, 065, 255), 0.0f),
			new GradientColorKey(new Color32(025, 040, 065, 255), 1.0f)
		}
	};

	/// [0, &infin;]
	/// Intensity of the light source.
	[Tooltip("Intensity of the light source.")]
	[TOD_Min(0f)] public float LightIntensity = 0.1f;

	/// [0, 1]
	/// Opacity of the shadows dropped by the light source.
	[Tooltip("Opacity of the shadows dropped by the light source.")]
	[TOD_Range(0f, 1f)] public float ShadowStrength = 1.0f;

	/// [0, 1]
	/// Brightness of colors.
	[Tooltip("Brightness of colors.")]
	[Range(0f, 1f)] public float ColorMultiplier = 1.0f;

	/// [0, 1]
	/// Brightness of ambient light.
	[Tooltip("Brightness of ambient light.")]
	[Range(0f, 1f)] public float AmbientMultiplier = 1.0f;

	/// [0, 1]
	/// Brightness of reflected light.
	[Tooltip("Brightness of reflected light.")]
	[Range(0f, 1f)] public float ReflectionMultiplier = 1.0f;
}

/// Parameters that are unique to the sun.
[Serializable] public class TOD_SunParameters
{
	/// Color of the sun spot.
	[Tooltip("Color of the sun spot.\nInterpolates from left (day) to right (night).")]
	public Gradient MeshColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(253, 171, 050, 255), 0.0f),
			new GradientColorKey(new Color32(253, 171, 050, 255), 1.0f)
		}
	};

	/// [0, &infin;]
	/// Size of the sun spot in degrees.
	[Tooltip("Size of the sun spot in degrees.")]
	[TOD_Min(0f)] public float MeshSize = 1.0f;

	/// [0, &infin;]
	/// Brightness of the sun spot.
	[Tooltip("Brightness of the sun spot.")]
	[TOD_Min(0f)] public float MeshBrightness = 1.0f;

	/// [0, &infin;]
	/// Contrast of the sun spot.
	[Tooltip("Contrast of the sun spot.")]
	[TOD_Min(0f)] public float MeshContrast = 1.0f;
}

/// Parameters that are unique to the moon.
[Serializable] public class TOD_MoonParameters
{
	/// Color of the moon mesh.
	[Tooltip("Color of the moon mesh.\nInterpolates from left (day) to right (night).")]
	public Gradient MeshColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(255, 233, 200, 255), 0.0f),
			new GradientColorKey(new Color32(255, 233, 200, 255), 1.0f)
		}
	};

	/// [0, &infin;]
	/// Size of the moon mesh in degrees.
	[Tooltip("Size of the moon mesh in degrees.")]
	[TOD_Min(0f)] public float MeshSize = 1.0f;

	/// [0, &infin;]
	/// Brightness of the moon mesh.
	[Tooltip("Brightness of the moon mesh.")]
	[TOD_Min(0f)] public float MeshBrightness = 1.0f;

	/// [0, &infin;]
	/// Contrast of the moon mesh.
	[Tooltip("Contrast of the moon mesh.")]
	[TOD_Min(0f)] public float MeshContrast = 1.0f;

	/// Color of the moon halo.
	[Tooltip("Color of the moon halo.\nInterpolates from left (day) to right (night).")]
	public Gradient HaloColor = new Gradient()
	{
		alphaKeys = new GradientAlphaKey[] {
			new GradientAlphaKey(1.0f, 0.0f),
			new GradientAlphaKey(1.0f, 1.0f)
		},
		colorKeys = new GradientColorKey[] {
			new GradientColorKey(new Color32(025, 040, 065, 255), 0.0f),
			new GradientColorKey(new Color32(025, 040, 065, 255), 1.0f)
		}
	};

	/// [0, &infin;]
	/// Size of the moon halo.
	[Tooltip("Size of the moon halo.")]
	[TOD_Min(0f)] public float HaloSize = 0.1f;

	/// Type of the moon position calculation.
	[Tooltip("Type of the moon position calculation.")]
	public TOD_MoonPositionType Position = TOD_MoonPositionType.Realistic;
}

/// Parameters of the stars.
[Serializable] public class TOD_StarParameters
{
	/// [0, &infin;]
	/// Texture tiling of the stars texture.
	[Tooltip("Texture tiling of the stars texture.")]
	[TOD_Min(0f)] public float Tiling = 6.0f;

	/// [0, &infin;]
	/// Brightness of the stars.
	[Tooltip("Brightness of the stars.")]
	[TOD_Min(0f)] public float Brightness = 3.0f;

	/// Type of the stars position calculation.
	[Tooltip("Type of the stars position calculation.")]
	public TOD_StarsPositionType Position = TOD_StarsPositionType.Rotating;
}

/// Parameters of the clouds.
[Serializable] public class TOD_CloudParameters
{
	/// [0, 1]
	/// Density of the clouds.
	[Tooltip("Density of the clouds.")]
	[TOD_Range(0f, 1f)] public float Density = 1.0f;

	/// [0, &infin;]
	/// Sharpness of the clouds.
	[Tooltip("Sharpness of the clouds.")]
	[TOD_Min(0f)] public float Sharpness = 3.0f;

	/// [0, &infin;]
	/// Brightness of the clouds.
	[Tooltip("Brightness of the clouds.")]
	[TOD_Min(0f)] public float Brightness = 1.0f;

	/// [0, &infin;]
	/// Number of billboard clouds to instantiate at start.
	/// Billboard clouds are not visible in edit mode.
	[Tooltip("Number of billboard clouds to instantiate at start.\nBillboard clouds are not visible in edit mode.")]
	[TOD_Min(0f)] public int Billboards = 0;

	/// [0, 1]
	/// Opacity of the cloud shadows.
	[Tooltip("Opacity of the cloud shadows.")]
	[TOD_Range(0f, 1f)] public float ShadowStrength = 0.0f;

	/// Scale of the first cloud layer.
	[Tooltip("Scale of the first cloud layer.")]
	public Vector2 Scale1 = new Vector2(3, 3);

	/// Scale of the second cloud layer.
	[Tooltip("Scale of the second cloud layer.")]
	public Vector2 Scale2 = new Vector2(7, 7);
}

/// Parameters of the light source.
[Serializable] public class TOD_LightParameters
{
	/// [0, &infin;]
	/// Refresh interval of the light source position in seconds.
	[Tooltip("Refresh interval of the light source position in seconds.")]
	[TOD_Min(0f)] public float UpdateInterval = 0.0f;

	/// [-1, 1]
	/// Controls how low the light source is allowed to go.
	/// \n = -1 light source can go as low as it wants.
	/// \n = 0 light source will never go below the horizon.
	/// \n = +1 light source will never leave zenith.
	[Tooltip("Controls how low the light source is allowed to go.")]
	[TOD_Range(-1f, 1f)] public float MinimumHeight = 0.0f;
}

/// Parameters of the fog mode.
[Serializable] public class TOD_FogParameters
{
	/// Fog color mode.
	[Tooltip("Fog color mode.")]
	public TOD_FogType Mode = TOD_FogType.Color;

	/// [0, 1]
	/// Fog color sampling height.
	/// \n = 0 fog is atmosphere color at horizon.
	/// \n = 1 fog is atmosphere color at zenith.
	[Tooltip("Fog color sampling height.")]
	[TOD_Range(0f, 1f)] public float HeightBias = 0.0f;
}

/// Parameters of the ambient mode.
[Serializable] public class TOD_AmbientParameters
{
	/// Ambient light mode.
	[Tooltip("Ambient light mode.")]
	public TOD_AmbientType Mode = TOD_AmbientType.Color;

	/// Refresh interval of the ambient light probe in seconds.
	[Tooltip("Refresh interval of the ambient light probe in seconds.")]
	[TOD_Min(0f)] public float UpdateInterval = 1.0f;
}

/// Parameters of the reflection mode.
[Serializable] public class TOD_ReflectionParameters
{
	/// Reflection probe mode.
	[Tooltip("Reflection probe mode.")]
	public TOD_ReflectionType Mode = TOD_ReflectionType.None;

	#if !UNITY_4

	/// Clear flags to use for the reflection.
	[Tooltip("Clear flags to use for the reflection.")]
	public ReflectionProbeClearFlags ClearFlags = ReflectionProbeClearFlags.Skybox;

	/// Layers to include in the reflection.
	[Tooltip("Layers to include in the reflection.")]
	public LayerMask CullingMask = 0;

	/// Time slicing behaviour to spread out rendering cost over multiple frames.
	[Tooltip("Time slicing behaviour to spread out rendering cost over multiple frames.")]
	public ReflectionProbeTimeSlicingMode TimeSlicing = ReflectionProbeTimeSlicingMode.AllFacesAtOnce;

	#endif

	/// Refresh interval of the reflection cubemap in seconds.
	[Tooltip("Refresh interval of the reflection cubemap in seconds.")]
	[TOD_Min(0f)] public float UpdateInterval = 1.0f;
}
