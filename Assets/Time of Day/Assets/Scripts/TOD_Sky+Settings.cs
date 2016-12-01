using UnityEngine;

public partial class TOD_Sky : MonoBehaviour
{
	private float timeSinceLightUpdate      = float.MaxValue;
	private float timeSinceAmbientUpdate    = float.MaxValue;
	private float timeSinceReflectionUpdate = float.MaxValue;

	private void UpdateQualitySettings()
	{
		if (Headless) return;

		// Mesh quality

		Mesh spaceMesh      = null;
		Mesh atmosphereMesh = null;
		Mesh clearMesh      = null;
		Mesh cloudMesh      = null;
		Mesh sunMesh        = null;
		Mesh moonMesh       = null;

		switch (MeshQuality)
		{
			case TOD_MeshQualityType.Low:
				spaceMesh      = Resources.IcosphereLow;
				atmosphereMesh = Resources.IcosphereLow;
				clearMesh      = Resources.IcosphereLow;
				cloudMesh      = Resources.HalfIcosphereLow;
				sunMesh        = Resources.Quad;
				moonMesh       = Resources.SphereLow;
				break;
			case TOD_MeshQualityType.Medium:
				spaceMesh      = Resources.IcosphereMedium;
				atmosphereMesh = Resources.IcosphereMedium;
				clearMesh      = Resources.IcosphereLow;
				cloudMesh      = Resources.HalfIcosphereMedium;
				sunMesh        = Resources.Quad;
				moonMesh       = Resources.SphereMedium;
				break;
			case TOD_MeshQualityType.High:
				spaceMesh      = Resources.IcosphereHigh;
				atmosphereMesh = Resources.IcosphereHigh;
				clearMesh      = Resources.IcosphereLow;
				cloudMesh      = Resources.HalfIcosphereHigh;
				sunMesh        = Resources.Quad;
				moonMesh       = Resources.SphereHigh;
				break;
		}

		// Materials

		if (Components.SpaceRenderer && Components.SpaceMaterial != Resources.SpaceMaterial)
		{
			Components.SpaceMaterial = Components.SpaceRenderer.sharedMaterial = Resources.SpaceMaterial;
		}

		if (Components.AtmosphereRenderer && Components.AtmosphereMaterial != Resources.AtmosphereMaterial)
		{
			Components.AtmosphereMaterial = Components.AtmosphereRenderer.sharedMaterial = Resources.AtmosphereMaterial;
		}

		if (Components.ClearRenderer && Components.ClearMaterial != Resources.ClearMaterial)
		{
			Components.ClearMaterial = Components.ClearRenderer.sharedMaterial = Resources.ClearMaterial;
		}

		if (Components.CloudRenderer && Components.CloudMaterial != Resources.CloudMaterial)
		{
			Components.CloudMaterial = Components.CloudRenderer.sharedMaterial = Resources.CloudMaterial;
		}

		if (Components.ShadowProjector && Components.ShadowMaterial != Resources.ShadowMaterial)
		{
			Components.ShadowMaterial = Components.ShadowProjector.material = Resources.ShadowMaterial;
		}

		if (Components.SunRenderer && Components.SunMaterial != Resources.SunMaterial)
		{
			Components.SunMaterial = Components.SunRenderer.sharedMaterial = Resources.SunMaterial;
		}

		if (Components.MoonRenderer && Components.MoonMaterial != Resources.MoonMaterial)
		{
			Components.MoonMaterial = Components.MoonRenderer.sharedMaterial = Resources.MoonMaterial;
		}

		// Meshes

		if (Components.SpaceMeshFilter && Components.SpaceMeshFilter.sharedMesh != spaceMesh)
		{
			Components.SpaceMeshFilter.mesh = spaceMesh;
		}

		if (Components.AtmosphereMeshFilter && Components.AtmosphereMeshFilter.sharedMesh != atmosphereMesh)
		{
			Components.AtmosphereMeshFilter.mesh = atmosphereMesh;
		}

		if (Components.ClearMeshFilter && Components.ClearMeshFilter.sharedMesh != clearMesh)
		{
			Components.ClearMeshFilter.mesh = clearMesh;
		}

		if (Components.CloudMeshFilter && Components.CloudMeshFilter.sharedMesh != cloudMesh)
		{
			Components.CloudMeshFilter.mesh = cloudMesh;
		}

		if (Components.SunMeshFilter && Components.SunMeshFilter.sharedMesh != sunMesh)
		{
			Components.SunMeshFilter.mesh = sunMesh;
		}

		if (Components.MoonMeshFilter && Components.MoonMeshFilter.sharedMesh != moonMesh)
		{
			Components.MoonMeshFilter.mesh = moonMesh;
		}
	}

	private void UpdateRenderSettings()
	{
		if (Headless) return;

		// Fog color
		{
			UpdateFog();
		}

		// Ambient light
		if (!Application.isPlaying || timeSinceAmbientUpdate >= Ambient.UpdateInterval)
		{
			timeSinceAmbientUpdate = 0;
			UpdateAmbient();
		}
		else
		{
			timeSinceAmbientUpdate += Time.deltaTime;
		}

		// Reflection cubemap
		if (!Application.isPlaying || timeSinceReflectionUpdate >= Reflection.UpdateInterval)
		{
			timeSinceReflectionUpdate = 0;
			UpdateReflection();
		}
		else
		{
			timeSinceReflectionUpdate += Time.deltaTime;
		}
	}

	private void UpdateShaderKeywords()
	{
		if (Headless) return;

		if (Resources.CloudMaterial)
		{
			SetCloudQuality(Resources.CloudMaterial);
			SetColorSpace(Resources.CloudMaterial);
			SetColorRange(Resources.CloudMaterial);
		}

		if (Resources.BillboardMaterial)
		{
			SetCloudQuality(Resources.BillboardMaterial);
			SetColorSpace(Resources.BillboardMaterial);
			SetColorRange(Resources.BillboardMaterial);
		}

		if (Resources.ShadowMaterial)
		{
			SetCloudQuality(Resources.ShadowMaterial);
		}

		if (Resources.AtmosphereMaterial)
		{
			SetSkyQuality(Resources.AtmosphereMaterial);
			SetColorSpace(Resources.AtmosphereMaterial);
			SetColorRange(Resources.AtmosphereMaterial);
		}

		if (Resources.SkyboxMaterial)
		{
			SetColorSpace(Resources.SkyboxMaterial);
			SetColorRange(Resources.SkyboxMaterial);
		}
	}

	private void UpdateShaderProperties()
	{
		if (Headless) return;

		// Precalculations
		Vector4 cloudUV = Components.Animation.CloudUV + Components.Animation.OffsetUV;
		Vector4 cloudScale = new Vector4(Clouds.Scale1.x, Clouds.Scale1.y, Clouds.Scale2.x, Clouds.Scale2.y);
		float cloudShadow = Clouds.ShadowStrength * Mathf.Clamp01(1f - LightZenith / 90f);

		// Setup global shader parameters
		{
			Shader.SetGlobalColor(Resources.ID_SunSkyColor,  SunSkyColor);
			Shader.SetGlobalColor(Resources.ID_MoonSkyColor, MoonSkyColor);

			Shader.SetGlobalColor(Resources.ID_SunCloudColor,  CloudColor * SunLightColor);
			Shader.SetGlobalColor(Resources.ID_MoonCloudColor, CloudColor * MoonLightColor);

			Shader.SetGlobalColor(Resources.ID_SunMeshColor,  SunMeshColor);
			Shader.SetGlobalColor(Resources.ID_MoonMeshColor, MoonMeshColor);

			Shader.SetGlobalColor(Resources.ID_CloudColor,    CloudColor);
			Shader.SetGlobalColor(Resources.ID_AmbientColor,  AmbientColor);
			Shader.SetGlobalColor(Resources.ID_MoonHaloColor, MoonHaloColor);

			Shader.SetGlobalVector(Resources.ID_SunDirection,   SunDirection);
			Shader.SetGlobalVector(Resources.ID_MoonDirection,  MoonDirection);
			Shader.SetGlobalVector(Resources.ID_LightDirection, LightDirection);

			Shader.SetGlobalVector(Resources.ID_LocalSunDirection,   LocalSunDirection);
			Shader.SetGlobalVector(Resources.ID_LocalMoonDirection,  LocalMoonDirection);
			Shader.SetGlobalVector(Resources.ID_LocalLightDirection, LocalLightDirection);

			Shader.SetGlobalFloat(Resources.ID_Contrast,       Atmosphere.Contrast);
			Shader.SetGlobalFloat(Resources.ID_Brightness,     Atmosphere.Brightness);
			Shader.SetGlobalFloat(Resources.ID_Fogginess,      Atmosphere.Fogginess);
			Shader.SetGlobalFloat(Resources.ID_Directionality, Atmosphere.Directionality);
			Shader.SetGlobalFloat(Resources.ID_MoonHaloPower,  1f / Moon.HaloSize);

			Shader.SetGlobalFloat(Resources.ID_CloudDensity,   Clouds.Density);
			Shader.SetGlobalFloat(Resources.ID_CloudSharpness, Clouds.Sharpness);
			Shader.SetGlobalFloat(Resources.ID_CloudShadow,    cloudShadow);
			Shader.SetGlobalVector(Resources.ID_CloudScale,    cloudScale);
			Shader.SetGlobalVector(Resources.ID_CloudUV,       cloudUV);

			Shader.SetGlobalFloat(Resources.ID_SpaceTiling,     Stars.Tiling);
			Shader.SetGlobalFloat(Resources.ID_SpaceBrightness, Stars.Brightness * (1-Atmosphere.Fogginess) * (1-LerpValue));

			Shader.SetGlobalFloat(Resources.ID_SunMeshContrast,   2f / Sun.MeshContrast);
			Shader.SetGlobalFloat(Resources.ID_SunMeshBrightness, Sun.MeshBrightness * (1-Atmosphere.Fogginess));

			Shader.SetGlobalFloat(Resources.ID_MoonMeshContrast,   1f / Moon.MeshContrast);
			Shader.SetGlobalFloat(Resources.ID_MoonMeshBrightness, Moon.MeshBrightness * (1-Atmosphere.Fogginess));

			Shader.SetGlobalVector(Resources.ID_kBetaMie, kBetaMie);
			Shader.SetGlobalVector(Resources.ID_kSun,     kSun);
			Shader.SetGlobalVector(Resources.ID_k4PI,     k4PI);
			Shader.SetGlobalVector(Resources.ID_kRadius,  kRadius);
			Shader.SetGlobalVector(Resources.ID_kScale,   kScale);

			Shader.SetGlobalMatrix(Resources.ID_World2Sky, Components.DomeTransform.worldToLocalMatrix);
			Shader.SetGlobalMatrix(Resources.ID_Sky2World, Components.DomeTransform.localToWorldMatrix);
		}

		// Setup shadow projector
		if (Components.ShadowProjector)
		{
			var farClipPlane     = Radius * 2;
			var orthographicSize = Radius;

			#if UNITY_EDITOR
			if (Components.ShadowProjector.farClipPlane != farClipPlane)
			#endif
			{
				Components.ShadowProjector.farClipPlane = farClipPlane;
			}

			#if UNITY_EDITOR
			if (Components.ShadowProjector.orthographicSize != orthographicSize)
			#endif
			{
				Components.ShadowProjector.orthographicSize = orthographicSize;
			}
		}
	}

	private void SetColorSpace(Material material)
	{
		switch (ColorSpace)
		{
			case TOD_ColorSpaceType.Auto:
				if (QualitySettings.activeColorSpace == UnityEngine.ColorSpace.Linear)
				{
					material.EnableKeyword("LINEAR");
					material.DisableKeyword("GAMMA");
				}
				else
				{
					material.DisableKeyword("LINEAR");
					material.EnableKeyword("GAMMA");
				}
				break;

			case TOD_ColorSpaceType.Linear:
				material.EnableKeyword("LINEAR");
				material.DisableKeyword("GAMMA");
				break;

			case TOD_ColorSpaceType.Gamma:
				material.DisableKeyword("LINEAR");
				material.EnableKeyword("GAMMA");
				break;
		}
	}

	private void SetColorRange(Material material)
	{
		switch (ColorRange)
		{
			case TOD_ColorRangeType.Auto:
				if (Components.Camera && Components.Camera.HDR)
				{
					material.EnableKeyword("HDR");
					material.DisableKeyword("LDR");
				}
				else
				{
					material.DisableKeyword("HDR");
					material.EnableKeyword("LDR");
				}
				break;

			case TOD_ColorRangeType.HDR:
				material.EnableKeyword("HDR");
				material.DisableKeyword("LDR");
				break;

			case TOD_ColorRangeType.LDR:
				material.DisableKeyword("HDR");
				material.EnableKeyword("LDR");
				break;
		}
	}

	private void SetSkyQuality(Material material)
	{
		switch (SkyQuality)
		{
			case TOD_SkyQualityType.PerVertex:
				material.EnableKeyword("PER_VERTEX");
				material.DisableKeyword("PER_PIXEL");
				break;

			case TOD_SkyQualityType.PerPixel:
				material.DisableKeyword("PER_VERTEX");
				material.EnableKeyword("PER_PIXEL");
				break;
		}
	}

	private void SetCloudQuality(Material material)
	{
		switch (CloudQuality)
		{
			case TOD_CloudQualityType.Fastest:
				material.EnableKeyword("FASTEST");
				material.DisableKeyword("DENSITY");
				material.DisableKeyword("BUMPED");
				break;

			case TOD_CloudQualityType.Density:
				material.DisableKeyword("FASTEST");
				material.EnableKeyword("DENSITY");
				material.DisableKeyword("BUMPED");
				break;

			case TOD_CloudQualityType.Bumped:
				material.DisableKeyword("FASTEST");
				material.DisableKeyword("DENSITY");
				material.EnableKeyword("BUMPED");
				break;
		}
	}
}
