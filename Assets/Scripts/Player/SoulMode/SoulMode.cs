using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoulMode : MonoBehaviour {

	#region movement variables
	float localDeltaTime;
	float horizontal;
	float vertical;
	float speed = 4;
	Vector3 direction = Vector3.zero;
	Vector3 tempMoveDir;
	Vector3 moveDirection = Vector3.zero;
	Vector3 faceDirection = Vector3.zero;
	float floatDir = 0f;
	public float maxSpeed = 5;
	Vector3 dashTarget;
	float dashingDistance = 10;
	public float heightOfJump = 8;
	public float gravity = 20;
	bool canDash = true;
	bool startDashCoolDown = false;
	public float dashCoolDown = 2;
	#endregion

	#region external scripts and object
	public GameObject ghostPrefab;
	CharacterController controller;
	GameObject player;
	PlayerController playerScript;
	ThirdPersonCamera mainCameraScript;
	GameObject soulBar;
	Slider soulBarSlide;
	#endregion

	#region other behaviour variables
	int currentGhostNumber = 0;
	[HideInInspector]
	public bool isDashing = false;
	float timer = 0;
	#endregion
	
	// Use this for initialization
	void Start () 
	{
		this.name = "Soul";
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
		player = GameObject.FindWithTag ("Player");
		playerScript = player.GetComponent<PlayerController> ();
		controller = this.GetComponent<CharacterController> ();

		soulBar = GameObject.Find ("SoulBar");
		soulBarSlide = soulBar.GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Time.timeScale = 0.1f;
		Time.fixedDeltaTime = 0.1f * 0.02f; //Make sure the physics simulation is still fluid.

		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		//Resetting back to body mode when pushing swith button or Soul Bar depleted.
		if (Input.GetButtonDown ("SwitchMode") || soulBarSlide.value <= 0)
			revertBack();

		if (Input.GetButtonDown ("Action") && !isDashing && canDash) 
		{
			mainCameraScript.camDirFromTarget = Camera.main.transform.position - this.transform.position; 
			isDashing = true;

			dashTarget = transform.position + transform.forward * dashingDistance;
		}

		if (isDashing) 
		{
			mainCameraScript.dashingSoul = true;
			Dash ();
		}
		else
		{
			move ();
			mainCameraScript.dashingSoul = false;
		}


		if (startDashCoolDown && controller.isGrounded) 
		{
			Timer ();
		}

		if (timer >= dashCoolDown) 
		{
			startDashCoolDown = false;
			timer = 0;
			canDash = true;
		}

		if (canDash)
			Debug.Log ("DASH");
		else
			Debug.Log ("NO DASH NO");

		Debug.Log("Timer = " +timer);
	}

	void Timer ()
	{
		timer += localDeltaTime;
	}

	//Toot toot Sonic Warrior, deeep in space and time. Toot toot Sonic Warrior, foreeever in your mind.
	//Nothing can surviiiive the wiiiiill to stay aliiive, cause if you tryyyyyy,
	//You can do anythiiiiing!
	void Dash()
	{
			transform.position = Vector3.Lerp (transform.position, dashTarget, 10 * localDeltaTime);

			float distance = (transform.position - dashTarget).sqrMagnitude;

			if (distance < .5f * 2) //If the soul got to its target point, let's leave dash mode.
			{
				isDashing = false;
				canDash = false;
				startDashCoolDown = true;
			}
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

	void move ()
	{
		//The rest of the update function is for the controls, which are exactly the same as in body mode.
		#region Get Axises
		//Get input from the main axis (Keyboard and stick)
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
		#endregion
		
		//This method will translate axis input into world coordinates, according to the camera's point of view.
		stickToWorldSpace (transform, mainCameraScript.transform, ref direction, ref floatDir, ref speed, false);
		
		Quaternion target = Quaternion.Euler (0, floatDir, 0);
		
		tempMoveDir = target * Vector3.forward * speed;
		tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed);
		
		moveDirection.x = tempMoveDir.x;
		moveDirection.z = tempMoveDir.z;
		
		if (Input.GetButton ("Jump") && controller.isGrounded)
			moveDirection.y = this.heightOfJump;
		
		if(!controller.isGrounded)
			moveDirection.y -= gravity * localDeltaTime;
		
		controller.Move (moveDirection * localDeltaTime);
		
		faceDirection = transform.position + moveDirection;
		faceDirection.y = transform.position.y;
		
		transform.LookAt (faceDirection);
	}

	void revertBack () //Revert Back to normal mode.
	{
		player.transform.position = transform.position;
		Time.timeScale = 1;
		Time.fixedDeltaTime = .02f;
		playerScript.soulMode = false;
		mainCameraScript.soulMode = false;
		Destroy (this.gameObject);
	}

}
