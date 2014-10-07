using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {
	
	public bool invertedVerticalAxis; //Is the rotation control inverted?
	public float lookHSpeed = 150; //Rotation control sensitivity.
	public float yMinLimit = -20f; //Angle limit when the camera looks down
	public float yMaxLimit = 80f; //Angle limit when the camera looks up
	public float xSpeed = 250.0f; //Aim Mode - horizontal sensitivity.
	public float ySpeed = 120.0f; //Aim mode - vertical sensitivity.
	public float distance = 10.0f;
	public float smooth = 5;
	public LayerMask CompensateLayer;
	public Transform thisTransform;
	public Quaternion rotationOffset = Quaternion.identity;

	private Vector3 targetPosition;
	private Vector3 lookDir; //Tells the cam where to look
	private Vector3 characterOffset = Vector3.zero;
	private float localDeltaTime;
	private float lookH = 0;
	private float x = 0.0f;
	private float y = 50f;
	public float damping = 6.0f;
	private Vector3 offset = new Vector3(0f, 1.3f, 0f);

	[HideInInspector]
	public Transform camTarget; //Ce que la caméra suit

	void Start()
	{
		camTarget = GameObject.FindWithTag ("Player").transform;
		thisTransform = transform;
	}

	void LateUpdate()
	{
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		if(camTarget!=null)
			characterOffset = camTarget.localPosition + offset;

		//On calcule la direction venant de la caméra et allant vers le joueur, on neutralise l'axe Y, et on normalise pour pouvoir utiliser la magnitude.
		lookDir = characterOffset - transform.position;
		lookDir.y = 0;
		lookDir.Normalize ();

		//Manual rotation around the player when the second stick is moved
		lookH = Input.GetAxis ("LookH");
		transform.RotateAround (camTarget.localPosition, Vector3.up, lookH * lookHSpeed * localDeltaTime);

		Vector3 setPosition = new Vector3(0.0f, 0.0f, -distance) + camTarget.position;
		targetPosition = setPosition;

		CompensateForWalls (characterOffset, ref targetPosition);
		Debug.Log ("target="+targetPosition);
		transform.position = Vector3.Lerp (transform.position, targetPosition, localDeltaTime * smooth);
		Debug.Log ("current="+transform.position);
		#region Aiming at target
		Quaternion rotation = Quaternion.LookRotation(camTarget.position - thisTransform.position);
		rotation *= rotationOffset;
		thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, rotation, localDeltaTime * damping);
		#endregion
	}

	static float ClampAngle (float angle,float min,float max) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}

	private void CompensateForWalls (Vector3 fromObject, ref Vector3 toTarget)
	{
		RaycastHit wallHit = new RaycastHit ();
		Debug.DrawLine (fromObject, toTarget);
		if (Physics.Linecast(fromObject, toTarget, out wallHit, CompensateLayer))
		{
			Vector3 hitWallNormal = wallHit.normal.normalized;
			toTarget = new Vector3(wallHit.point.x + 1f * hitWallNormal.x, wallHit.point.y + 1f * hitWallNormal.y, wallHit.point.z + 1f * hitWallNormal.z);
		}
	}
}
