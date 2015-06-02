using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoulMode : CommonControls {

	#region movement variables
	[SerializeField]
	private float setMaximumSpeed = 5;
	public float heightOfJump = 8;
	#endregion

	#region external scripts and object
	GameObject player;
	PlayerController playerScript;
	GameObject soulBar;
	Slider soulBarSlide;
	#endregion
	
	private bool climbRock = false;
	private GameObject currentClimbingRock;
	private NewThrowableRock currentClimbRockScript;
	private float gravitySave;
	
	public GameObject footStep;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start ();
		maxSpeed = setMaximumSpeed;

		this.name = "Soul";
		player = GameObject.Find ("Player");
		playerScript = player.GetComponent<PlayerController> ();

		soulBar = GameObject.Find ("SoulBar");
		soulBarSlide = soulBar.GetComponent<Slider> ();
	}

	// Update is called once per frame
	void Update () 
	{
	
		Time.timeScale = 0.1f;
		Time.fixedDeltaTime = 0.1f * 0.02f; //Make sure the physics simulation is still fluid.
		
		if (climbRock)
			if(currentClimbRockScript.beingThrowned)
			{
				Vector3 rockClimbOffset = new Vector3 (0, 1, 0);
				transform.position = Vector3.MoveTowards(transform.position, currentClimbingRock.transform.position + rockClimbOffset, localDeltaTime * 5);
				gravity = 0;
			}
			else
			{
				climbRock = false;
				controller.enabled = true;
				gravity = gravitySave;
			}

		GetAxis ();

		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		//Resetting back to body mode when pushing swith button or Soul Bar depleted.
		if (Input.GetButtonDown ("SwitchMode") || soulBarSlide.value <= 0)
			revertBack(true);
		else if (Input.GetButtonDown ("SoulToBody"))
			revertBack (false);

		#region Controls according to situation
		if ((Input.GetButtonDown ("AutoCam") || continueResetControls)) //If the camera is resetting, the stick will only have control on the player's speed, not its direction
			ResettingCameraControls();
		else
		{
			DefaultControls(heightOfJump, localDeltaTime);
			mainCameraScript.dashingSoul = false;
		}
		#endregion
	}

	void revertBack (bool bodyToSoul) //Revert Back to normal mode.
	{
		Vector3 revertBackOffset = new Vector3 (0, .5f, 0); 
		
		if(bodyToSoul)
			player.transform.position = transform.position + revertBackOffset;
		else
			transform.position = player.transform.position + revertBackOffset;
			
		Time.timeScale = 1;
		Time.fixedDeltaTime = .02f;
		playerScript.soulMode = false;
		mainCameraScript.SwitchPlayerMode( player.gameObject, false );
		Destroy (this.gameObject);
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		// Make sure we are really standing on a straight platform 
		// Not on the underside of one and not falling down from it either! 
		if (hit.moveDirection.y < -0.9 && hit.normal.y > 0.5) 
			activePlatform = hit.collider.transform;
		
		if (hit.collider.CompareTag("ThrowableRock"))
		{
			NewThrowableRock hitScript = hit.transform.GetComponent <NewThrowableRock>();
			
			if (hitScript.beingThrowned)
			{
				Physics.IgnoreCollision (this.collider, hit.collider);
				controller.enabled = false;
				climbRock = true;
				currentClimbingRock = hit.gameObject;
				currentClimbRockScript = currentClimbingRock.GetComponent <NewThrowableRock> ();
				gravitySave = gravity;
			}
		}
	}
	
	void FootStep ()
	{
		Instantiate (footStep);
	}
}
