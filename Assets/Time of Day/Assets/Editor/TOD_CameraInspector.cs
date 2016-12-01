using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(TOD_Camera))]
public class TOD_CameraInspector : Editor
{
	public override void OnInspectorGUI()
	{
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Update Sky Dome Position"))
		{
			(target as TOD_Camera).DoDomePosToCamera();
		}
		if (GUILayout.Button("Update Sky Dome Scale"))
		{
			(target as TOD_Camera).DoDomeScaleToFarClip();
		}
		GUILayout.EndHorizontal();

		DrawDefaultInspector();
	}
}
