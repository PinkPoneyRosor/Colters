using UnityEngine;
using System.Collections;

public class PlayerController : CommonControls {

	//Direction & movement variables
	public float heightOfJump = 8;

	[SerializeField]
	private float setMaximumSpeed = 3;

	//External scripts and objects
	public GameObject Soul;
	[HideInInspector]
	public GameObject EarthQuakeParticles;


	//Other variables
	[HideInInspector]
	public bool soulMode = false;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		maxSpeed = setMaximumSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		if(Input.GetButtonDown("SwitchMode") && !soulMode)
			SwitchToSoulMode();

		if(Input.GetButtonDown("EarthQuake") && !soulMode)
			Instantiate(EarthQuakeParticles, transform.position , Quaternion.Euler(90,0,0) );

		//Make the controls adapted to the current camera mode.
		if (!soulMode) 
		{
			if (Input.GetButtonDown ("AutoCam") || continueResetControls) //If the camera is resetting, the stick will only have control on the player's speed, not its direction
				ResettingCameraControls();
			else if (!aimingMode) //Else, and if we're in normal camera mode
				DefaultControls(heightOfJump, localDeltaTime);
			else if (aimingMode) //Else, and if we're in aiming camera mode
			{
				AimingControls (heightOfJump);
			}
		}
	}

	//Setting everything in order to engage soul mode.
	void SwitchToSoulMode()
	{
		//Offset for spawn point based on the player's position.
		Vector3 soulSpawnOffset = new Vector3(0,.5f,0);
		Vector3 soulSpawnPoint = transform.position + soulSpawnOffset;

		Instantiate(Soul, soulSpawnPoint , transform.rotation);
		soulMode = true;
		mainCameraScript.SwitchPlayerMode( true );
	}
}
