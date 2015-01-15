using UnityEngine;
using System.Collections;

public class PlayerController : CommonControls {

	//Direction & movement variables
	public float heightOfJump = 8;

	[SerializeField]
	private float setMaximumSpeed = 3;

	//External scripts and objects
	public GameObject Soul;

	//Other variables
	[HideInInspector]
	public bool aimingMode = false;
	[HideInInspector]
	public bool soulMode = false;

	//EXPERIMENTAL
	[HideInInspector]
	public bool setAimMode = true;
	public GameObject EarthQuakeParticles;


	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
		maxSpeed = setMaximumSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		Transform EarthQuakeParticlesInstance;

		if(Input.GetButtonDown("SwitchMode") && !soulMode)
			SwitchToSoulMode();

		if(Input.GetButtonDown("EarthQuake") && !soulMode)
			EarthQuakeParticlesInstance = Instantiate(EarthQuakeParticles, transform.position , Quaternion.Euler(90,0,0) ) as Transform;

		//Make the controls adapted to the current camera mode.
		if (!soulMode) 
		{
			if (Input.GetButtonDown ("AutoCam") || continueResetControls) //If the camera is resetting, the stick will only have control on the player's speed, not its direction
				ResettingCameraControls();
			else if (!aimingMode) //Else, and if we're in normal camera mode
				DefaultControls(heightOfJump, localDeltaTime);
			else if (aimingMode) //Else, and if we're in aiming camera mode
			{
				AimingControls ();
			}
		}
	}

	void AimingControls () //When aiming, the controls are not the same.
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
						moveDirection.y = this.heightOfJump;

		if(!controller.isGrounded)
			moveDirection.y -= gravity * Time.deltaTime;

		controller.Move (moveDirection * Time.deltaTime);
		transform.Rotate (new Vector3 (0, Input.GetAxisRaw ("LookH")*50, 0) * mainCameraScript.aimLookSpeed * Time.deltaTime);
	}

	//Setting everything in order to engage soul mode.
	void SwitchToSoulMode()
	{
		//Offset for spawn point based on the player's position.
		Vector3 soulSpawnOffset = new Vector3(0,.5f,0);
		Vector3 soulSpawnPoint = transform.position + soulSpawnOffset;
		Transform spawnedSoul;
		
		spawnedSoul = Instantiate(Soul, soulSpawnPoint , transform.rotation) as Transform;
		soulMode = true;
		mainCameraScript.soulMode = true;
	}
}
