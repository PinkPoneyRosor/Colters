using UnityEngine;
using System.Collections;


//This Script manages the bird's eye point of view when the player is entering Soul Mode or Ghost mode.
public class BirdsEyeCam : MonoBehaviour {
	
	Transform soul;
	Transform player;
	ghostFollow ghostFollowScript;

	[HideInInspector]
	public bool followBody = false;

	private float localDeltaTime;

	public float TranslationSmooth;	//Camera translation movement smoothing multiplier
	public float RotationSmooth = 6.0f;	//Camera rotation movement smoothing multiplier

	Vector3 targetPosition;

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
			targetPosition = soul.position + new Vector3 (0, 10, 0) + Vector3.forward * -5 + Vector3.right * 2;
			transform.position = Vector3.Lerp (transform.position, targetPosition, localDeltaTime * TranslationSmooth);


			Quaternion selfRotation = Quaternion.LookRotation (soul.position - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, localDeltaTime * RotationSmooth); //selfRotation;
		} 
		else 
			followingBody();

	}

	void followingBody()
	{
		targetPosition = player.position + new Vector3 (0, 3, 0) + Vector3.forward * -3 + Vector3.right * 1;
		float distBetweenCamAndBody = (targetPosition - transform.position).sqrMagnitude;


		
		transform.position = Vector3.Lerp (transform.position, targetPosition, localDeltaTime * TranslationSmooth);
		
		Quaternion selfRotation = Quaternion.LookRotation (player.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, localDeltaTime * RotationSmooth); //selfRotation;
		if (distBetweenCamAndBody < 1) 
		{
			TranslationSmooth = 500;
			ghostFollowScript.cameraInPlace = true;
			Debug.Log("I'm in place, brah!");
		}
	}
}
