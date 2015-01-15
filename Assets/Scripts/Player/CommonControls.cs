using UnityEngine;
using System.Collections;

public class CommonControls : MonoBehaviour {

	public float horizontal = 0;
	public float vertical = 0;

	public Vector3 direction = Vector3.zero;
	public Vector3 faceDirection = Vector3.zero;
	public Vector3 tempMoveDir;
	public Vector3 moveDirection = Vector3.zero;
	public CharacterController controller;
	public ThirdPersonCamera mainCameraScript;
	public bool continueResetControls = false;
	public float floatDir = 0f;
	public float speed = 3f;
	public float maxSpeed = 0;
	public float gravity = 20;
	public float localDeltaTime;


	// Use this for initialization
	void Start () 
	{
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DefaultControls (float heightOfJump, float localDeltaTime)
	{
		#region Get Axises
		//Get input from the main axis (Keyboard and stick)
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
		#endregion

		#region default Controls
		#region setting essential variables each frame
		
		//This method will translate axis input into world coordinates, according to the camera's point of view.
		stickToWorldSpace (transform, mainCameraScript.transform, ref direction, ref floatDir, ref speed, false, horizontal, vertical);
		#endregion
		
		Quaternion target = Quaternion.Euler (0, floatDir, 0);
		tempMoveDir = target * Vector3.forward * speed;
		tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed);
		moveDirection.x = tempMoveDir.x;
		moveDirection.z = tempMoveDir.z;
		
		if (Input.GetButton ("Jump") && controller.isGrounded)
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
		if (Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") <= 0) 
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
}
