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
	float localDeltaTime;
	CharacterController controller;
	[HideInInspector]
	public bool aimingMode = false;
	[HideInInspector]
	public bool soulMode = false;


	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
	}
	
	// Update is called once per frame
	void Update () {

		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		#region Get Axises
		//Get input from the main axis (Keyboard and stick)
		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");
		#endregion

		if(Input.GetButtonDown("SwitchMode") && !soulMode)
			SwitchToSoulMode();

		//Make the controls adapted to the current camera mode.
		if (!soulMode) 
		{
			if (!aimingMode)
				DefaultControls ();
			else
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

	void AimingControls ()
	{
		controller.Move ((transform.right * horizontal + transform.forward * vertical) * Time.deltaTime);
		transform.Rotate (new Vector3 (0, Input.GetAxis ("LookH")*50, 0) * Time.deltaTime);
		}

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
		//Ces lignes permettent de visualiser la façon dont sont gérés les vecteurs dans la fonction StickToWorldSpace (Debug)
		
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

	void SwitchToSoulMode()
	{
		//Décalage du point de spawn de l'ame par rapport à Phalene...
		Vector3 soulSpawnOffset = new Vector3(0,2,-2);
		Vector3 soulSpawnPoint = transform.position + soulSpawnOffset;
		Transform spawnedSoul;
		
		spawnedSoul = Instantiate(Soul, soulSpawnPoint , transform.rotation) as Transform;
		soulMode = true;
	}
}
