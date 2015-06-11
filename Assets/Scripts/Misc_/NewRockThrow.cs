using UnityEngine;
using System.Collections;

public class NewRockThrow : MonoBehaviour {
	
	private PlayerController playerScript;
	
	public GameObject[] allLaunchedRocks = new GameObject[4];
	
	public GameObject RockPrefab;
	public GameObject ExplosivePrefab;
	public float explosiveRockLoadTime = 1;
	
	private int launchCount = 0;

	Transform mainCamera;
	
	private GameObject [] allRocks;
	private GameObject [] allSpawners;
	
	public LayerMask RockLayer;
	public LayerMask otherLayers;
	bool canThrow = true;
	
	public float globalPickUpRadius = 5;
	
	#region Selected Rocks Positions
	[HideInInspector]
	public Vector3 firstOffset;
	[HideInInspector]
	public Vector3 secondOffset;
	[HideInInspector]
	public Vector3 thirdOffset;
	[HideInInspector]
	public Vector3 fourthOffset;
	#endregion
	
	#region particle effects
	public GameObject explosiveFinishLoadParticles;
	#endregion
	
	private bool loopCrush = false;
	private GameObject HudObject;
	private GUImainBehaviour HudScript;
	
	private float holdDownThrowTime = 0;
	private float localDeltaTime;
	private bool justHitThrowButton = false;
	private bool nextRockWillBeExplosive = false;
	
	private ParticleSystem explosiveLoadParticles;
	private bool startFinishedLoadparticle = true;
	
	public GameObject armLoadingParticles;
	public GameObject armSpawnRockParticles;
	
	public GameObject swoosh;
	
	GameObject spawnedArmLoading;
	
	Animator animator;
	
	// Use this for initialization
	void Start () 
	{
		mainCamera = Camera.main.transform;
		playerScript = this.GetComponent <PlayerController>();
		
		HudObject = GameObject.Find ("GameHUD");
		HudScript = HudObject.GetComponent <GUImainBehaviour>();
		
		explosiveLoadParticles = this.GetComponent <ParticleSystem>();
		
		animator = gameObject.GetComponent <Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!HudScript.paused)
		{
			//Setting this object's local delta time...
			localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;
			
			if (HudScript.rockPercent <= .20f)
				canThrow = false;
			else
				canThrow = true;
		
			if(canThrow)
			{
				if (Input.GetAxisRaw("RockThrow") != 0)
					HoldingThrowButton();
				else if (Input.GetButtonDown ("Melee Attack") || loopCrush || Input.GetKeyDown ("e"))
				    ShortRangeAttack();
				    
				if (Input.GetAxisRaw("RockThrow") == 0)
				{
					if (spawnedArmLoading != null)
						Destroy (spawnedArmLoading);
				}
				  
				#region Gonna throw a rock
				if (Input.GetAxisRaw("RockThrow") == 0 && justHitThrowButton)
				{
					explosiveLoadParticles.Stop ();
					startFinishedLoadparticle = true;
				
					holdDownThrowTime = 0;
					justHitThrowButton = false;
				
					if (nextRockWillBeExplosive && HudScript.rockPercent == 1)
						ThrowRock (true);
					else
						ThrowRock (false);
				}
				#endregion
			}
		}
	}
	
	void HoldingThrowButton ()
	{
		holdDownThrowTime += localDeltaTime;
		
		if (spawnedArmLoading == null && armLoadingParticles != null)
		{
		spawnedArmLoading = Instantiate(armLoadingParticles, transform.position, Quaternion.identity) as GameObject;
		spawnedArmLoading.transform.SetParent(this.transform);
		}
		
		
		justHitThrowButton = true;
		
		if (holdDownThrowTime >= .2f)
		{
			explosiveLoadParticles.Play ();
		}
		
		if (holdDownThrowTime >= explosiveRockLoadTime - .2f)
		{
			explosiveLoadParticles.Stop ();
		}
		
		if (holdDownThrowTime >= explosiveRockLoadTime)
		{
			nextRockWillBeExplosive = true;
			
			if (startFinishedLoadparticle && HudScript.rockPercent == 1)
			{
				GameObject particlesLoadEnd;
				HudScript.NextRockExplosive = true;
				particlesLoadEnd = Instantiate(explosiveFinishLoadParticles, transform.position + transform.up, Quaternion.identity) as GameObject;
				particlesLoadEnd.transform.SetParent (this.transform);
				startFinishedLoadparticle = false;
			}
		}
		else
		{
			nextRockWillBeExplosive = false;
		}
	}
	
	void ShortRangeAttack()
	{
		Vector3 startPos = (transform.position + transform.up * 2) + transform.forward * 1.5f;
		GameObject thrownRock = Instantiate (RockPrefab, startPos, Quaternion.identity) as GameObject;
		
		NewThrowableRock currentThrowedRockScript;
		
		currentThrowedRockScript = thrownRock.GetComponent <NewThrowableRock> ();
		currentThrowedRockScript.Melee = true;

		//Let's throw the rock downward
		Vector3 throwDirection = -Vector3.up;
		
		//This line is just to make absolutely sure there is no more constraints so that we can throw the rock in a straight line.
		currentThrowedRockScript.rigidbody.constraints = RigidbodyConstraints.None;
		
		playerScript.SendMessage ("CrushImpulse");
		
		thrownRock.rigidbody.constantForce.force = throwDirection * currentThrowedRockScript.throwForce;
		
		currentThrowedRockScript.beingThrowned = true;
		
		launchCount ++;
		
		if(playerScript.controller.isGrounded)
		HudScript.rockPercent -= .25f;
		else
		HudScript.rockPercent = 0;
		
		animator.SetTrigger ("RockPunch");
		
		Instantiate (swoosh, transform.position, Quaternion.identity);
		
		if(launchCount > 4)
		{
			ShiftRockArray (thrownRock);
		}
		else if (launchCount == 1)
		{
			allLaunchedRocks[0] = thrownRock;
		}
		else if (launchCount == 2)
		{
			allLaunchedRocks[1] = thrownRock;
		}
		else if (launchCount == 3)
		{
			allLaunchedRocks[2] = thrownRock;
		}
		else if (launchCount == 4)
		{
			allLaunchedRocks[3] = thrownRock;
		}
	}
	
	void ThrowRock(bool explosive)
	{
		Vector3 startPos = transform.position + transform.up * 2.5f + mainCamera.forward;
		GameObject thrownRock;
		
		if(!explosive)
			thrownRock = Instantiate (RockPrefab, startPos, Quaternion.identity) as GameObject;
		else
			thrownRock = Instantiate (ExplosivePrefab, startPos, Quaternion.identity) as GameObject;
			
		HudScript.NextRockExplosive = false;
		
		RaycastHit HitObject;
		NewThrowableRock currentThrowedRockScript;
		Ray ray;
		
		ray = mainCamera.camera.ScreenPointToRay (new Vector3(Screen.width/2, Screen.height/2, 0));
	
		currentThrowedRockScript = thrownRock.GetComponent <NewThrowableRock> ();
			
		if (Physics.Raycast (ray.origin, ray.direction, out HitObject, Mathf.Infinity, otherLayers)
		    && HitObject.transform.CompareTag ("Enemy") 
		    && HitObject.transform.GetComponent <BasicEnemy> ().canGetHit)
		{ 
		  //If what we aimed at is an enemy and that it's not knocked out, let's do a homing attack
			currentThrowedRockScript.aimHoming = HitObject.transform;
			currentThrowedRockScript.homingAttackBool = true;
		}
		else 
		{
			//If what we aimed at is not an enemy or it is but he's knocked out, just throw the rock straightforward.
			Vector3 throwDirection = ray.direction;
			throwDirection.Normalize ();
			
			//This line is just to make absolutely sure there is no more constraints so that we can throw the rock in a straight line.
			currentThrowedRockScript.rigidbody.constraints = RigidbodyConstraints.None;
			
			thrownRock.rigidbody.constantForce.force = throwDirection * currentThrowedRockScript.throwForce;
			
			if (armSpawnRockParticles != null)
			Instantiate (armSpawnRockParticles, transform.position, Quaternion.identity);
			
			animator.SetTrigger("ThrowRock");
		}
		
		launchCount ++;
		
		if(!explosive)
		HudScript.rockPercent -= .25f;
		else
		HudScript.rockPercent = 0;
		
		if(launchCount > 4)
		{
			ShiftRockArray (thrownRock);
		}
		else if (launchCount == 1)
		{
			allLaunchedRocks[0] = thrownRock;
		}
		else if (launchCount == 2)
		{
			allLaunchedRocks[1] = thrownRock;
		}
		else if (launchCount == 3)
		{
			allLaunchedRocks[2] = thrownRock;
		}
		else if (launchCount == 4)
		{
			allLaunchedRocks[3] = thrownRock;
		}
	}
	
	void ShiftRockArray(GameObject newFirstRock)
	{
		Destroy (allLaunchedRocks[3].gameObject);
		allLaunchedRocks[3] = allLaunchedRocks[2];
		allLaunchedRocks[2] = allLaunchedRocks[1];
		allLaunchedRocks[1] = allLaunchedRocks[0];
		allLaunchedRocks[0] = newFirstRock;
	}
}
