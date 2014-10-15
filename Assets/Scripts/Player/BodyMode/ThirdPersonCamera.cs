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
	BirdsEyeCam BirdsEyeScript;
	#endregion

	#region position and orientation
	public float cameraHeight = 6;
	public Quaternion rotationOffset = Quaternion.identity;
	public float distance = 10.0f; //Set the distance between the camera and the player
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	float x = 0.0f;
	float y = 50f;
	private Vector3 targetToCamDir; //Direction from the camera to the player, only for x and z coordinates.
	public Vector3 aimOffset;
	Vector3 setPosition = Vector3.zero;
	#endregion

	#region Misc. Variables
	float localDeltaTime;
	public bool invertedVerticalAxis;
	public LayerMask CompensateLayer;
	public bool birdsEyeActivated;
	private bool aimingMode = false;
	bool resetCameraPosition = false;
	#endregion

	#endregion

	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
		playerController = player.GetComponent<PlayerController> ();
		camTarget = player.transform;

		BirdsEyeScript = this.GetComponent<BirdsEyeCam> ();
	}

	void Update()
	{
		//Setting this object's local delta time...
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		if (!birdsEyeActivated) 
		{
			BirdsEyeScript.enabled = false;
			#region Aim Mode Trigger
			if (Input.GetAxisRaw ("Triggers") > 0) {
					playerController.aimingMode = true;
					aimingMode = true;
			} else {
					playerController.aimingMode = false;
					aimingMode = false;
			}
			#endregion
		}
		else
		BirdsEyeScript.enabled = true;
	}

	//LateUpdate is called right after all of the update functions.
	void LateUpdate()
	{
			if (!birdsEyeActivated) 
			{
		if (!aimingMode) 
		{
			DefaultCamera ();
		} 
		else 
		{
			aimingCameraMode();
			ManualMode = false; //This is to ensure that next time we get back to Default Camera Mode, the behaviour will be in automatic mode.
		}
			}
	}

	//Default camera mode
	void DefaultCamera()
	{

		//If the secondary stick is being moved, we switch to manual mode.
		if (Input.GetAxisRaw ("LookH") != 0 || Input.GetAxisRaw ("LookV") != 0)
			ManualMode = true;
		else if (Input.GetButtonDown ("AutoCam")) 
		{
			ManualMode = false;
			resetCameraPosition = true;
		}

		#region Input for the second stick's manual camera controls.
		if (ManualMode) 
		{
			x += Input.GetAxis ("LookH") * xSpeed * 0.02f;

			if (!invertedVerticalAxis)
				y -= Input.GetAxis ("LookV") * ySpeed * 0.02f;
			else
				y += Input.GetAxis ("LookV") * ySpeed * 0.02f;
			
			y = Mathf.Clamp (y, camTarget.transform.position.y + yMinLimit, camTarget.transform.position.y + yMaxLimit); //The vertical angle is clamped to the camera getting to high or to low.
			Quaternion rotationAroundTarget = Quaternion.Euler (0, x, 0f);
			setPosition = rotationAroundTarget * new Vector3 (0.0f, y, -distance) + camTarget.position;
		}
		else
			#endregion 
		{

			targetToCamDir = camTarget.transform.position - this.transform.position;
			targetToCamDir.y = 0f; //We don't want to use the height, it is controlled by another variable.
			targetToCamDir.Normalize (); //We just need the direction, so we normalize it, in order to multiply the vector later.

			if (resetCameraPosition) 
			{
				Vector3 tempSetPosition = -camTarget.transform.forward * distance + camTarget.transform.up * cameraHeight;
				tempSetPosition += camTarget.transform.position;

				Vector3 distFromSetPosToCurrentPos = tempSetPosition - transform.position;
				float sqrDistFromSetPosToCurrentPos = distFromSetPosToCurrentPos.sqrMagnitude;

				//If the distance between the camera's position and the hypothetical target position is too small..
				if(sqrDistFromSetPosToCurrentPos < 1)
				{
					//We just switch back to camera mode, without repositioning the camera.
					resetCameraPosition = false;
				}
				else
				{
					//But if the distance is more than 1 unit long, we use the position calculated before to get the camera in Phalene's back.
					setPosition = tempSetPosition;
				}
			}
			else
			{
				//Once the camera has been resetted correctly, we get back to its automatic positioning.
				setPosition = camTarget.position + new Vector3 (0, cameraHeight, 0) - targetToCamDir * distance;
			}

			x = this.transform.eulerAngles.y;	//Setting y & x to the current camera roation values, so the manual mode can use this to start next time.
			y = this.transform.eulerAngles.x; 
		}
		#region Getting camera to target position
		CompensateForWalls (camTarget.localPosition, ref setPosition);
		transform.position = Vector3.Lerp (transform.position, setPosition, localDeltaTime * TranslationSmooth);
		//At this point, the camera is at the right place.
		#endregion
		
		#region look at camera target
		Quaternion selfRotation = Quaternion.LookRotation (camTarget.position - transform.position);
		selfRotation *= rotationOffset;
		transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, localDeltaTime * RotationSmooth); //selfRotation;
		//At this point, the camera is at the right place AND is looking at the right point.
		#endregion
		}

	//This functions is enabled when the 3rd Person Camera switches to aiming mode.
	void aimingCameraMode ()
	{
		//Those two lines make sure the camera get to the right place.
		Vector3 setAimOffset = camTarget.transform.forward * aimOffset.z + camTarget.transform.up * aimOffset.y + camTarget.transform.right * aimOffset.x;
		this.transform.position = Vector3.Lerp (transform.position, camTarget.position + setAimOffset, localDeltaTime * 60);

		#region Manual aiming
		Vector3 currentCamTargetRotation = this.transform.eulerAngles;
		currentCamTargetRotation.y = camTarget.transform.eulerAngles.y;

		//When aiming, the camera must look in the same exact vertical direction than the player model.
		transform.eulerAngles = currentCamTargetRotation; //Change this to set it to a neutral angle when just entered aiming mode
		transform.Rotate  (new Vector3 (Input.GetAxis ("LookV")*50, 0,0) * Time.deltaTime);
		#endregion
	}

	//This method can clamp different angles.
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
