using UnityEngine;

/// God ray camera component.
///
/// Based on SunShafts from the Unity Standard Assets.
/// Extended to get the god ray color from TOD_Sky and properly handle transparent meshes like clouds.

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Time of Day/Camera God Rays")]
public class TOD_Rays : TOD_ImageEffect
{
	public Shader GodRayShader = null;
	public Shader ScreenClearShader = null;

	/// The god ray rendering resolution.
	public ResolutionType Resolution = ResolutionType.Normal;

	/// The god ray rendering blend mode.
	public BlendModeType BlendMode = BlendModeType.Screen;

	/// The number of blur iterations to be performaed.
	[TOD_Range(0, 4)] public int BlurIterations = 2;

	/// The radius to blur filter applied to the god rays.
	[TOD_Min(0f)] public float BlurRadius = 2;

	/// The intensity of the god rays.
	[TOD_Min(0f)] public float Intensity = 1;

	/// The maximum radius of the god rays.
	[TOD_Min(0f)] public float MaxRadius = 0.5f;

	/// Whether or not to use the depth buffer.
	/// If enabled, requires the target platform to allow the camera to create a depth texture.
	/// Unity always creates this depth texture if deferred lighting is enabled.
	/// Otherwise this script will enable it for the camera it is attached to.
	/// If disabled, requires all shaders writing to the depth buffer to also write to the frame buffer alpha channel.
	/// Only the frame buffer alpha channel will then be used to check for ray blockers in the image effect.
	public bool UseDepthTexture = true;

	private Material godRayMaterial = null;
	private Material screenClearMaterial = null;

	private const int PASS_DEPTH   = 2;
	private const int PASS_NODEPTH = 3;
	private const int PASS_RADIAL  = 1;
	private const int PASS_SCREEN  = 0;
	private const int PASS_ADD     = 4;

	protected void OnEnable()
	{
		godRayMaterial = CreateMaterial(GodRayShader);
		screenClearMaterial = CreateMaterial(ScreenClearShader);
	}

	protected void OnDisable()
	{
		if (godRayMaterial) DestroyImmediate(godRayMaterial);
		if (screenClearMaterial) DestroyImmediate(screenClearMaterial);
	}

	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckSupport(UseDepthTexture))
		{
			Graphics.Blit(source, destination);
			return;
		}

		sky.Components.Rays = this;

		// Selected resolution
		int width, height, depth;
		if (Resolution == ResolutionType.High)
		{
			width  = source.width;
			height = source.height;
			depth  = 0;
		}
		else if (Resolution == ResolutionType.Normal)
		{
			width  = source.width / 2;
			height = source.height / 2;
			depth  = 0;
		}
		else
		{
			width  = source.width / 4;
			height = source.height / 4;
			depth  = 0;
		}

		// Light position
		Vector3 v = cam.WorldToViewportPoint(sky.Components.LightTransform.position);

		godRayMaterial.SetVector("_BlurRadius4", new Vector4(1.0f, 1.0f, 0.0f, 0.0f) * BlurRadius);
		godRayMaterial.SetVector("_LightPosition", new Vector4(v.x, v.y, v.z, MaxRadius));

		RenderTexture buffer1 = RenderTexture.GetTemporary(width, height, depth);
		RenderTexture buffer2 = null; // Will be allocated later

		// Create blocker mask
		if (UseDepthTexture)
		{
			Graphics.Blit(source, buffer1, godRayMaterial, PASS_DEPTH);
		}
		else
		{
			Graphics.Blit(source, buffer1, godRayMaterial, PASS_NODEPTH);
		}

		// Paint a small black border to get rid of clamping problems
		DrawBorder(buffer1, screenClearMaterial);

		// Radial blur
		{
			float ofs = BlurRadius * (1.0f / 768.0f);
			godRayMaterial.SetVector("_BlurRadius4", new Vector4 (ofs, ofs, 0.0f, 0.0f));
			godRayMaterial.SetVector("_LightPosition", new Vector4 (v.x, v.y, v.z, MaxRadius));

			for (int i = 0; i < BlurIterations; i++ )
			{
				// Each iteration takes 2 * 6 samples
				// We update _BlurRadius each time to cheaply get a very smooth look

				buffer2 = RenderTexture.GetTemporary(width, height, depth);
				Graphics.Blit(buffer1, buffer2, godRayMaterial, PASS_RADIAL);
				RenderTexture.ReleaseTemporary(buffer1);

				ofs = BlurRadius * (((i * 2.0f + 1.0f) * 6.0f)) / 768.0f;
				godRayMaterial.SetVector("_BlurRadius4", new Vector4 (ofs, ofs, 0.0f, 0.0f) );

				buffer1 = RenderTexture.GetTemporary(width, height, depth);
				Graphics.Blit(buffer2, buffer1, godRayMaterial, PASS_RADIAL);
				RenderTexture.ReleaseTemporary(buffer2);

				ofs = BlurRadius * (((i * 2.0f + 2.0f) * 6.0f)) / 768.0f;
				godRayMaterial.SetVector("_BlurRadius4", new Vector4 (ofs, ofs, 0.0f, 0.0f) );
			}
		}

		// Blend together
		{
			Vector4 color = (v.z >= 0.0)
			              ? Intensity * (Vector4)sky.RayColor
			              : Vector4.zero; // No back projection!

			godRayMaterial.SetVector("_LightColor", color);
			godRayMaterial.SetTexture("_ColorBuffer", buffer1);

			if (BlendMode == BlendModeType.Screen)
			{
				Graphics.Blit(source, destination, godRayMaterial, PASS_SCREEN);
			}
			else
			{
				Graphics.Blit(source, destination, godRayMaterial, PASS_ADD);
			}

			RenderTexture.ReleaseTemporary(buffer1);
		}
	}

	/// Resolutions for rendering the god rays.
	public enum ResolutionType
	{
		Low,
		Normal,
		High,
	}

	/// Methods to blend the god rays with the image.
	public enum BlendModeType
	{
		Screen,
		Add,
	}
}
