#if UNITY_4_0||UNITY_4_1||UNITY_4_2||UNITY_4_3||UNITY_4_4||UNITY_4_5||UNITY_4_6||UNITY_4_7||UNITY_4_8||UNITY_4_9
#define UNITY_4
#endif

using UnityEngine;

public partial class TOD_Sky : MonoBehaviour
{
	protected void OnEnable()
	{
		Components = GetComponent<TOD_Components>();
		Components.Initialize();

		Resources = GetComponent<TOD_Resources>();
		Resources.Initialize();

		LateUpdate();

		instances.Add(this);

		Initialized = true;
	}

	protected void OnDisable()
	{
		instances.Remove(this);

		#if !UNITY_4
		if (Probe) Destroy(Probe.gameObject);
		#endif
	}

	protected void Start()
	{
		if (Application.isPlaying)
		{
			Vector2 atlas = Resources.BillboardMaterial.mainTextureScale;
			int atlas_x = Mathf.RoundToInt(1f / atlas.x);
			int atlas_y = Mathf.RoundToInt(1f / atlas.y);

			// Create mesh prototypes
			var meshes = new Mesh[2 * atlas_x * atlas_y];
			{
				for (int y = 0; y < atlas_y; y++)
				for (int x = 0; x < atlas_x; x++)
				{
					meshes[y * atlas_x + x] = TOD_Resources.CreateQuad(new Vector2(x, y), new Vector2(x + 1, y + 1));
				}

				for (int y = 0; y < atlas_y; y++)
				for (int x = 0; x < atlas_x; x++)
				{
					meshes[atlas_x * atlas_y + y * atlas_x + x] = TOD_Resources.CreateQuad(new Vector2(x + 1, y), new Vector2(x, y + 1));
				}
			}

			// Spawn billboard instances
			for (int i = 0; i < Clouds.Billboards; i++)
			{
				var go = new GameObject("Cloud " + i);
				go.transform.parent = Components.Billboards.transform;

				var scale = Random.Range(0.3f, 0.4f);
				go.transform.localScale = new Vector3(scale, scale * 0.5f, 1.0f);

				var angle = 2.0f * Mathf.PI * ((float)i / Clouds.Billboards);
				go.transform.localPosition = 0.95f * new Vector3(Mathf.Sin(angle), Random.Range(0.1f, 0.2f), Mathf.Cos(angle)).normalized;
				go.transform.LookAt(Components.DomeTransform.position);

				var cloudFilter = go.AddComponent<MeshFilter>();
				cloudFilter.sharedMesh = meshes[Random.Range(0, meshes.Length)];

				var cloudRenderer = go.AddComponent<MeshRenderer>();
				cloudRenderer.sharedMaterial = Resources.BillboardMaterial;
			}
		}
	}

	protected void LateUpdate()
	{
		Profiler.BeginSample("UpdateScattering");
		UpdateScattering();
		Profiler.EndSample();

		Profiler.BeginSample("UpdateCelestials");
		UpdateCelestials();
		Profiler.EndSample();

		Profiler.BeginSample("UpdateQualitySettings");
		UpdateQualitySettings();
		Profiler.EndSample();

		Profiler.BeginSample("UpdateRenderSettings");
		UpdateRenderSettings();
		Profiler.EndSample();

		Profiler.BeginSample("UpdateShaderKeywords");
		UpdateShaderKeywords();
		Profiler.EndSample();

		Profiler.BeginSample("UpdateShaderProperties");
		UpdateShaderProperties();
		Profiler.EndSample();
	}

	protected void OnValidate()
	{
		Cycle.DateTime = Cycle.DateTime;
	}
}
