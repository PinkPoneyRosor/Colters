using UnityEngine;
using System.Collections;


//This Script manages the bird's eye point of view when the player is entering Soul Mode or Ghost mode.
public class BirdsEyeCam : MonoBehaviour {
	
	Transform soul;
	Transform player;
	ghostFollow ghostFollowScript;
	GameObject[] allGhosts;
	bool notAllGhostsVisible = false;
	Vector3 finalBodyCameraPoint;
	Quaternion selfRotation;

	[HideInInspector]
	public bool followBody = false;

	private float localDeltaTime;

	public float TranslationSmooth;	//Camera translation movement smoothing multiplier
	public float RotationSmooth = 6.0f;	//Camera rotation movement smoothing multiplier

	Vector3 targetPosition;

	bool stopPlacingFollowingBody = false;

	// Use this for initialization
	void Start () 
	{
		soul = GameObject.Find ("Soul").transform;
		player = GameObject.Find ("Player").transform;

		ghostFollowScript = player.GetComponent<ghostFollow> ();
	}

	void Update ()
	{
		//Setting this object's local delta time...
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		//The camera's not looking at the same thing according to which mode hte player is in.
		//In Soul Mode, followBody will be false, whereas in Ghost mode, it'll follow the body.
		if (!followBody) 
		{
			TranslationSmooth = 5;

			soul = GameObject.Find ("Soul").transform;
			targetPosition = soul.position + new Vector3 (0, player.position.y + 10, 0) + Vector3.forward * -5 + Vector3.right * 2;
			transform.position = Vector3.Lerp (transform.position, targetPosition, localDeltaTime * TranslationSmooth);


			selfRotation = Quaternion.LookRotation (soul.position - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, localDeltaTime * RotationSmooth);
		} 
		else 
			followingBody();

	}

	void followingBody()
	{
		if (GameObject.FindWithTag ("Action Ghost") != null) 
		{
			allGhosts = GameObject.FindGameObjectsWithTag ("Action Ghost");

			Vector3 ghostsCentroid = Vector3.zero;
			int ghostCount = 0;

			foreach (GameObject ghost in allGhosts) 
			{
				ghostCount ++;
				ghostsCentroid += ghost.transform.position;
			}

			ghostsCentroid /= ghostCount;

			targetPosition = ghostsCentroid + new Vector3 (0, player.position.y + 10);

			TranslationSmooth = 1;

			transform.position = Vector3.Lerp (transform.position, targetPosition, localDeltaTime * TranslationSmooth);
			selfRotation = Quaternion.LookRotation (player.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, localDeltaTime * RotationSmooth);

			if ((this.transform.position - targetPosition).sqrMagnitude < 1) //Wait for the camera to get to the first point before commencing ghost follow mode.
					ghostFollowScript.cameraInPlace = true;
		}
	}

}
