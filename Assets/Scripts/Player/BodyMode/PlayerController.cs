using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//Direction & movement variables
	float horizontal = 0.0f;
	float vertical = 0.0f;
	float floatDir = 0f;
	float speed = 3f;
	float gravity = 20;
	Vector3 moveDirection = Vector3.zero;
	Vector3 faceDirection = Vector3.zero;
	Vector3 direction = Vector3.zero;
	Vector3 tempMoveDir;

	public float maxSpeed = 5;

	//External scripts and objects
	ThirdPersonCamera mainCameraScript;
	public GameObject Soul;

	//Other variables
	CharacterController controller;
	[HideInInspector]
	public bool aimingMode = false;
	[HideInInspector]
	public bool soulMode = false;
	private bool continueResetControls = false;


	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
	}
	
	// Update is called once per frame
	void Update () {

		#region Get Axises
		//Get input from the main axis (Keyboard and stick)
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
		#endregion

		if(Input.GetButtonDown("SwitchMode") && !soulMode)
			SwitchToSoulMode();

		//Make the controls adapted to the current camera mode.
		if (!soulMode) 
		{
			if (Input.GetButtonDown ("AutoCam") || continueResetControls) //If the camera is resetting, the stick will only have control on the player's speed, not its direction
				ResettingCameraControls();
			else if (!aimingMode) //Else, and if we're in normal camera mode
				DefaultControls ();
			else if (aimingMode) //Else, and if we're in aiming camera mode
				AimingControls ();
		}
	}

	void DefaultControls()
	{
		#region default Controls
		#region setting essential variables each frame
		
		//This method will translate axis input into world coordinates, according to the camera's point of view.
		stickToWorldSpace(transform, mainCameraScript.transform, ref direction, ref floatDir, ref speed, false);
		#endregion
		
		Quaternion target = Quaternion.Euler(0, floatDir, 0);
		tempMoveDir = target * Vector3.forward * speed;
		tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed); 
		moveDirection.x = tempMoveDir.x;
		moveDirection.z = tempMoveDir.z;
		
		#region apply movements & gravity
		//This final section will appy movements and gravity.
		if(!controller.isGrounded)
			moveDirection.y -= gravity * Time.deltaTime;
		
		controller.Move(moveDirection * Time.deltaTime);
		
		faceDirection = transform.position + moveDirection;
		faceDirection.y = transform.position.y;
		
		transform.LookAt (faceDirection);
		#endregion
		#endregion
	}

	void AimingControls () //When aiming, the controls are not the same.
	{
		tempMoveDir = (transform.right * horizontal + transform.forward * vertical) * maxSpeed;
		moveDirection.x = tempMoveDir.x;
		moveDirection.z = tempMoveDir.z;

		if(!controller.isGrounded)
			moveDirection.y -= gravity * Time.deltaTime;

		controller.Move (moveDirection * Time.deltaTime);
		transform.Rotate (new Vector3 (0, Input.GetAxisRaw ("LookH")*50, 0) * mainCameraScript.lookSpeed * Time.deltaTime);
	}

	void ResettingCameraControls() //If the player is still moving when he's resetting the camera, the character's move are different, else there's a risk to see undesired behaviours.
	{
		if (Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) 
		{
			Debug.Log ("Everyday I'm resettin'");
			continueResetControls = true;

			Vector3 stickDirection = new Vector3 (horizontal, 0, vertical);
			float speedOut = stickDirection.sqrMagnitude;

			Quaternion target = Quaternion.Euler(0, floatDir, 0);
			tempMoveDir = target * Vector3.forward * speed;
			tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed);
			moveDirection.x = tempMoveDir.x;
			moveDirection.z = tempMoveDir.z;

			if (!controller.isGrounded)
					moveDirection.y -= gravity * Time.deltaTime;

			controller.Move (moveDirection * speedOut * Time.deltaTime);
		} 
		else //Once the stick has been released, we get back to the standard controls
			continueResetControls = false;
	}

	//This is where the magic happens, this method translate the left stick coordinates into world space coordinates, according to the camera's point of view!
	public void stickToWorldSpace(Transform root, Transform camera, ref Vector3 directionOut, ref float floatDirOut, ref float speedOut, bool outForAnim)
	{
		//We take the model's direction, the stick's direction, then we put in the square magnitude.
		Vector3 rootDirection = root.forward;
		Vector3 stickDirection = new Vector3(horizontal, 0, vertical);
		speedOut = stickDirection.sqrMagnitude;
		
		//Getting the camera's current rotation.
		Vector3 CameraDirection = camera.forward;
		CameraDirection.y = 0.0f;
		Quaternion referentialShift = Quaternion.FromToRotation (Vector3.forward, CameraDirection);
		
		//Conversion de l'input du joystick/clavier en coordonnées World.
		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);
		
		#region debug draw rays
		//These lines allow us to see how the vectors are handled for debug.
		
		/*Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), axisSign, Color.red);
		Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		Debug.DrawRay (new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);*/
		#endregion

		//Give the angle between the model's direction and the direction we give to it.
		float angleRootToMove = Vector3.Angle (rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f :1f);

		//DirectionOut will be useful to give the direction where the character have to go to the animator.
		floatDirOut = angleRootToMove;
	}

	//Setting everything in order to engage soul mode.
	void SwitchToSoulMode()
	{
		//Offset for spawn point based on the player's position.
		Vector3 soulSpawnOffset = new Vector3(0,2,-2);
		Vector3 soulSpawnPoint = transform.position + soulSpawnOffset;
		Transform spawnedSoul;
		
		spawnedSoul = Instantiate(Soul, soulSpawnPoint , transform.rotation) as Transform;
		soulMode = true;
	}
}
