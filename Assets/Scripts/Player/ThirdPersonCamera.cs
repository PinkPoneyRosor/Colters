using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

	#region variables
	#region smoothing
	public float TranslationSmooth;	//Camera translation movement smoothing multiplier
	public float RotationSmooth = 6.0f;	//Camera rotation movement smoothing multiplier
	#endregion

	#region External scripts & gameObjects
	[HideInInspector]
	public Transform camTarget; //What the camera follows
	private GameObject player;
	private PlayerController playerController;
	private bool ManualMode = false;
	#endregion

	#region position and orientation
	public Quaternion rotationOffset = Quaternion.identity;
	public float distance = 10.0f; //Set the distance between the camera and the player
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	float x = 0.0f;
	float y = 50f;
	#endregion

	#region Misc. Variables
	float localDeltaTime;
	public bool invertedVerticalAxis;
	public LayerMask CompensateLayer;
	#endregion

	#endregion

	void Start () {
		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent<PlayerController> ();
		camTarget = player.transform;
	}


	void Update()
	{
		//Setting this object's local delta time...
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;
	}


	//LateUpdate is called right after all of the update functions.
	void LateUpdate()
	{
		Vector3 setPosition;
		#region Input for the second stick's manual camera controls.
		if(ManualMode)
		{
			x += Input.GetAxis("LookH") * xSpeed * 0.02f;

			if (!invertedVerticalAxis)
				y -= Input.GetAxis("LookV") * ySpeed * 0.02f;
			else
				y += Input.GetAxis("LookV") * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit); //The vertical angle is clamped to avoid undesired behaviours.

			Quaternion rotationAroundTarget = Quaternion.Euler(y, x, 0f);
			setPosition = rotationAroundTarget * new Vector3(0.0f, 0.0f, -distance) + camTarget.position;
		}
		else
		#endregion
		{
			setPosition = player.transform.position + player.transform.forward * 10; //Mauvais, mauvais, mauvais, il faut que la caméra ORBITE autour du joueur, pas qu'elle parte directement sur le point !!!
			Debug.Log (setPosition);
		}
		Debug.DrawLine (this.transform.position, player.transform.forward);

		#region Getting camera to target position
		CompensateForWalls (camTarget.localPosition, ref setPosition);
		transform.position = Vector3.Lerp (transform.position, setPosition, localDeltaTime * TranslationSmooth);
		#endregion

		#region look at camera target
		Quaternion aimRotation = Quaternion.LookRotation(camTarget.position - transform.position);
		aimRotation *= rotationOffset;
		transform.rotation = Quaternion.Slerp(transform.rotation, aimRotation, localDeltaTime * RotationSmooth); //aimRotation;
		#endregion
	}


	//This method clamps the camera's manual vertical movement to avoid undesired behaviours.
	static float ClampAngle (float angle,float min,float max) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}


	//Recalculate the target position of the camera if a wall is between it and the player.
	private void CompensateForWalls (Vector3 fromObject, ref Vector3 toTarget)
	{
		RaycastHit wallHit = new RaycastHit ();
		//Debug.DrawLine (fromObject, toTarget);
		if (Physics.Linecast(fromObject, toTarget, out wallHit, CompensateLayer))
		{
			Vector3 hitWallNormal = wallHit.normal.normalized;
			toTarget = new Vector3(wallHit.point.x + 1f * hitWallNormal.x, wallHit.point.y + 1f * hitWallNormal.y, wallHit.point.z + 1f * hitWallNormal.z);
		}
	}
}
