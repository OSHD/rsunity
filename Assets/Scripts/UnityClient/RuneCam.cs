using UnityEngine;
using System.Collections;
using RS2Sharp;

public class RuneCam : MonoBehaviour {
	public Transform target;
	public Transform targetRot;
	public float distance = 3.0f;
	public float height = 3.0f;
	public float damping = 5.0f;
	public bool smoothRotation = true;
	public float rotationDamping = 10.0f;
	public float yOffset = 0;
	public UnityClient runeClient;
	public GameObject miniMapCam;
	public Camera gameCam;
	void Update () {
		if (targetRot == null || target == null) {
						if (target == null && UnityClient.myPlayer != null && UnityClient.myPlayer.playerObj != null && UnityClient.myPlayer.playerObj.myObject != null) {
								target = UnityClient.myPlayer.playerObj.myObject.transform;

						}
						if (targetRot == null) {
								targetRot = new GameObject ("CameraRotationPivot").transform;
						}
				}
		else
		{
			float playerX = (UnityClient.myPlayer.x >> 7) + UnityClient.baseX;
			float playerY = (UnityClient.myPlayer.y >> 7) + UnityClient.baseY;
		height = ((runeClient.yCameraCurve - 128) / 24f) + 0.66f;
			//target.transform.localRotation = Quaternion.Euler(new Vector3(0,(client.myPlayer.turnDirection/ 2048f)*360f,0));
			targetRot.transform.position = target.transform.position;
			UnityClient.playerPos = target.transform.position;
			
//			miniMapCam.transform.position = new Vector3(target.transform.position.x,miniMapCam.transform.position.y,target.transform.position.z);
//			miniMapCam.transform.localRotation = Quaternion.Euler(new Vector3(90,-((runeClient.minimapInt1/ 2048f)*360f),0));
			
		//target.transform.position = new Vector3 (playerX-0.095f, 2.26f, playerY);
			targetRot.transform.localRotation = Quaternion.Euler (0, -(((float)runeClient.xCameraCurve / 2048f)*360f), 0);
			Vector3 wantedPosition = targetRot.TransformPoint(0, height, -distance);
		transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);
		
		if (smoothRotation) {
				var wantedRotation = Quaternion.LookRotation(targetRot.position - transform.position, targetRot.up);
				transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
		}
		
			else transform.LookAt (targetRot, targetRot.up);
		}
		ProcessMouseHover();
	}
	
	Ray ray;
	public void ProcessMouseHover()
	{	
		ray = gameCam.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray);
		int i = 0;
        while (i < hits.Length) {
            RaycastHit hit = hits[i];
	        RuneLiveObject rlo = hit.collider.gameObject.GetComponent<RuneLiveObject>();
			RuneLiveObjectUpdate rloU = hit.collider.gameObject.GetComponent<RuneLiveObjectUpdate>();
			if(rlo != null) rlo.MouseIsOver();
			else if(rloU != null) rloU.MouseIsOver();
            i++;
        }
	}
//	public client runeClient;
//	public void Update()
//	{
//		transform.position = new Vector3 (((runeClient.xCameraPos + 64) / 2), 0, ((runeClient.yCameraPos + 64) / 2));
//	}
}
