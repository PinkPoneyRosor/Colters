using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {
#region variables

	#region smoothing
	public float TranslationSmooth;	//Camera translation movement smoothing multiplier
	public float RotationSmooth = 6.0f;	//Camera rotation movement smoothing multiplier

	private float currentTranslationSmooth;
	private float currentRotationSmooth;
	#endregion

	#region External scripts & gameObjects
	[HideInInspector]
	public Transform camTarget; //What the camera follows

	private GameObject player;
	private CommonControls playerControls;
	private bool ManualMode = false;
	public GameObject soul;
	private bool soulMode = false;
	[HideInInspector]
	public bool dashingSoul = false;
	#endregion

	#region position and orientation
	public float cameraHeight = 6;
	public Quaternion rotationOffset = Quaternion.identity;
	public float distance = 10.0f; //Set the distance between the camera and the player
	//private float manualHorizontalSpeed = 250.0f;
	public float manualCameraSpeed = 10;
	public float manualCameraHeightMinLimit = -20f;
	public float manualCameraHeightMaxLimit = 80f;
	public Vector3 aimOffset;
	public float aimLookSpeed = 5;

	private Vector3 setPosition = Vector3.zero;
	private float x = 0.0f;
	private float y = 50f;
	private Vector3 targetToCamDir; //Direction from the camera to the player, only for x and z coordinates.
	#endregion

	#region Misc. Variables
	public bool invertedVerticalAxis;
	public bool aimInvertedVerticalAxis;
	LayerMask CompensateLayer;

	private float localDeltaTime;
	private bool JustEnteredAimMode = true;
	private bool resetManualModeValues = false;

	[HideInInspector]
	public bool aimingMode = false;
	[HideInInspector]
	public bool resetCameraPosition = false;
	[HideInInspector]
	public bool resetCameraFinalPosition = false;
	private float angleDir;
	#endregion

	#region testing
	[HideInInspector]
	public Vector3 camDirFromTarget = Vector3.zero;
	float xAim;
	public float AimMinVerticalAngle = 0;
	public float AimMaxVerticalAngle = 0;
	private bool initAimMode = true;
	private bool canUseManualMode = true;
	private bool justExitedAimMode = false;
	private bool transitioningToNormal = false;
	private bool AimSetUpTime = true;
	private float currentXReset = 0;
	[HideInInspector]
	public bool justHitAWall = false;
	#endregion
	#endregion

	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
		camTarget = player.transform;
		playerControls = player.GetComponent<CommonControls> ();

		currentRotationSmooth = RotationSmooth;
		currentTranslationSmooth = TranslationSmooth;
		CompensateLayer = LayerMask.GetMask("CameraCollider");
	}

	public void SwitchPlayerMode( bool bSoulMode )
	{
		soulMode = bSoulMode;
		player = GameObject.FindWithTag( soulMode ? "PlayerSoul" : "Player" );
		playerControls = player.GetComponent<CommonControls> ();
		camTarget = player.transform;
	}

	void LateUpdate()
	{
		//Setting this object's local delta time...
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;
	
		#region Aim Mode Trigger
		if ((Input.GetAxisRaw ("LT") > 0) || (Input.GetButton("Aim"))) 
			{
				StopCoroutine("transitionBackToNormalMode");
				
				if(JustEnteredAimMode)
				{
					xAim = 0;
					Debug.Log ("Resetting aim angle");
					playerControls.setAimMode = true;
					StartCoroutine("AimSetUpTimeFunc");
				}
				
				JustEnteredAimMode = false;
				CommonControls.aimingMode = true;
				aimingMode = true;
				
				if (initAimMode && playerControls.characterAngleOkForAim)
				{
					Vector3 setAimOffset = camTarget.transform.forward * aimOffset.z + camTarget.transform.up * aimOffset.y + camTarget.transform.right * aimOffset.x;
					float DistBetweenCamAndTargetPoint = Vector3.SqrMagnitude ((camTarget.position + setAimOffset) - transform.position );
					
					Vector3 currentCamTargetRotation = this.transform.eulerAngles;
					currentCamTargetRotation.y = camTarget.transform.eulerAngles.y;
					
					xAim += Input.GetAxis ("LookV") * 50 * aimLookSpeed * localDeltaTime;
					xAim = ClampAngle (xAim, AimMinVerticalAngle, AimMaxVerticalAngle);
					
					Quaternion targetRotation = Quaternion.Euler (xAim, currentCamTargetRotation.y, 0);
					float AngleFromCurrentToTarget = Quaternion.Angle ( transform.rotation, targetRotation);
					
					
					
					if ((DistBetweenCamAndTargetPoint < .1f && AngleFromCurrentToTarget < .1f) || !AimSetUpTime)
					{
						initAimMode = false;
					}
					else
					{
						this.transform.position = Vector3.Lerp (transform.position, camTarget.position + setAimOffset, localDeltaTime * 20);
						Vector3 forwardNeutralX = camera.transform.forward;
						
						Quaternion lookRotation = Quaternion.LookRotation (player.transform.forward);
						transform.localRotation = Quaternion.Slerp(transform.rotation, lookRotation, localDeltaTime * 20f);
						justExitedAimMode = true;
					}
				}	
			} 
			else 
			{
				
				aimingMode = false;
				JustEnteredAimMode = true;
				initAimMode = true;
				playerControls.characterAngleOkForAim = false;
			}
		#endregion

		if (camTarget != null) 
		{
			if (!aimingMode) //The aiming mode can only be activated while in normal camera mode.
			{
				if(justExitedAimMode)
				{
					this.StartCoroutine("transitionBackToNormalMode");
					justExitedAimMode = false;
				}
				DefaultCamera ();
			}
			else
			{
				aimingCameraMode ();
				ManualMode = false; //This is to ensure that next time we get back to Default Camera Mode, the behaviour will be in automatic mode.
			}
		}

	}
	
	IEnumerator transitionBackToNormalMode()
	{
		if(!ManualMode)
		{
			transitioningToNormal = true;
			yield return new WaitForSeconds (12f * Time.deltaTime);
			transitioningToNormal = false;
			CommonControls.aimingMode = false;
		}
	}
	
	IEnumerator AimSetUpTimeFunc()
	{
		AimSetUpTime = true;
		yield return new WaitForSeconds (15f * Time.deltaTime);
		AimSetUpTime = false;
	}

	public static float AngleDir( Vector3 fwd, Vector3 targetDir, Vector3 up )
	{
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);
		
		if (dir >= 0.0f)
		{
			return 1.0f;
		} 
		else //if (dir < 0.0f)
		{
			return -1.0f;
		} 
		/*
		else
		{
			return 0.0;
		}
		*/
	}
		
	//Default camera mode
	void DefaultCamera()
	{
		//If the secondary stick is being moved, we switch to manual mode.
		if ((Input.GetAxisRaw ("LookH") != 0 || Input.GetAxisRaw ("LookV") != 0) && ! resetCameraPosition)
			ManualMode = true;
		else if (Input.GetButtonDown ("AutoCam")) 
		{
			ManualMode = false;
			resetCameraPosition = true;
			resetCameraFinalPosition = false;
			angleDir = 0f;
		}

		#region Input for the second stick's manual camera controls.
		if (ManualMode && !transitioningToNormal) 
		{
			currentRotationSmooth = 60;

			if (resetManualModeValues)
			{
				resetManualModeValues = false;
				y = transform.position.y - camTarget.position.y;
			}

				x += Input.GetAxis ("LookH") * (manualCameraSpeed * 25) * 0.02f;

			if (!invertedVerticalAxis)
				y -= Input.GetAxis ("LookV") * manualCameraSpeed * 0.02f;
			else
				y += Input.GetAxis ("LookV") * manualCameraSpeed * 0.02f;
			
			y = Mathf.Clamp (y, manualCameraHeightMinLimit, manualCameraHeightMaxLimit); //The vertical angle is clamped to prevent the camera getting too high or too low.
			Quaternion rotationAroundTarget = Quaternion.Euler (0, x, 0f);
			setPosition = rotationAroundTarget * new Vector3 (0.0f, y, -distance) + camTarget.position;
		}
		else
		#endregion 
		{
			//From here, the camera is not in manual mode, so we make sure the camera will position itself automatically.
			resetManualModeValues = true;

			targetToCamDir = camTarget.transform.position - this.transform.position;
			targetToCamDir.y = 0f; //We don't want to use the height, it is controlled by another variable.
			targetToCamDir.Normalize (); //We just need the direction, so we normalize it, in order to multiply the vector later.

			#region reset position with button & transition from aiming mode
			if (resetCameraPosition) 
			{
				currentRotationSmooth = 60;
			
				Vector3 tempSetPosition = -camTarget.transform.forward * distance + camTarget.transform.up * cameraHeight;
				tempSetPosition += camTarget.transform.position;

				Vector3 distFromSetPosToCurrentPos = tempSetPosition - transform.position;
				float sqrDistFromSetPosToCurrentPos = distFromSetPosToCurrentPos.sqrMagnitude;

				//If the distance between the camera's position and the hypothetical target position is small enough..
				if (sqrDistFromSetPosToCurrentPos < .5f || justHitAWall)
				{
					//We just switch back to camera mode, without repositioning the camera.
					resetCameraPosition = false;
				}
				else
				{
					//But if the distance is more than 1 unit long, we use the position calculated before to get the camera in Phalene's back.
					if( !resetCameraFinalPosition || justHitAWall)
					{
						float currentAngleDir = AngleDir( camTarget.transform.forward, transform.position-camTarget.transform.position, Vector3.up );
						if( angleDir == 0f )
						{
							angleDir = currentAngleDir;
						}
						
						if( Mathf.Sign(angleDir) != Mathf.Sign(currentAngleDir) || justHitAWall )
						{
							setPosition = tempSetPosition;
							resetCameraFinalPosition = true;
							Debug.Log ("Finished reset or reset aborted");
						}
						else
						{
							currentXReset = this.transform.eulerAngles.y + (angleDir * manualCameraSpeed * 30) * 0.2f;
							
							Quaternion rotationAroundTarget = Quaternion.Euler (0, currentXReset, 0f);
							setPosition = rotationAroundTarget * new Vector3 (0, cameraHeight, -distance) + camTarget.position;
						}
						
						Debug.DrawLine(transform.position, setPosition, Color.red, 10f);
	
						
							//Debug.DrawLine(camTarget.transform.position, setPosition, Color.cyan, 10f);
/*						
						if( Vector3.Dot( (setPosition-transform.position).normalized, (tempSetPosition-setPosition).normalized ) <= 0 )
						{
							setPosition = tempSetPosition;
							resetCameraFinalPosition = true;
						}
						*/
					}
				}
			}
			else if (transitioningToNormal)
			{
				currentRotationSmooth = 6;
			
				Vector3 tempSetPosition = -camTarget.transform.forward * distance + camTarget.transform.up * cameraHeight;
				tempSetPosition += camTarget.transform.position;
				Vector3 distFromSetPosToCurrentPos = tempSetPosition - transform.position;
				float sqrDistFromSetPosToCurrentPos = distFromSetPosToCurrentPos.sqrMagnitude;
				//If the distance between the camera's position and the hypothetical target position is small enough..
				if (sqrDistFromSetPosToCurrentPos < .05f)
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
			#endregion

			x = this.transform.eulerAngles.y;	//Setting y & x to the current camera rotation values, so the manual mode can use this to start next time.
			y = this.transform.eulerAngles.x; 
		}
		#region Getting camera to target position
		CompensateForWalls (camTarget.position, ref setPosition);
		transform.position = Vector3.Lerp (transform.position, setPosition, localDeltaTime * currentTranslationSmooth);
		//At this point, the camera is at the right place.
		#endregion
			
		#region look at camera target
		Quaternion selfRotation = Quaternion.identity;
		
		if(!transitioningToNormal)
		{
			selfRotation = Quaternion.LookRotation (camTarget.position - transform.position);
			selfRotation *= rotationOffset;
		}
		else
		{	
			selfRotation = Quaternion.LookRotation (camTarget.position - transform.position);
			selfRotation.y = transform.rotation.y;	
		}
		transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, localDeltaTime * currentRotationSmooth);
		//At this point, the camera is at the right place AND is looking at the right point.
		
		#endregion
	}

	//This functions is enabled when the 3rd Person Camera switches to aiming mode.
	void aimingCameraMode ()
	{
		if (!playerControls.setAimMode && !initAimMode) 
		{
			//Those two lines make sure the camera get to the right place.
			Vector3 setAimOffset = camTarget.transform.forward * aimOffset.z + camTarget.transform.up * aimOffset.y + camTarget.transform.right * aimOffset.x;
			this.transform.position = Vector3.Lerp (transform.position, camTarget.position + setAimOffset, localDeltaTime * 30);

			#region Manual aiming
			Vector3 currentCamTargetRotation = this.transform.eulerAngles;
			currentCamTargetRotation.y = camTarget.transform.eulerAngles.y;

			//When aiming, the camera must look in the same exact vertical direction than the player model.
			transform.eulerAngles = currentCamTargetRotation;

			xAim += Input.GetAxis ("LookV") * 50 * aimLookSpeed * localDeltaTime;
			xAim = ClampAngle (xAim, AimMinVerticalAngle, AimMaxVerticalAngle);

			Quaternion rotation = Quaternion.Euler (xAim, currentCamTargetRotation.y, 0);

			transform.rotation = rotation;
			#endregion
		}
	}

	//This method can clamp different angles.
	static float ClampAngle (float angle, float min, float max) 
	{
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
		Debug.DrawLine (fromObject, toTarget); //Debug line to make the line visually appear.
		
		if (Physics.Linecast(fromObject, toTarget, out wallHit, CompensateLayer))
		{
			Vector3 hitWallNormal = wallHit.normal.normalized;
			toTarget = new Vector3(wallHit.point.x + .5f * hitWallNormal.x, wallHit.point.y + .5f * hitWallNormal.y, wallHit.point.z + .5f * hitWallNormal.z);
			justHitAWall = true;
		}
		else
			justHitAWall = false;
			
			Debug.Log ("TPC Script Side Hit Wall = "+ justHitAWall);
	}
}
