using UnityEngine;
using System.Collections;

public class NewRockThrow : MonoBehaviour {
	
	private PlayerController playerScript;
	
	public GameObject[] allLaunchedRocks = new GameObject[4];
	
	public GameObject RockPrefab;
	public GameObject ExplosivePrefab;
	
	public float energyPerRockLaunched = 0.1f;
	
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
	
	private bool loopCrush = false;
	private GameObject HudObject;
	private GUImainBehaviour HudScript;
	
	// Use this for initialization
	void Start () 
	{
		mainCamera = Camera.main.transform;
		playerScript = this.GetComponent <PlayerController>();
		
		HudObject = GameObject.Find ("GameHUD");
		HudScript = HudObject.GetComponent <GUImainBehaviour>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (HudScript.rockBarSlide.value <= 0)
			canThrow = false;
	
		if(canThrow)
		{
			if (Input.GetAxisRaw("RockThrow") != 0 || Input.GetButton("Action"))
				ThrowRock(false);
			else if (Input.GetButton("ExplosiveThrow"))
				ThrowRock(true);
			else if (Input.GetButton ("Melee Attack") || loopCrush || Input.GetKey ("e"))
			    ShortRangeAttack();
		}
	}
	
	void ShortRangeAttack()
	{
		Vector3 startPos = (transform.position + transform.up * 2) + transform.forward * 1.5f;
		GameObject thrownRock = Instantiate (RockPrefab, startPos, Quaternion.identity) as GameObject;
		NewThrowableRock currentThrowedRockScript;
		
		canThrow = false;	
		currentThrowedRockScript = thrownRock.GetComponent <NewThrowableRock> ();

		//Let's throw the rock downward
		Vector3 throwDirection = -Vector3.up + transform.forward;
		
		//This line is just to make absolutely sure there is no more constraints so that we can throw the rock in a straight line.
		currentThrowedRockScript.rigidbody.constraints = RigidbodyConstraints.None;
		
		playerScript.SendMessage ("CrushImpulse");
		
		thrownRock.rigidbody.constantForce.force = throwDirection * currentThrowedRockScript.throwForce;
		
		currentThrowedRockScript.beingThrowned = true;
		
		launchCount ++;
		
		HudScript.rockBarSlide.value -= energyPerRockLaunched;
		
		if(launchCount > 4)
		{
			ShiftRockArray (thrownRock);
		}
		else if (launchCount == 1)
		{
			allLaunchedRocks[1] = thrownRock;
		}
		else if (launchCount == 2)
		{
			allLaunchedRocks[2] = thrownRock;
		}
		else if (launchCount == 3)
		{
			allLaunchedRocks[3] = thrownRock;
		}
		else if (launchCount == 4)
		{
			allLaunchedRocks[4] = thrownRock;
		}
		
		StartCoroutine("CoolDown");
	}
	
	void ThrowRock(bool explosive)
	{
		Vector3 startPos = transform.position + transform.up * 2;
		GameObject thrownRock;
		
		if(!explosive)
			thrownRock = Instantiate (RockPrefab, startPos, Quaternion.identity) as GameObject;
		else
			thrownRock = Instantiate (ExplosivePrefab, startPos, Quaternion.identity) as GameObject;
		
		RaycastHit HitObject;
		NewThrowableRock currentThrowedRockScript;
		Ray ray;
		
		ray = mainCamera.camera.ScreenPointToRay (new Vector3(Screen.width/2, Screen.height/2, 0));
	
		canThrow = false;	
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
		}
		
		launchCount ++;
		
		HudScript.rockBarSlide.value -= energyPerRockLaunched;
		
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
		
		StartCoroutine("CoolDown");
	}

	IEnumerator CoolDown() 
	{
		yield return new WaitForSeconds(1f);
		canThrow = true;
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
