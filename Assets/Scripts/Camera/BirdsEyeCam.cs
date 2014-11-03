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
			targetPosition = soul.position + new Vector3 (0, player.position.y + 10, 0) + Vector3.forward * -5 + Vector3.right * 2;
			transform.position = Vector3.Lerp (transform.position, targetPosition, localDeltaTime * TranslationSmooth);


			Quaternion selfRotation = Quaternion.LookRotation (soul.position - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, localDeltaTime * RotationSmooth);
		} 
		else 
			followingBody();

	}

	void followingBody()
	{
		float distBetweenCamAndBody = (player.position - transform.position).sqrMagnitude;

		if (distBetweenCamAndBody > 15 * 15) 
		{
			targetPosition = player.position + new Vector3 (0, player.position.y + 10, 0) + player.forward * -3 + Vector3.right * 1;
		} 
		else 
		{
			targetPosition = player.position + new Vector3(0,.5f,0) + player.forward * -3;
		}

		Vector3 playerForwardDir = (player.position - player.forward);

		Quaternion selfRotation = Quaternion.LookRotation (player.forward);
		transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, localDeltaTime * RotationSmooth);

		transform.position = Vector3.Lerp (transform.position, targetPosition, localDeltaTime * TranslationSmooth);

		float distBetweenCamAndTargetPos = (transform.position - targetPosition).sqrMagnitude;

		if (distBetweenCamAndTargetPos < .5f * .5f && GameObject.FindGameObjectWithTag("Action Ghost") != null) 
		{
			TranslationSmooth = 500;
			RotationSmooth = 60;
			ghostFollowScript.cameraInPlace = true;
			Debug.Log("I'm in place, brah!");
		}
	}
}
