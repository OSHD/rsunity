#if UNITY_4_0||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
#define UNITY_4
#endif

using UnityEngine;
#if !UNITY_4
using UnityEngine.Rendering;
#endif
using System.Collections.Generic;

public partial class TOD_Sky : MonoBehaviour
{
	private static List<TOD_Sky> instances = new List<TOD_Sky>();

	#if !UNITY_4
	private int probeRenderID = -1;
	#endif

	//
	// Static properties
	//

	/// All currently active sky dome instances.
	public static List<TOD_Sky> Instances
	{
		get
		{
			return instances;
		}
	}

	/// The most recently created sky dome instance.
	public static TOD_Sky Instance
	{
		get
		{
			return instances.Count == 0 ? null : instances[instances.Count-1];
		}
	}

	//
	// Inspector variables
	//

	/// The color space.
	public TOD_ColorSpaceType ColorSpace = TOD_ColorSpaceType.Auto;

	/// The color range.
	public TOD_ColorRangeType ColorRange = TOD_ColorRangeType.Auto;

	/// The sky quality.
	public TOD_SkyQualityType SkyQuality = TOD_SkyQualityType.PerVertex;

	/// The cloud quality.
	public TOD_CloudQualityType CloudQuality = TOD_CloudQualityType.Bumped;

	/// The mesh quality.
	public TOD_MeshQualityType MeshQuality = TOD_MeshQualityType.High;

	/// Parameters of the day and night cycle.
	public TOD_CycleParameters Cycle;

	/// Parameters of the world.
	public TOD_WorldParameters World;

	/// Parameters of the atmosphere.
	public TOD_AtmosphereParameters Atmosphere;

	/// Parameters of the day.
	public TOD_DayParameters Day;

	/// Parameters of the night.
	public TOD_NightParameters Night;

	/// Parameters of the sun.
	public TOD_SunParameters Sun;

	/// Parameters of the moon.
	public TOD_MoonParameters Moon;

	/// Parameters of the stars.
	public TOD_StarParameters Stars;

	/// Parameters of the cloud layers.
	public TOD_CloudParameters Clouds;

	/// Parameters of the light source.
	public TOD_LightParameters Light;

	/// Parameters of the fog.
	public TOD_FogParameters Fog;

	/// Parameters of the ambient light.
	public TOD_AmbientParameters Ambient;

	/// Parameters of the reflection cubemap.
	public TOD_ReflectionParameters Reflection;

	//
	// Class properties
	//

	/// Whether or not the sky dome was successfully initialized.
	internal bool Initialized
	{
		get; private set;
	}

	/// Whether or not the sky dome is running in headless mode.
	internal bool Headless
	{
		#if UNITY_EDITOR
		get { return false; }
		#else
		get { return Camera.allCamerasCount == 0; }
		#endif
	}

	/// Containins references to all components.
	internal TOD_Components Components
	{
		get; private set;
	}

	/// Containins references to all resources.
	internal TOD_Resources Resources
	{
		get; private set;
	}

	/// Boolean to check if it is day.
	internal bool IsDay
	{
		get; private set;
	}

	/// Boolean to check if it is night.
	internal bool IsNight
	{
		get; private set;
	}

	/// Radius of the sky dome.
	internal float Radius
	{
		get { return Components.DomeTransform.lossyScale.y; }
	}

	/// Diameter of the sky dome.
	internal float Diameter
	{
		get { return Components.DomeTransform.lossyScale.y * 2; }
	}

	/// Falls off the darker the sunlight gets.
	/// Can for example be used to lerp between day and night values in shaders.
	/// \n = +1 at day
	/// \n = 0 at night
	internal float LerpValue
	{
		get; private set;
	}

	/// Sun zenith angle in degrees.
	/// \n = 0   if the sun is exactly at zenith.
	/// \n = 180 if the sun is exactly below the ground.
	internal float SunZenith
	{
		get; private set;
	}

	/// Moon zenith angle in degrees.
	/// \n = 0   if the moon is exactly at zenith.
	/// \n = 180 if the moon is exactly below the ground.
	internal float MoonZenith
	{
		get; private set;
	}

	/// Currently active light source zenith angle in degrees.
	/// \n = 0  if the currently active light source (sun or moon) is exactly at zenith.
	/// \n = 90 if the currently active light source (sun or moon) is exactly at the horizon.
	internal float LightZenith
	{
		get { return Mathf.Min(SunZenith, MoonZenith); }
	}

	/// Current light intensity.
	internal float LightIntensity
	{
		get { return Components.LightSource.intensity; }
	}

	/// Sun direction vector in world space.
	internal Vector3 SunDirection
	{
		get; private set;
	}

	/// Moon direction vector in world space.
	internal Vector3 MoonDirection
	{
		get; private set;
	}

	/// Current directional light vector in world space.
	/// Lerps between TOD_Sky.SunDirection and TOD_Sky.MoonDirection at dusk and dawn.
	internal Vector3 LightDirection
	{
		get; private set;
	}

	/// Sun direction vector in sky dome object space.
	internal Vector3 LocalSunDirection
	{
		get; private set;
	}

	/// Moon direction vector in sky dome object space.
	internal Vector3 LocalMoonDirection
	{
		get; private set;
	}

	/// Current directional light vector in sky dome object space.
	/// Lerps between TOD_Sky.LocalSunDirection and TOD_Sky.LocalMoonDirection at dusk and dawn.
	internal Vector3 LocalLightDirection
	{
		get; private set;
	}

	/// Current sun light color.
	internal Color SunLightColor
	{
		get; private set;
	}

	/// Current moon light color.
	internal Color MoonLightColor
	{
		get; private set;
	}

	/// Current light color.
	/// The color of TOD_Sky.Components.LightSource.
	/// Lerps between TOD_Sky.SunLightColor and TOD_Sky.MoonLightColor at dusk and dawn.
	internal Color LightColor
	{
		get { return Components.LightSource.color; }
	}

	/// Current sun ray color.
	internal Color SunRayColor
	{
		get; private set;
	}

	/// Current moon ray color.
	internal Color MoonRayColor
	{
		get; private set;
	}

	/// Current ray color.
	/// Lerps between TOD_Sky.SunRayColor and TOD_Sky.MoonRayColor at dusk and dawn.
	internal Color RayColor
	{
		get; private set;
	}

	/// Current sun sky color.
	internal Color SunSkyColor
	{
		get; private set;
	}

	/// Current moon sky color.
	internal Color MoonSkyColor
	{
		get; private set;
	}

	/// Current sun mesh color.
	internal Color SunMeshColor
	{
		get; private set;
	}

	/// Current moon mesh color.
	internal Color MoonMeshColor
	{
		get; private set;
	}

	/// Current cloud color.
	internal Color CloudColor
	{
		get; private set;
	}

	/// Current ambient light color.
	internal Color AmbientColor
	{
		get; private set;
	}

	/// Current moon halo color.
	internal Color MoonHaloColor
	{
		get; private set;
	}

	#if !UNITY_4
	/// Current reflection probe.
	internal ReflectionProbe Probe
	{
		get; private set;
	}
	#endif

	//
	// Class methods
	//

	/// Convert spherical coordinates to cartesian coordinates.
	/// \param radius Spherical coordinates radius.
	/// \param theta Spherical coordinates theta.
	/// \param phi Spherical coordinates phi.
	/// \return Unity position in world space.
	internal Vector3 OrbitalToUnity(float radius, float theta, float phi)
	{
		Vector3 res;

		float sinTheta = Mathf.Sin(theta);
		float cosTheta = Mathf.Cos(theta);
		float sinPhi   = Mathf.Sin(phi);
		float cosPhi   = Mathf.Cos(phi);

		res.z = radius * sinTheta * cosPhi;
		res.y = radius * cosTheta;
		res.x = radius * sinTheta * sinPhi;

		return res;
	}

	/// Convert spherical coordinates to cartesian coordinates.
	/// \param theta Spherical coordinates theta.
	/// \param phi Spherical coordinates phi.
	/// \return Unity position in local space.
	internal Vector3 OrbitalToLocal(float theta, float phi)
	{
		Vector3 res;

		float sinTheta = Mathf.Sin(theta);
		float cosTheta = Mathf.Cos(theta);
		float sinPhi   = Mathf.Sin(phi);
		float cosPhi   = Mathf.Cos(phi);

		res.z = sinTheta * cosPhi;
		res.y = cosTheta;
		res.x = sinTheta * sinPhi;

		return res;
	}

	/// Sample atmosphere colors from the sky dome.
	/// \param direction View direction in world space.
	/// \param directLight Whether or not to include direct light.
	/// \return Color of the atmosphere in the specified direction.
	internal Color SampleAtmosphere(Vector3 direction, bool directLight = true)
	{
		Vector3 dir = Components.DomeTransform.InverseTransformDirection(direction);

		Color color = ShaderScatteringColor(dir, directLight);
		color = TOD_HDR2LDR(color);
		color = TOD_LINEAR2GAMMA(color);

		return color;
	}

	#if !UNITY_4
	/// Render the sky dome to 3rd order spherical harmonics.
	internal SphericalHarmonicsL2 RenderToSphericalHarmonics()
	{
		var sh = new SphericalHarmonicsL2();

		bool directLight = false;

		const float scale1 = 1f / 7f;
		const float scale2 = 2f / 7f;
		const float scale3 = 3f / 7f;

		var amb = AmbientColor.linear;

		Vector3 halfway = new Vector3(0.61237243569579f, 0.5f, 0.61237243569579f);

		// Top
		{
			var dir = Vector3.up;
			var col = SampleAtmosphere(dir, directLight).linear;
			sh.AddDirectionalLight(dir, col, scale3);
		}

		// Upper
		{
			var dir = new Vector3(-halfway.x, +halfway.y, -halfway.z);
			var col = SampleAtmosphere(dir, directLight).linear;
			sh.AddDirectionalLight(dir, col, scale2);
		}
		{
			var dir = new Vector3(+halfway.x, +halfway.y, -halfway.z);
			var col = SampleAtmosphere(dir, directLight).linear;
			sh.AddDirectionalLight(dir, col, scale2);
		}
		{
			var dir = new Vector3(-halfway.x, +halfway.y, +halfway.z);
			var col = SampleAtmosphere(dir, directLight).linear;
			sh.AddDirectionalLight(dir, col, scale2);
		}
		{
			var dir = new Vector3(+halfway.x, +halfway.y, +halfway.z);
			var col = SampleAtmosphere(dir, directLight).linear;
			sh.AddDirectionalLight(dir, col, scale2);
		}

		// Equator
		{
			var dir = Vector3.left;
			var col = SampleAtmosphere(dir, directLight).linear;
			sh.AddDirectionalLight(dir, col, scale1);
		}
		{
			var dir = Vector3.right;
			var col = SampleAtmosphere(dir, directLight).linear;
			sh.AddDirectionalLight(dir, col, scale1);
		}
		{
			var dir = Vector3.back;
			var col = SampleAtmosphere(dir, directLight).linear;
			sh.AddDirectionalLight(dir, col, scale1);
		}
		{
			var dir = Vector3.forward;
			var col = SampleAtmosphere(dir, directLight).linear;
			sh.AddDirectionalLight(dir, col, scale1);
		}

		// Lower
		{
			var dir = new Vector3(-halfway.x, -halfway.y, -halfway.z);
			sh.AddDirectionalLight(dir, amb, scale2);
		}
		{
			var dir = new Vector3(+halfway.x, -halfway.y, -halfway.z);
			sh.AddDirectionalLight(dir, amb, scale2);
		}
		{
			var dir = new Vector3(-halfway.x, -halfway.y, +halfway.z);
			sh.AddDirectionalLight(dir, amb, scale2);
		}
		{
			var dir = new Vector3(+halfway.x, -halfway.y, +halfway.z);
			sh.AddDirectionalLight(dir, amb, scale2);
		}

		// Bottom
		{
			var dir = Vector3.down;
			sh.AddDirectionalLight(dir, amb, scale3);
		}

		return sh;
	}
	#endif

	#if !UNITY_4
	/// Render the sky dome to a cubemap render texture.
	/// \param targetTexture Target RenderTexture in which rendering should be done.
	internal void RenderToCubemap(RenderTexture targetTexture = null)
	{
		if (!Probe)
		{
			Probe = new GameObject().AddComponent<ReflectionProbe>();
			Probe.name = gameObject.name + " Reflection Probe";
			Probe.mode = ReflectionProbeMode.Realtime;
		}

		if (probeRenderID < 0 || Probe.IsFinishedRendering(probeRenderID))
		{
			var size = float.MaxValue;

			Probe.transform.position = Components.DomeTransform.position;
			Probe.size = new Vector3(size, size, size);
			Probe.intensity = RenderSettings.reflectionIntensity;
			Probe.clearFlags = Reflection.ClearFlags;
			Probe.cullingMask = Reflection.CullingMask;
			Probe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
			Probe.timeSlicingMode = Reflection.TimeSlicing;
			probeRenderID = Probe.RenderProbe(targetTexture);
		}
	}
	#endif

	/// Calculate the fog color.
	/// \param directLight Whether or not to include direct light.
	internal Color SampleFogColor(bool directLight = true)
	{
		var camera = Vector3.forward;
		if (Components.Camera != null)
		{
			camera = Quaternion.Euler(0, Components.Camera.transform.rotation.eulerAngles.y, 0) * camera;
		}
		var sample = Vector3.Lerp(camera, Vector3.up, Fog.HeightBias);
		var color  = SampleAtmosphere(sample.normalized, directLight);
		return new Color(color.r, color.g, color.b, 1);
	}

	/// Calculate the sky color.
	internal Color SampleSkyColor()
	{
		var sample = SunDirection; sample.y = Mathf.Abs(sample.y);
		var color  = SampleAtmosphere(sample.normalized, false);
		return new Color(color.r, color.g, color.b, 1);
	}

	/// Calculate the equator color.
	internal Color SampleEquatorColor()
	{
		var sample = SunDirection; sample.y = 0;
		var color  = SampleAtmosphere(sample.normalized, false);
		return new Color(color.r, color.g, color.b, 1);
	}

	/// Update the RenderSettings fog color according to TOD_FogParameters.
	internal void UpdateFog()
	{
		switch (Fog.Mode)
		{
			case TOD_FogType.None:
				break;

			case TOD_FogType.Color:
				var fogColor = SampleFogColor(false);

				#if UNITY_EDITOR
				if (RenderSettings.fogColor != fogColor)
				#endif
				{
					RenderSettings.fogColor = fogColor;
				}
				break;

			case TOD_FogType.Directional:
				var fogColorDirectional = SampleFogColor(true);

				#if UNITY_EDITOR
				if (RenderSettings.fogColor != fogColorDirectional)
				#endif
				{
					RenderSettings.fogColor = fogColorDirectional;
				}
				break;
		}
	}

	/// Update the RenderSettings ambient light according to TOD_AmbientParameters.
	internal void UpdateAmbient()
	{
		float intensity = Mathf.Lerp(Night.AmbientMultiplier, Day.AmbientMultiplier, LerpValue);

		#if !UNITY_4
		switch (Ambient.Mode)
		{
			case TOD_AmbientType.Color:
				#if UNITY_EDITOR
				if (RenderSettings.ambientMode != AmbientMode.Flat)
				#endif
				{
					RenderSettings.ambientMode = AmbientMode.Flat;
				}

				#if UNITY_EDITOR
				if (RenderSettings.ambientLight != AmbientColor)
				#endif
				{
					RenderSettings.ambientLight = AmbientColor;
				}

				#if UNITY_EDITOR
				if (RenderSettings.ambientIntensity != intensity)
				#endif
				{
					RenderSettings.ambientIntensity = intensity;
				}
				break;

			case TOD_AmbientType.Gradient:
				var groundColor  = AmbientColor;
				var equatorColor = SampleEquatorColor();
				var skyColor     = SampleSkyColor();

				#if UNITY_EDITOR
				if (RenderSettings.ambientMode != AmbientMode.Trilight)
				#endif
				{
					RenderSettings.ambientMode = AmbientMode.Trilight;
				}

				#if UNITY_EDITOR
				if (RenderSettings.ambientSkyColor != skyColor)
				#endif
				{
					RenderSettings.ambientSkyColor = skyColor;
				}

				#if UNITY_EDITOR
				if (RenderSettings.ambientEquatorColor != equatorColor)
				#endif
				{
					RenderSettings.ambientEquatorColor = equatorColor;
				}

				#if UNITY_EDITOR
				if (RenderSettings.ambientGroundColor != groundColor)
				#endif
				{
					RenderSettings.ambientGroundColor = groundColor;
				}

				#if UNITY_EDITOR
				if (RenderSettings.ambientIntensity != intensity)
				#endif
				{
					RenderSettings.ambientIntensity = intensity;
				}
				break;

			case TOD_AmbientType.Spherical:
				#if UNITY_EDITOR
				if (RenderSettings.ambientMode != AmbientMode.Skybox)
				#endif
				{
					RenderSettings.ambientMode = AmbientMode.Skybox;
				}

				#if UNITY_EDITOR
				if (RenderSettings.skybox != Resources.SkyboxMaterial)
				#endif
				{
					RenderSettings.skybox = Resources.SkyboxMaterial;
				}

				#if UNITY_EDITOR
				if (RenderSettings.ambientLight != AmbientColor)
				#endif
				{
					RenderSettings.ambientLight = AmbientColor;
				}

				#if UNITY_EDITOR
				if (RenderSettings.ambientIntensity != intensity)
				#endif
				{
					RenderSettings.ambientIntensity = intensity;
				}

				RenderSettings.ambientProbe = RenderToSphericalHarmonics();
				break;
		}
		#else
		switch (Ambient.Mode)
		{
			case TOD_AmbientType.Color:
				var ambientLight = TOD_Util.MulRGB(AmbientColor, intensity);

				#if UNITY_EDITOR
				if (RenderSettings.ambientLight != ambientLight)
				#endif
				{
					RenderSettings.ambientLight = ambientLight;
				}
				break;
		}
		#endif
	}

	/// Update the RenderSettings reflection probe according to TOD_ReflectionParameters.
	internal void UpdateReflection()
	{
		#if !UNITY_4
		switch (Reflection.Mode)
		{
			case TOD_ReflectionType.Cubemap:
				float intensity = Mathf.Lerp(Night.ReflectionMultiplier, Day.ReflectionMultiplier, LerpValue);

				#if UNITY_EDITOR
				if (RenderSettings.defaultReflectionMode != DefaultReflectionMode.Skybox)
				#endif
				{
					RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
				}

				#if UNITY_EDITOR
				if (RenderSettings.skybox != Resources.SkyboxMaterial)
				#endif
				{
					RenderSettings.skybox = Resources.SkyboxMaterial;
				}

				#if UNITY_EDITOR
				if (RenderSettings.reflectionIntensity != intensity)
				#endif
				{
					RenderSettings.reflectionIntensity = intensity;
				}

				if (Application.isPlaying)
				{
					RenderToCubemap();
				}
				break;
		}
		#endif
	}

	/// Load parameters at runtime.
	/// \param xml The parameters to load, serialized to XML.
	internal void LoadParameters(string xml)
	{
		var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TOD_Parameters));
		var reader = new System.Xml.XmlTextReader(new System.IO.StringReader(xml));
		var parameters = serializer.Deserialize(reader) as TOD_Parameters;

		parameters.ToSky(this);
	}
}
