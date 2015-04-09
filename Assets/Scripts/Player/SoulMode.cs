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
