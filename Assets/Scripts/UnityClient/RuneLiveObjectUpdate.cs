using UnityEngine;
using System.Collections;
using System;
using RS2Sharp;

public class RuneLiveObjectUpdate : MonoBehaviour
{

	public RuneObject runeScript;
	public float timeAlive = 0;

	public void FixedUpdate ()
	{
	
		if (runeScript.shouldRender) {
			runeScript.shouldRender = false;
			if (!runeScript.initialRendered) {
				runeScript.initialRendered = true;
				runeScript.myObject.transform.localPosition = new Vector3 ((runeScript.localX)+(runeScript.player != null ? runeScript.baseX : 0), (-runeScript.height) + 0.075f, (runeScript.localY)+(runeScript.player != null ? runeScript.baseY : 0));
				runeScript.objPos = runeScript.myObject.transform.localPosition;
				runeScript.myMesh = runeScript.myObject.GetComponent<MeshFilter> ().sharedMesh;
				if (runeScript.myMesh == null)
					runeScript.myMesh = UnityClient.GetMesh ();
				runeScript.myObject.GetComponent<MeshFilter> ().sharedMesh = runeScript.myMesh;
				runeScript.myRenderer = runeScript.myObject.GetComponent<MeshRenderer> ();
						
				MeshAndMat meshAndMaterial = null;//new RuneMesh();
				try {
					meshAndMaterial = runeScript.myRuneMesh.Render (runeScript.myMesh, false);
				} catch (Exception ex) {
					//Debug.Log ("Model ex " + ex.Message + " " + ex.StackTrace);
					//	GameObject.Destroy (myObject);
					goto end;
				}

				runeScript.myMesh = meshAndMaterial.mesh;
				//if(runeScript.myMesh == null || runeScript.myMesh.vertexCount = 0) GameObject.Destroy(gameObject);
				runeScript.myObject.GetComponent<MeshRenderer> ().sharedMaterials = meshAndMaterial.materials;
				if (!runeScript.isAnimated && !runeScript.isGfx) {
					runeScript.SetParent(runeScript.objectRoot);
				}
				runeScript.myObject.AddComponent<BoxCollider> ();
				UnityClient.objectsSinceLastBatch ++;
			} else {
				if ((runeScript.isAnimated || runeScript.isGfx) && runeScript.myMesh != null) {
					//myObject.transform.position = new Vector3 ((localX/128f) + baseX, -height/128f, (localY/128f) + baseY);
						
					MeshAndMat meshAndMaterial = null;//new RuneMesh();
					try {
						meshAndMaterial = runeScript.myRuneMesh.Render (runeScript.myMesh, true);
					} catch (Exception ex) {
						//Debug.Log ("Model ex" + ex.Message + " " + ex.StackTrace);
						//GameObject.Destroy (myObject);
						goto end;
					}
						runeScript.myMesh = meshAndMaterial.mesh;
					//myObject.GetComponent<MeshRenderer> ().sharedMaterials = meshAndMaterial.materials;
				}
			}
		}
		
		end:

		if (runeScript.shouldMove) {
			runeScript.shouldMove = false;
			runeScript.myObject.transform.localPosition = new Vector3 ((runeScript.localX)+(runeScript.player != null ? runeScript.baseX : 0), (-runeScript.height) + 0.075f, (runeScript.localY)+(runeScript.player != null ? runeScript.baseY : 0));
			runeScript.objPos = runeScript.myObject.transform.localPosition;
		}
		//transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.time * speed);
		if (runeScript.player != null)
			transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (new Vector3 (0, (runeScript.player.turnDirection / 2048f) * 360f, 0)), Time.deltaTime * UnityClient.rotationLerpSpeed);
		//if(!runeScript.isAnimated) GameObject.Destroy(gameObject.GetComponent<RuneLiveObject>());
				
//		if (runeScript.myObject != null) {
//			bool isDead = runeScript.isDead;
//			if (timeAlive >= UnityClient.gfxTimeout)
//				isDead = true;
//			Vector3 objPos = runeScript.objPos;
//			if (UnityClient.playerPos != Vector3.zero && !runeScript.isMainPlayer)
//			if ((objPos.x < (UnityClient.playerPos.x - UnityClient.disposeDistance)) || (objPos.x > (UnityClient.playerPos.x + UnityClient.disposeDistance)) || (objPos.z < (UnityClient.playerPos.z - UnityClient.disposeDistance)) || (objPos.z > (UnityClient.playerPos.z + UnityClient.disposeDistance)) || isDead) {
//				if (!UnityClient.objectsToDispose.Contains (runeScript.myObject) && !runeScript.isGrassObj && !runeScript.isTreeObj)
//					UnityClient.objectsToDispose.Add (runeScript.myObject);
//			}
//		}	
					
		if (runeScript.isGfx)
			timeAlive += 1;	
						
	}
	
	public void MouseIsOver ()
	{
		Model.MouseOnObjects [Model.anInt1687++] = runeScript.objConf;
	}
}