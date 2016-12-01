using UnityEngine;

/// Atmospheric scattering and aerial perspective camera component.

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Time of Day/Camera Scattering")]
public class TOD_Scattering : TOD_ImageEffect
{
	public Shader ScatteringShader = null;

	public Texture2D DitheringTexture = null;

	private Material scatteringMaterial = null;

	protected void OnEnable()
	{
		scatteringMaterial = CreateMaterial(ScatteringShader);
	}

	protected void OnDisable()
	{
		if (scatteringMaterial) DestroyImmediate(scatteringMaterial);
	}

	protected void OnPreCull()
	{
		if (sky && sky.Initialized) sky.Components.AtmosphereRenderer.enabled = false;
	}

	protected void OnPostRender()
	{
		if (sky && sky.Initialized) sky.Components.AtmosphereRenderer.enabled = true;
	}

	[ImageEffectOpaque]
	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckSupport(true, true))
		{
			Graphics.Blit(source, destination);
			return;
		}

		sky.Components.Scattering = this;

		// Setup frustum corners
		float CAMERA_NEAR = cam.nearClipPlane;
		float CAMERA_FAR = cam.farClipPlane;
		float CAMERA_FOV = cam.fieldOfView;
		float CAMERA_ASPECT_RATIO = cam.aspect;

		Matrix4x4 frustumCorners = Matrix4x4.identity;

		float fovWHalf = CAMERA_FOV * 0.5f;

		Vector3 toRight = cam.transform.right * CAMERA_NEAR * Mathf.Tan (fovWHalf * Mathf.Deg2Rad) * CAMERA_ASPECT_RATIO;
		Vector3 toTop = cam.transform.up * CAMERA_NEAR * Mathf.Tan (fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (cam.transform.forward * CAMERA_NEAR - toRight + toTop);
		float CAMERA_SCALE = topLeft.magnitude * CAMERA_FAR/CAMERA_NEAR;

		topLeft.Normalize();
		topLeft *= CAMERA_SCALE;

		Vector3 topRight = (cam.transform.forward * CAMERA_NEAR + toRight + toTop);
		topRight.Normalize();
		topRight *= CAMERA_SCALE;

		Vector3 bottomRight = (cam.transform.forward * CAMERA_NEAR + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= CAMERA_SCALE;

		Vector3 bottomLeft = (cam.transform.forward * CAMERA_NEAR - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= CAMERA_SCALE;

		frustumCorners.SetRow (0, topLeft);
		frustumCorners.SetRow (1, topRight);
		frustumCorners.SetRow (2, bottomRight);
		frustumCorners.SetRow (3, bottomLeft);

		scatteringMaterial.SetMatrix("_FrustumCornersWS", frustumCorners);
		scatteringMaterial.SetTexture("_DitheringTexture", DitheringTexture);

		CustomBlit(source, destination, scatteringMaterial);
	}
}
