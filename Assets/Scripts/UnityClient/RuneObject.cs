using UnityEngine;
using System.Collections;
using RS2Sharp;
using System;

public class RuneObject
{
	public GameObject myObject;
	public float localX;
	public float localY;
	public float height;
	public int regionX;
	public int regionY;
	public int objectType;
	public WallObject wallObj;
	public WallDecoration obj2;
	public GroundDecoration obj3;
	public Object4 obj4;
	public InteractiveObject obj5;
	public int baseX;
	public int baseY;
	public Entity player;
	public bool isAnimated = false;
	public int modelId = 0;
	public bool initialRendered = false;
	public RuneMesh myRuneMesh;
	public bool isMainPlayer = false;
	public bool shouldRender = false;
	public MeshRenderer myRenderer;
	public ObjectDef def;
	public bool isGrassObj = false;
	public bool isTreeObj = false;
	public int objConf = 0;
	public bool shouldMove = false;
	//public bool active = true;
	public bool isDead = false;
	public Mesh myMesh;
	public Vector3 objPos;
	public bool isGfx = false;
	public GameObject objectRoot;
	public GameObject animatedObjectsRoot;

	public RuneObject (Entity aPlayer)
	{
		player = aPlayer;
		isAnimated = true;
		myRuneMesh = new RuneMesh ();
	}

	public void UpdatePos (float x, float y, float z)
	{
		if (localX != x || height != y || localY != z || baseX != UnityClient.baseX || baseY != UnityClient.baseY) {
		baseX = UnityClient.baseX;
		baseY = UnityClient.baseY;
		localX = x;         
		height = y;
		localY = z;
		shouldMove = true;
		}
	}
	
	public void UpdateObjConf (int newConf)
	{
		objConf = newConf;
		int mId = objConf >> 14 & 0x7fff;
		if (modelId != mId) {
			modelId = mId;
			if (modelId > 0) {
				def = ObjectDef.forID (modelId);
				if (def != null) {
					if (def.animId != -1) {
						isAnimated = true;
					}
				}
			}
		}
	}

	public RuneObject (int x, int y, int z, int type, object mainClass, int bX, int bY, int id, GameObject objects, GameObject animatedObjects)
	{
		localX = x/128f;
		height = y/128f;
		localY = z/128f;
		if (mainClass is InteractiveObject)
		if ((mainClass as InteractiveObject).renderable is Projectile)
			isGfx = true;
		shouldMove = true;
		objectType = type;
		if (type == 1)
			wallObj = (WallObject)mainClass;
		if (type == 2)
			obj2 = (WallDecoration)mainClass;
		if (type == 3)
			obj3 = (GroundDecoration)mainClass;
		if (type == 4)
			obj4 = (Object4)mainClass;
		if (type == 5)
			obj5 = (InteractiveObject)mainClass;
		baseX = bX;
		baseY = bY;
		modelId = id;
		if (id > 0) {
			def = ObjectDef.forID (id);
			if (def != null) {
				if (def.animId != -1) {
					isAnimated = true;
				}
			}
		}
		myRuneMesh = new RuneMesh ();
		this.objectRoot = objects;
		this.animatedObjectsRoot = animatedObjects;
	}
	
	private bool isGrass ()
	{
		//if(myRuneMesh != null && myRuneMesh.faceTexture != null && myRuneMesh.faceTexture.Length != 0 && (myRuneMesh.faceTexture[0] == 970 || myRuneMesh.faceTexture[0] == 187))
		//	return true;
		for (int i = 0; i < UnityClient.grassIds.Length; ++i)
			if (UnityClient.grassIds [i] == modelId)
				return true;
		return false;
	}

	private int isTree ()
	{
		for (int i = 0; i < UnityClient.treeIds.Length; ++i)
			if (UnityClient.treeIds [i] == modelId)
				return 1;
		for (int i = 0; i < UnityClient.oakIds.Length; ++i)
			if (UnityClient.oakIds [i] == modelId)
				return 2;
		for (int i = 0; i < UnityClient.yewIds.Length; ++i)
			if (UnityClient.yewIds [i] == modelId)
				return 3;
		for (int i = 0; i < UnityClient.mapleIds.Length; ++i)
			if (UnityClient.mapleIds [i] == modelId)
				return 4;
		for (int i = 0; i < UnityClient.deadIds.Length; ++i)
			if (UnityClient.deadIds [i] == modelId)
				return 5;
		for (int i = 0; i < UnityClient.willowIds.Length; ++i)
			if (UnityClient.willowIds [i] == modelId)
				return 6;
		return 0;
	}

	public void Render (Model myModel)
	{
		if (myObject == null) {
			myRuneMesh.Fill (myModel, true);
		} else if (isAnimated || player != null || isGfx) {
			myRuneMesh.Fill (myModel, myRuneMesh.numVertices == myModel.numVertices ? false : true);
		}
		 else {
			return;
		}
		
		if (UnityClient.objectsToRender != null && this != null)
		if (!UnityClient.objectsToRender.Contains (this))
			UnityClient.objectsToRender.Add (this);
			
	}
	
	public void SetParent(GameObject parent)
	{
		if(parent == null) return;
		while (myObject.transform.parent != parent.transform)
			myObject.transform.parent = parent.transform;
	}

	public void RenderMesh ()
	{
		//if(!active) active = true;
		if (myObject == null) {
		
			//modelId = myModel.objId;
			try {
				if (player == null)
					def = ObjectDef.forID (modelId);
			} catch (Exception ex) {
			}
			//Loom.DispatchToMainThread (() => {
			//myRuneMesh.Fill (myModel,true);
			int treeType = isTree ();
			if (!isGrass () && treeType == 0) {
				if (!initialRendered) {
					myObject = UnityClient.GetRuneObject ();
					myObject.name = player != null ? (player is Player ? "Player : " + (player as Player).name : "Npc : " + (player as NPC).desc.name) : (def != null ? "Object : " + (def.name) + " : " + modelId : "Object" + " : " + modelId);
					if (isAnimated || player != null || isGfx) {
						myObject.AddComponent<RuneLiveObjectUpdate> ();
						myObject.GetComponent<RuneLiveObjectUpdate> ().runeScript = this;
						SetParent(animatedObjectsRoot);
					} else {
						myObject.AddComponent<RuneLiveObject> ();
						myObject.GetComponent<RuneLiveObject> ().runeScript = this;
						UnityClient.allRuneObjects.Add (myObject.GetComponent<RuneLiveObject> ());
						SetParent(objectRoot);
					}
					shouldRender = true;
				}
				
			} else if (isGrass ()) {
				myObject = GameObject.Instantiate (UnityClient.grassObjStatic);
				myObject.name = "Grass : " + (def.name) + " : " + modelId;
				myObject.layer = LayerMask.NameToLayer ("Objects");
				myObject.AddComponent<RuneLiveObject> ();
				myObject.GetComponent<RuneLiveObject> ().runeScript = this;
				isGrassObj = true;
				UnityClient.allRuneObjects.Add (myObject.GetComponent<RuneLiveObject> ());
				shouldRender = false;
				isAnimated = false;
				initialRendered = true;
				myRenderer = myObject.GetComponent<MeshRenderer> ();
				SetParent(animatedObjectsRoot);
			} else if (treeType > 0) {
				if (treeType == 1)
					myObject = GameObject.Instantiate (UnityClient.standardTreeStatic);
				else if (treeType == 2)
					myObject = GameObject.Instantiate (UnityClient.oakTreeStatic);
				else if (treeType == 3)
					myObject = GameObject.Instantiate (UnityClient.yewTreeStatic);
				else if (treeType == 4)
					myObject = GameObject.Instantiate (UnityClient.mapleTreeStatic);
				else if (treeType == 5)
					myObject = GameObject.Instantiate (UnityClient.deadTreeStatic);
				else if (treeType == 6)
					myObject = GameObject.Instantiate (UnityClient.willowTreeStatic);
				myObject.name = "Tree : " + (def.name) + " : " + modelId;
				myObject.layer = LayerMask.NameToLayer ("Objects");
				myObject.AddComponent<RuneLiveObject> ();
				myObject.GetComponent<RuneLiveObject> ().runeScript = this;
				UnityClient.allRuneObjects.Add (myObject.GetComponent<RuneLiveObject> ());
				myRenderer = myObject.GetComponent<MeshRenderer> ();
				shouldRender = false;
				isAnimated = false;
				isTreeObj = true;
				initialRendered = true;
				SetParent(animatedObjectsRoot);
			}
			//}, true, true);
		}
		//myObject.SetActive(true);
		if (myObject != null) {
			if (isAnimated || player != null || isGfx) {
				//myRuneMesh.Fill (myModel,myRuneMesh.numVertices == myModel.numVertices ? false : true);
				shouldRender = true;
			} else {
				if (shouldRender) {
					shouldRender = false;
					if (!initialRendered) {
						initialRendered = true;
						
						myObject.transform.localPosition = new Vector3 ((localX+(player != null ? baseX : 0)), (-height) + 0.075f, (localY+(player != null ? baseY : 0)));
						objPos = myObject.transform.localPosition;
						myMesh = myObject.GetComponent<MeshFilter> ().sharedMesh;
						if (myMesh == null) {
							myMesh = UnityClient.GetMesh ();
						}
						myObject.GetComponent<MeshFilter> ().sharedMesh = myMesh;
						myRenderer = myObject.GetComponent<MeshRenderer> ();
						
						MeshAndMat meshAndMaterial = null;//new RuneMesh();
						try {
							meshAndMaterial = myRuneMesh.Render (myMesh, false);
							if(meshAndMaterial.mesh != null && meshAndMaterial.mesh.vertexCount < 4) meshAndMaterial.mesh = null;
						} catch (Exception ex) {
							Debug.Log ("Model ex " + ex.Message + " " + ex.StackTrace);
							myObject.name = "Model EX";
							//	GameObject.Destroy (myObject);
							goto end;
						}

						myMesh = meshAndMaterial.mesh;
						//if(myMesh == null || myMesh.vertexCount = 0) GameObject.Destroy(gameObject);
						myObject.GetComponent<MeshRenderer> ().sharedMaterials = meshAndMaterial.materials;
						myObject.AddComponent<BoxCollider> ();
						UnityClient.objectsSinceLastBatch ++;
					} else {
						//  if (isAnimated && myMesh != null) {
						//  	//myObject.transform.localPosition = new Vector3 ((localX/128f) + baseX, -height/128f, (localY/128f) + baseY);
						
						//  	MeshAndMat meshAndMaterial = null;//new RuneMesh();
						//  	try {
						//  		meshAndMaterial = myRuneMesh.Render (myMesh, true);
						//  	} catch (Exception ex) {
						//  		//Debug.Log ("Model ex" + ex.Message + " " + ex.StackTrace);
						//  		//GameObject.Destroy (myObject);
						//  		goto end;
						//  	}
						//  	myMesh = meshAndMaterial.mesh;
						//  	//myObject.GetComponent<MeshRenderer> ().sharedMaterials = meshAndMaterial.materials;
						//  }
					}
				}
				end:
				if (shouldMove) {
					shouldMove = false;
					myObject.transform.localPosition = new Vector3 ((localX+(player != null ? baseX : 0)), (-height) + 0.075f, (localY+(player != null ? baseY : 0)));
					objPos = myObject.transform.localPosition;
				}

			
			}
		}
	}

}
