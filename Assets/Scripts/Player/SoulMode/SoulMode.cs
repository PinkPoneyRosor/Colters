using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoulMode : CommonControls {

	#region movement variables
	[SerializeField]
	private float setMaximumSpeed = 5;
	Vector3 dashTarget = Vector3.zero;
	float dashingDistance = 10;
	public float heightOfJump = 8;
	bool canDash = true;
	bool startDashCoolDown = false;
	public float dashCoolDown = 2;
	#endregion

	#region external scripts and object
	public GameObject ghostPrefab;
	GameObject player;
	PlayerController playerScript;
	GameObject soulBar;
	Slider soulBarSlide;
	#endregion

	#region other behaviour variables
	[HideInInspector]
	public bool isDashing = false;
	float timer = 0;
	public LayerMask dashGetThrough;
	#endregion

	#region Dash Vars
	public float dashSpeed = 10;
	public float dashDistance = 10;
	float dashTimer;
	Vector3 dashDirection;
	#endregion
	
	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		maxSpeed = setMaximumSpeed;

		this.name = "Soul";
		player = GameObject.FindWithTag ("Player");
		playerScript = player.GetComponent<PlayerController> ();

		soulBar = GameObject.Find ("SoulBar");
		soulBarSlide = soulBar.GetComponent<Slider> ();

		dashTimer = dashDistance / dashSpeed;
	}

	// Update is called once per frame
	void Update () 
	{
		Time.timeScale = 0.1f;
		Time.fixedDeltaTime = 0.1f * 0.02f; //Make sure the physics simulation is still fluid.

		GetAxis ();

		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		//Resetting back to body mode when pushing swith button or Soul Bar depleted.
		if (Input.GetButtonDown ("SwitchMode") || soulBarSlide.value <= 0)
			revertBack();

		if (Input.GetButtonDown ("Action") && !isDashing && canDash && !aimingMode) 
		{
			mainCameraScript.camDirFromTarget = Camera.main.transform.position - this.transform.position; 
			isDashing = true;

			dashTarget = transform.position + transform.forward * dashingDistance;
			dashDirection = dashTarget - transform.position;
		}

		#region Controls according to situation
		if ((Input.GetButtonDown ("AutoCam") || continueResetControls) && !isDashing) //If the camera is resetting, the stick will only have control on the player's speed, not its direction
			ResettingCameraControls();
		else if (isDashing) 
		{
			mainCameraScript.dashingSoul = true;
			Dash ();
		}
		else if (aimingMode) //Else, and if we're in aiming camera mode
		{
			AimingControls (heightOfJump);
		}
		else
		{
			DefaultControls(heightOfJump, localDeltaTime);
			mainCameraScript.dashingSoul = false;
		}
		#endregion


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
		if (dashTimer > 0) 
		{
			dashTimer -= localDeltaTime;
			controller.Move (dashDirection.normalized * localDeltaTime * dashSpeed);
		} else {
			isDashing = false;
			canDash = false;
			startDashCoolDown = true;
			dashTimer = dashDistance / dashSpeed;
		}
	}

	void revertBack () //Revert Back to normal mode.
	{
		player.transform.position = transform.position;
		Time.timeScale = 1;
		Time.fixedDeltaTime = .02f;
		playerScript.soulMode = false;
		mainCameraScript.SwitchPlayerMode( false );
		Destroy (this.gameObject);
	}
}
