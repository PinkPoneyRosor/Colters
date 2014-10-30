using UnityEngine;
using System.Collections;

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
	#endregion

	#region external scripts and object
	public GameObject ghostPrefab;
	CharacterController controller;
	GameObject player;
	ThirdPersonCamera mainCameraScript;
	BirdsEyeCam birdsEyeScript;
	#endregion

	#region other behaviour variables
	int currentGhostNumber = 0;
	#endregion
	
	// Use this for initialization
	void Start () {
		this.name = "Soul";
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
		birdsEyeScript = Camera.main.GetComponent<BirdsEyeCam> ();
		mainCameraScript.birdsEyeActivated = true;
		birdsEyeScript.followBody = false;
		player = GameObject.FindWithTag ("Player");
		controller = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Time.timeScale = 0.1f;
		Time.fixedDeltaTime = 0.1f * 0.02f; //Make sure the physics simulation is still fluid.

		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		//Resetting back to body mode when pushing swith button...
		if (Input.GetButtonDown ("SwitchMode")) 
		{
			player.GetComponent<ghostFollow>().enabled = true;
			player.GetComponent<ghostFollow>().justGotActivated = true;
			birdsEyeScript.followBody = true;
			currentGhostNumber = 0;

			Destroy (this.gameObject);
		}

		if(Input.GetButtonDown("Action"))
			placeGhost();

		//The rest of the update function is for the controls, which are exactly the same as in body mode.
		#region Get Axises
		//Get input from the main axis (Keyboard and stick)
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
		#endregion

		//This method will translate axis input into world coordinates, according to the camera's point of view.
		stickToWorldSpace(transform, mainCameraScript.transform, ref direction, ref floatDir, ref speed, false);

		Quaternion target = Quaternion.Euler(0, floatDir, 0);

		tempMoveDir = target * Vector3.forward * speed;
		tempMoveDir = transform.TransformDirection (tempMoveDir * maxSpeed);

		moveDirection.x = tempMoveDir.x;
		moveDirection.z = tempMoveDir.z;

		controller.Move(moveDirection * localDeltaTime);
		
		faceDirection = transform.position + moveDirection;
		faceDirection.y = transform.position.y;
		
		transform.LookAt (faceDirection);
	}

	//This will place a ghost right under the soul cursor.
	//We also check if the environment allows us to place a ghost that won't glitch the game's behaviour.
	void placeGhost()
	{
		RaycastHit pointHit;
		if (Physics.Raycast (transform.position, -transform.up,out pointHit, 50)) 
		{
			GameObject placedGhost = Instantiate (ghostPrefab, pointHit.point + new Vector3(0,1,0), this.transform.rotation) as GameObject;
			placedGhost.name = "actionGhost_"+currentGhostNumber;
			currentGhostNumber++;
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

}
