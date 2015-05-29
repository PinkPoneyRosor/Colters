using UnityEngine;
using System.Collections;

public class PlayerController : CommonControls {

	//Direction & movement variables
	public float heightOfJump = 8;

	[SerializeField]
	private float setMaximumSpeed = 3;

	//External scripts and objects
	public GameObject Soul;
	
	public float maxHealth = 10;
	public float currentHealth;

	//Other variables
	[HideInInspector]
	public bool soulMode = false;

	[SerializeField]
	private float maxSlopeAngleToJump = 35;
	
	private Vector3 startPosition;
	
	[HideInInspector]
	public bool dead = false;
	
	public GameObject footStep;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		maxSpeed = setMaximumSpeed;
		maxJumpSlopeAngle = maxSlopeAngleToJump;
		
		startPosition = transform.position;
		
		currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!dead)
		{
			//localDeltaTime allows the script to not be influenced by the time scale change.
			localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;
		
			GetAxis ();
		
			if(Input.GetButtonDown("SwitchMode") && !soulMode)
				SwitchToSoulMode();
		
			//Make the controls adapted to the current camera mode.
			if (!soulMode) 
			{
				if (mainCameraScript.resetCameraPosition && !mainCameraScript.justHitAWall) //If the camera is resetting, the stick will only have control on the player's speed, not its direction
					ResettingCameraControls();
				else//Else, and if we're in normal camera mode
					DefaultControls (heightOfJump, localDeltaTime);
			}
		}
	}

	//Setting everything in order to engage soul mode.
	void SwitchToSoulMode()
	{
		//Offset for spawn point based on the player's position.
		Vector3 soulSpawnOffset = new Vector3(0, 1f, 0);
		Vector3 soulSpawnPoint = transform.position + soulSpawnOffset;
		GameObject spawnedSoul = Instantiate(Soul, soulSpawnPoint , transform.rotation) as GameObject;
		soulMode = true;
		mainCameraScript.SwitchPlayerMode( spawnedSoul, true );
	}
	
	void GetHurt (float damageAmount)
	{
		currentHealth -= damageAmount;
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit) 
	{ 
		// Make sure we are really standing on a straight platform 
		// Not on the underside of one and not falling down from it either! 
		if (hit.moveDirection.y < -0.9 && hit.normal.y > 0.5) 
			activePlatform = hit.collider.transform;
	}
	
	public void Die ()
	{
		animator.SetBool ("Dead", true);
		
		dead = true;
		
		StartCoroutine(RespawnCountDown());
	}
	
	IEnumerator RespawnCountDown()
	{
		yield return new WaitForSeconds (3);
		
		animator.SetBool ("Dead", false);
		
		if (checkPointOn)
			transform.position = lastCheckpointPosition + Vector3.up; //ayo !
		else
			transform.position = startPosition + Vector3.up * 1.5f;
		
		currentHealth = maxHealth;
		
		dead = false;
	}
	
	void CrushImpulse ()
	{
		if(!controller.isGrounded)
			moveDirection.y = heightOfJump;
		else
			moveDirection.y = heightOfJump;
	}
	
	void FootStep ()
	{
		Instantiate (footStep);
	}
}
