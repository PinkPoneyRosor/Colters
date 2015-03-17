using UnityEngine;
using System.Collections;

public class CommonControls : MonoBehaviour {

	[HideInInspector]
	public float horizontal = 0;
	[HideInInspector]
	public float vertical = 0;
	[HideInInspector]
	public Vector3 direction = Vector3.zero;
	[HideInInspector]
	public Vector3 faceDirection = Vector3.zero;
	[HideInInspector]
	public Vector3 tempMoveDir;
	[HideInInspector]
	public Vector3 moveDirection = Vector3.zero;
	[HideInInspector]
	public CharacterController controller;
	[HideInInspector]
	public ThirdPersonCamera mainCameraScript;
	[HideInInspector]
	public bool continueResetControls = false;
	[HideInInspector]
	public float floatDir = 0f;
	[HideInInspector]
	public float speed = 3f;
	[HideInInspector]
	public float localDeltaTime;
	[HideInInspector]
	public bool setAimMode = true;
	[HideInInspector]
	public static bool aimingMode = false;
	[HideInInspector]
	public bool canJump = true;
	[HideInInspector]
	public float airControlMultiplier = 50;
	[HideInInspector]
	public static float maxJumpAngle = 35;
	
	[HeaderAttribute("Moves parameters")]
	public float maxSpeed = 0;
	public float gravity = 20;

	// Use this for initialization
	protected virtual void Start () 
	{
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
		controller = GetComponent<CharacterController>();
	}

	public void GetAxis()
	{
		#region Get Axises
		//Get input from the main axis (Keyboard and stick)
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
		#endregion
	}
	
	public void DefaultControls (float heightOfJump, float localDeltaTime)
	{
		#region default Controls
		#region setting essential variables each frame
		
		//This method will translate axis input into world coordinates, according to the camera's point of view.
		stickToWorldSpace (transform, mainCameraScript.transform, ref direction, ref floatDir, ref speed, false, horizontal, vertical);
		#endregion
		
		Quaternion target = Quaternion.Euler (0, floatDir, 0);
		if(controller.isGrounded)
		{
			tempMoveDir = target * Vector3.forward * speed;
			tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed);
		}
		else
		{
			tempMoveDir += direction * airControlMultiplier * localDeltaTime;
			tempMoveDir = Vector3.ClampMagnitude(tempMoveDir, maxSpeed);
		}
		
		//Debug.Log ("moveDirection = "+moveDirection);
		
		moveDirection.x = tempMoveDir.x;
		moveDirection.z = tempMoveDir.z;

		RaycastHit hit;
		if(Physics.Raycast (transform.position, -Vector3.up, out hit, Mathf.Infinity))
		{
			float angle = Vector3.Angle(-hit.normal, -Vector3.up);
			if(angle>maxJumpAngle)
				canJump = false;
			else
				canJump = true;
		}
		
		if (Input.GetButtonDown ("Jump") && controller.isGrounded && canJump)
			moveDirection.y = heightOfJump;
		
		#region apply movements & gravity
		if(!controller.isGrounded)
			moveDirection.y -= gravity * localDeltaTime;
		
		controller.Move (moveDirection * localDeltaTime);
		
		faceDirection = transform.position + moveDirection;
		faceDirection.y = transform.position.y;
		
		transform.LookAt (faceDirection);
		#endregion
		#endregion
	}

	//This is where the magic happens, this method translate the left stick coordinates into world space coordinates, according to the camera's point of view!
	public void stickToWorldSpace(Transform root, Transform camera, ref Vector3 directionOut, ref float floatDirOut, ref float speedOut, bool outForAnim, float horizontal, float vertical)
	{
		//We take the model's direction, the stick's direction, then we put in the square magnitude.
		Vector3 rootDirection = root.forward;
		Vector3 stickDirection = new Vector3(horizontal, 0, vertical);
		speedOut = Mathf.Clamp(stickDirection.sqrMagnitude,0,1);
		
		//Getting the camera's current rotation.
		Vector3 CameraDirection = camera.forward;
		CameraDirection.y = 0.0f;
		Quaternion referentialShift = Quaternion.FromToRotation (Vector3.forward, CameraDirection);
		
		//Conversion de l'input du joystick/clavier en coordonnées World.
		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);
		
		directionOut = moveDirection;
		
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

	public void ResettingCameraControls() //If the player is still moving when he's resetting the camera, the character's move are different, else there's a risk to see undesired behaviours.
	{
		if ( (Input.GetAxisRaw ("Horizontal") < -.25f && Input.GetAxisRaw ("Horizontal") > .25f) || Input.GetAxisRaw ("Vertical") <= -.2f) 
		{
			continueResetControls = true;
			
			Vector3 stickDirection = new Vector3 (horizontal, 0, vertical);
			float speedOut = Mathf.Clamp(stickDirection.sqrMagnitude,0,1);
			
			Quaternion target = Quaternion.Euler(0, floatDir, 0);
			tempMoveDir = target * Vector3.forward * speed;
			tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed);
			moveDirection.x = tempMoveDir.x;
			moveDirection.z = tempMoveDir.z;
			
			if (!controller.isGrounded)
				moveDirection.y -= gravity * localDeltaTime;
			
			controller.Move (moveDirection * speedOut * localDeltaTime);
		} 
		else //Once the stick has been released, we get back to the standard controls
			continueResetControls = false;
	}

	public void AimingControls (float heightOfJump) //When aiming, the controls are not the same.
	{
		if (setAimMode) 
		{
			this.transform.eulerAngles = new Vector3 (transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
			setAimMode = false;
		}
		
		tempMoveDir = (transform.right * horizontal + transform.forward * vertical) * maxSpeed;
		moveDirection.x = tempMoveDir.x;
		moveDirection.z = tempMoveDir.z;
		
		if (Input.GetButton ("Jump") && controller.isGrounded)
			moveDirection.y = heightOfJump;
		
		if(!controller.isGrounded)
			moveDirection.y -= gravity * localDeltaTime;
		
		controller.Move (moveDirection * localDeltaTime);
		transform.Rotate (new Vector3 (0, Input.GetAxisRaw ("LookH")*50, 0) * mainCameraScript.aimLookSpeed * localDeltaTime);
	}
}
