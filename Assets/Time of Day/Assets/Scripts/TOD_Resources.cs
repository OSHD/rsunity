using UnityEngine;

/// Material and mesh wrapper class.
///
/// Component of the sky dome parent game object.

public class TOD_Resources : MonoBehaviour
{
	public Mesh Quad;

	public Mesh SphereHigh;
	public Mesh SphereMedium;
	public Mesh SphereLow;

	public Mesh IcosphereHigh;
	public Mesh IcosphereMedium;
	public Mesh IcosphereLow;

	public Mesh HalfIcosphereHigh;
	public Mesh HalfIcosphereMedium;
	public Mesh HalfIcosphereLow;

	public Material CloudMaterial;
	public Material ShadowMaterial;
	public Material BillboardMaterial;
	public Material SpaceMaterial;
	public Material AtmosphereMaterial;
	public Material SunMaterial;
	public Material MoonMaterial;
	public Material ClearMaterial;
	public Material SkyboxMaterial;

	internal int ID_SunSkyColor;
	internal int ID_MoonSkyColor;

	internal int ID_SunCloudColor;
	internal int ID_MoonCloudColor;

	internal int ID_SunMeshColor;
	internal int ID_MoonMeshColor;

	internal int ID_CloudColor;
	internal int ID_AmbientColor;
	internal int ID_MoonHaloColor;

	internal int ID_SunDirection;
	internal int ID_MoonDirection;
	internal int ID_LightDirection;

	internal int ID_LocalSunDirection;
	internal int ID_LocalMoonDirection;
	internal int ID_LocalLightDirection;

	internal int ID_Contrast;
	internal int ID_Brightness;
	internal int ID_Fogginess;
	internal int ID_Directionality;
	internal int ID_MoonHaloPower;

	internal int ID_CloudDensity;
	internal int ID_CloudSharpness;
	internal int ID_CloudShadow;
	internal int ID_CloudScale;
	internal int ID_CloudUV;

	internal int ID_SpaceTiling;
	internal int ID_SpaceBrightness;

	internal int ID_SunMeshContrast;
	internal int ID_SunMeshBrightness;

	internal int ID_MoonMeshContrast;
	internal int ID_MoonMeshBrightness;

	internal int ID_kBetaMie;
	internal int ID_kSun;
	internal int ID_k4PI;
	internal int ID_kRadius;
	internal int ID_kScale;

	internal int ID_World2Sky;
	internal int ID_Sky2World;

	/// Initializes all resource references.
	public void Initialize()
	{
		ID_SunSkyColor = Shader.PropertyToID("TOD_SunSkyColor");
		ID_MoonSkyColor = Shader.PropertyToID("TOD_MoonSkyColor");

		ID_SunCloudColor = Shader.PropertyToID("TOD_SunCloudColor");
		ID_MoonCloudColor = Shader.PropertyToID("TOD_MoonCloudColor");

		ID_SunMeshColor = Shader.PropertyToID("TOD_SunMeshColor");
		ID_MoonMeshColor = Shader.PropertyToID("TOD_MoonMeshColor");

		ID_CloudColor = Shader.PropertyToID("TOD_CloudColor");
		ID_AmbientColor = Shader.PropertyToID("TOD_AmbientColor");
		ID_MoonHaloColor = Shader.PropertyToID("TOD_MoonHaloColor");

		ID_SunDirection = Shader.PropertyToID("TOD_SunDirection");
		ID_MoonDirection = Shader.PropertyToID("TOD_MoonDirection");
		ID_LightDirection = Shader.PropertyToID("TOD_LightDirection");

		ID_LocalSunDirection = Shader.PropertyToID("TOD_LocalSunDirection");
		ID_LocalMoonDirection = Shader.PropertyToID("TOD_LocalMoonDirection");
		ID_LocalLightDirection = Shader.PropertyToID("TOD_LocalLightDirection");

		ID_Contrast = Shader.PropertyToID("TOD_Contrast");
		ID_Brightness = Shader.PropertyToID("TOD_Brightness");
		ID_Fogginess = Shader.PropertyToID("TOD_Fogginess");
		ID_Directionality = Shader.PropertyToID("TOD_Directionality");
		ID_MoonHaloPower = Shader.PropertyToID("TOD_MoonHaloPower");

		ID_CloudDensity = Shader.PropertyToID("TOD_CloudDensity");
		ID_CloudSharpness = Shader.PropertyToID("TOD_CloudSharpness");
		ID_CloudShadow = Shader.PropertyToID("TOD_CloudShadow");
		ID_CloudScale = Shader.PropertyToID("TOD_CloudScale");
		ID_CloudUV = Shader.PropertyToID("TOD_CloudUV");

		ID_SpaceTiling = Shader.PropertyToID("TOD_SpaceTiling");
		ID_SpaceBrightness = Shader.PropertyToID("TOD_SpaceBrightness");

		ID_SunMeshContrast = Shader.PropertyToID("TOD_SunMeshContrast");
		ID_SunMeshBrightness = Shader.PropertyToID("TOD_SunMeshBrightness");

		ID_MoonMeshContrast = Shader.PropertyToID("TOD_MoonMeshContrast");
		ID_MoonMeshBrightness = Shader.PropertyToID("TOD_MoonMeshBrightness");

		ID_kBetaMie = Shader.PropertyToID("TOD_kBetaMie");
		ID_kSun = Shader.PropertyToID("TOD_kSun");
		ID_k4PI = Shader.PropertyToID("TOD_k4PI");
		ID_kRadius = Shader.PropertyToID("TOD_kRadius");
		ID_kScale = Shader.PropertyToID("TOD_kScale");

		ID_World2Sky = Shader.PropertyToID("TOD_World2Sky");
		ID_Sky2World = Shader.PropertyToID("TOD_Sky2World");
	}

	// Creates a quad.
	// \param minUV Minimum uv values.
	// \param maxUV Maximum uv values.
	public static Mesh CreateQuad(Vector2 minUV, Vector2 maxUV)
	{
		return new Mesh()
		{
			name = "Quad " + minUV + " " + maxUV,
			vertices = new Vector3[]
			{
				new Vector3(-1, -1, 0),
				new Vector3(-1, +1, 0),
				new Vector3(+1, +1, 0),
				new Vector3(+1, -1, 0)
			},
			uv = new Vector2[]
			{
				new Vector2(minUV.x, minUV.y),
				new Vector2(minUV.x, maxUV.y),
				new Vector2(maxUV.x, maxUV.y),
				new Vector2(maxUV.x, minUV.y)
			},
			triangles = new int[]
			{
				0, 3, 2,
				0, 2, 1
			},
			normals = new Vector3[]
			{
				new Vector3(0, 0, 1),
				new Vector3(0, 0, 1),
				new Vector3(0, 0, 1),
				new Vector3(0, 0, 1)
			},
			tangents = new Vector4[]
			{
				new Vector4(1, 0, 0, 1),
				new Vector4(1, 0, 0, 1),
				new Vector4(1, 0, 0, 1),
				new Vector4(1, 0, 0, 1)
			}
		};
	}
}
