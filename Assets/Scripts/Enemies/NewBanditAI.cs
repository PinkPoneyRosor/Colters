using UnityEngine;
using System.Collections;

public class NewBanditAI : MonoBehaviour 
{
	
	[HideInInspector]
	public enum EnemyMode { BowAttack, Chasing }; //Define the current state of the enemy
	[HideInInspector]
	public EnemyMode CurrentMode = EnemyMode.BowAttack;
	
	private NewBanditSight sightScript;
	
	[HideInInspector]
	public GameObject player;
	
	private Quaternion desiredRotation;
	public float turnSpeed = 5f;
	public float reloadTime = 1f;
	public GameObject myProjectile;
	public Animator animator;
	
	[HideInInspector]
	public float nextFireTime;
	
	[SerializeField]
	private float minRockVelocityToGetHurt = 5;
	
	public NavMeshAgent navAgent;
	
	GameObject hud;
	GUImainBehaviour guiScript;
	
	public GameObject bow;
	public GameObject sword;
	
	public MeshRenderer bowMesh;
	public MeshRenderer swordMesh;
	
	private bool wasAfar = false;
	
	bool wasWieldingSword = false;
	bool wasWieldingBow = false;
	
	bool canHit = true;
	
	public Vector3 newEnGardePosition;
	
	public bool newEnGarde = true;
	
	private float setSpeed;
	
	public GameObject ragdoll;
	
	void Start ()
	{
		player = GameObject.Find ("Player");
		sightScript = this.gameObject.GetComponentInChildren <NewBanditSight> ();
		
		hud = GameObject.Find ("GameHUD");
		guiScript = hud.GetComponent <GUImainBehaviour> ();
		
		bow = transform.Find ("Bandit_Rigging_Finished/Hip/Spine1/Spine2/Chest/L_Collar/L_Shoulder/L_Elbow/L_Hand/Bow").gameObject;
		sword = transform.Find ("Bandit_Rigging_Finished/Hip/Spine1/Spine2/Chest/R_Collar/R_Shoulder/R_Elbow/R_Hand/Sword/SwordModel").gameObject;
		
		bowMesh = bow.GetComponent <MeshRenderer>();
		swordMesh = sword.GetComponent <MeshRenderer>();
	}
	
	void CalculateAimPosition (Vector3 targetPos)
	{
		Vector3 aimPoint = new Vector3(targetPos.x - transform.position.x, targetPos.y - transform.position.y, targetPos.z - transform.position.z);
		desiredRotation = Quaternion.LookRotation (aimPoint);
		desiredRotation.x = 0;
		desiredRotation.z = 0;
		
		Debug.DrawRay (transform.position, targetPos, Color.cyan);
	}
	
	void Update ()
	{
		if(!guiScript.paused)
		{
			animator.SetFloat ("Speed", setSpeed);
			
			setSpeed = 0;
		
			if (newEnGarde || (Vector3.SqrMagnitude (transform.position - player.transform.position) > 2.1f * 2.1f && CurrentMode == EnemyMode.Chasing)) 
			{
				newEnGardePosition = player.transform.position + (Random.insideUnitSphere * Random.Range (2, 2.1f));
				newEnGarde = false;
				Debug.DrawLine (transform.position, newEnGardePosition, Color.black, 5f); 
			}
		
			if (sightScript.isNear)
			{
				CurrentMode = EnemyMode.Chasing;
			}
			else if (sightScript.isFar)
			{
				CurrentMode = EnemyMode.BowAttack;
			}
		
			if (CurrentMode == EnemyMode.BowAttack)
			{
				bowMesh.enabled = true;
				swordMesh.enabled = false;
			
				navAgent.updatePosition = false;
				navAgent.updateRotation = false;
				
				//newEnGarde = true;
				
				if (sightScript.inSight)
				{
					CalculateAimPosition (player.transform.position);
					transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);
					
					if (Time.time >= nextFireTime)
					{
						FireProjectile ();
					}
				}
			}
			else if (CurrentMode == EnemyMode.Chasing)
			{
				
				bowMesh.enabled = false;
				swordMesh.enabled = true;
				
				Debug.Log ("Chasing");
				navAgent.updatePosition = true;
				navAgent.updateRotation = false;
				
				Quaternion selfRotation = Quaternion.LookRotation (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position);
				
				transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, Time.deltaTime * 15);
				
				newEnGarde = false;
				
				navAgent.SetDestination (newEnGardePosition);
				
				Hit ();
			}
		}
	}
	
	void FireProjectile()
	{
		nextFireTime = Time.time + reloadTime;
		
		GameObject spawnedArrow;
		spawnedArrow = Instantiate (myProjectile, transform.position + transform.up * 1.5f, transform.rotation) as GameObject;
		
		animator.SetTrigger ("Arrow");
		
		if (spawnedArrow.CompareTag ("HomingProjectile"))
		{
			Arrow_Homing projectileScript = spawnedArrow.GetComponent <Arrow_Homing>();
			projectileScript.masterTurret = this.gameObject;
			projectileScript.target = player;
		}
		else
		{
			Arrow_Normal projectileScript = spawnedArrow.GetComponent <Arrow_Normal>();
			projectileScript.masterTurret = this.gameObject;
		}
	}
	
	void Hit ()
	{	
		if (CurrentMode == EnemyMode.Chasing && Vector3.Distance (this.transform.position, player.transform.position) < 2 && canHit)
		{
			player.SendMessage ("GetHurt", 1 , SendMessageOptions.RequireReceiver);
			
			animator.SetTrigger ("Hit");
			
			StartCoroutine (HitCoolDown());
		}
		else if (CurrentMode == EnemyMode.Chasing && canHit) //If the enemy is not near enough to the player...
		{
			//navAgent.SetDestination (player.transform.position + (Random.insideUnitSphere * Random.Range (1,2)));
			
			setSpeed = Vector3.SqrMagnitude (navAgent.velocity.normalized);
		}
	}
	
	IEnumerator HitCoolDown ()
	{
		canHit = false;
		yield return new WaitForSeconds(1);
		canHit = true;
	}
	
	void gotHit (float damageAmount) //Managing all the damage the enemy is taking. Ouch.
	{
		Die ();
	}
	
	void Die () //DIE MOTHERFUCKER DIE
	{
		Destroy (this.gameObject);
		Ragdoll r = (Instantiate (ragdoll, transform.position, transform.rotation) as GameObject).GetComponent <Ragdoll> ();
		r.CopyPose (transform);
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if (hit.CompareTag ("EarthQuake")) //As this is in the OnTrigger Method, it'll be triggered only if the player is dashing in body mode.
		{
			gotHit (1);
			Debug.Log ("EARTHQUAKE");
		}
	}
	
	void OnCollisionEnter(Collision hit)
	{
		if (hit.collider.CompareTag ("ThrowableRock")) 
		{ //Rocks are rigidBody, so we have to check this in OnCollision Method.
			if(hit.rigidbody.velocity.sqrMagnitude > minRockVelocityToGetHurt * minRockVelocityToGetHurt)
				gotHit (2);		
		}
	}
	
}

	/*#region In Inspector Variables
	public float currentHealthPoint = 10f;
	public float recoveryTime = 3;
	#endregion
	
	#region Enemy's behaviour var
	[HideInInspector]
	public enum EnemyMode { BowAttack, Chasing }; //Define the current state of the enemy
	[HideInInspector]
	public EnemyMode CurrentMode = EnemyMode.BowAttack;
	
	[HideInInspector]
	public Vector3 newPosition;
	Vector3 newEnGardePosition;
	Vector3 moveDirection = Vector3.zero;
	
	[SerializeField]
	private float minRockVelocityToGetHurt = 5;
	
	[HideInInspector]
	public bool canGetHit = true;
	bool justJumped = false;
	bool canHit = true;
	#endregion
	
	#region Enemy's components
	[HideInInspector]
	public NavMeshAgent agent;
	[HideInInspector]
	public EnemySight sightScript;
	[HideInInspector]
	public CharacterController controller;
	[HideInInspector]
	public Transform sightSphere;
	[HideInInspector]
	public NavMeshAgent navMeshAgent;
	#endregion
	
	#region External scripts and objects
	[HideInInspector]
	public GameObject player;
	public GameObject soul;
	[HideInInspector]
	public bool isAnArcher = false;
	public bool archerChasing = false;
	#endregion
	
	public GameObject ragdoll;
	
	public Animator animator;
	
	private float setSpeed;
	bool newEnGarde = false;
	bool wasAfar = true;
	
	private bool holdFire = false;
	private Quaternion desiredRotation;
	
	public LayerMask sightObstructionLayers;
	public float turnSpeed = 5f;
	public float reloadTime = 1f;
	public GameObject myProjectile;
	public float errorAmount = .001f;
	
	[HideInInspector]
	public GameObject myTarget = null;
	[HideInInspector]
	public float nextFireTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		agent.updateRotation = false;
		
		animator.SetFloat ("Speed", setSpeed);
		
		setSpeed = 0;
		
		if (canGetHit) 
		{ //If the ennemy can't get hit, he can't move either.
			navMeshAgent.updatePosition = true;
			
			#region Changing target positions according to the random timer
			if (newEnGarde) 
			{
				newEnGardePosition = player.transform.position + (Random.insideUnitSphere * Random.Range (4, 7));
				newEnGarde = false;
			}
			#endregion
			
			if (CurrentMode == EnemyMode.BowAttack)
			{
				
			}
			
			#region setting target position according to current behaviour
			if (CurrentMode == EnemyMode.Chasing) //Movement when the player is in sight, or was in sight recently.
			{
				if (Vector3.Distance (this.transform.position, player.transform.position) > 5)
					agent.SetDestination (sightScript.lastKnownPosition);
				else if (!canHit && wasAfar)
				{
					newEnGarde = true;
				}
				else if (!canHit)
				{
					agent.SetDestination (newEnGardePosition);
				}
				
				if (Vector3.Distance (this.transform.position, player.transform.position) > 5)
				{
					wasAfar = true;
				}
				else
				{
					wasAfar = false;
				}
				
				setSpeed = Vector3.SqrMagnitude (agent.velocity.normalized);	
				
				Quaternion selfRotation = Quaternion.LookRotation (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position);
				
				transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, Time.deltaTime * 15);
			}
			#endregion
			
		} else
			navMeshAgent.updatePosition = false;
		
		if (currentHealthPoint <= 0) 
			Die();
		
		//Those last lines controls some of the enemy's movements
		
		if(!navMeshAgent.enabled)
			controller.Move(moveDirection * Time.deltaTime);
		
		Hit ();
	}
	
	void Hit ()
	{	
		if (CurrentMode == EnemyMode.Chasing && Vector3.Distance (this.transform.position, player.transform.position) < 2 && canHit)
		{
			player.SendMessage ("GetHurt", 1 , SendMessageOptions.RequireReceiver);
			
			animator.SetTrigger ("Hit");
			
			StartCoroutine (HitCoolDown());
		}
		else if (CurrentMode == EnemyMode.Chasing && canHit) //If the enemy is not near enough to the player...
		{
			agent.SetDestination (player.transform.position + (Random.insideUnitSphere * Random.Range (1,2)));
			
			setSpeed = Vector3.SqrMagnitude (agent.velocity.normalized);
		}
	}
	
	IEnumerator HitCoolDown ()
	{
		canHit = false;
		yield return new WaitForSeconds(3);
		canHit = true;
	}
	
	void OnTriggerEnter(Collider hit)
	{
		if (hit.CompareTag ("EarthQuake") && canGetHit) //As this is in the OnTrigger Method, it'll be triggered only if the player is dashing in body mode.
		{
			gotHit (1);
			Debug.Log ("EARTHQUAKE");
		}
	}
	
	void OnCollisionEnter(Collision hit)
	{
		if (hit.collider.CompareTag ("ThrowableRock")) 
		{ //Rocks are rigidBody, so we have to check this in OnCollision Method.
			if(hit.rigidbody.velocity.sqrMagnitude > minRockVelocityToGetHurt * minRockVelocityToGetHurt)
				gotHit (2);		
		}
	}
	
	void gotHit (float damageAmount) //Managing all the damage the enemy is taking. Ouch.
	{
		if (canGetHit) 
		{
			currentHealthPoint -= damageAmount;
			
			StartCoroutine (temporaryIntangible ());
		}
	}
	
	void Die () //DIE MOTHERFUCKER DIE
	{
		Destroy (this.gameObject);
		Ragdoll r = (Instantiate (ragdoll, transform.position, transform.rotation) as GameObject).GetComponent <Ragdoll> ();
		r.CopyPose (transform);
	}
	
	IEnumerator temporaryIntangible() //Right after being hit, the enemy is stunned and Intangible. Consider this as a small recovery time for the enemy.
	{
		gameObject.layer = 17;
		canGetHit = false; //This prevents the ennemy to get hurt twice with a single attack and deactivate all of its NavMesh agent's behaviours.
		//Jump (true);
		
		yield return new WaitForSeconds (recoveryTime);
		
		canGetHit = true;
		gameObject.layer = 10;
	}
	
	void FireProjectile()
	{
		nextFireTime = Time.time + reloadTime;
		
		GameObject spawnedArrow;
		spawnedArrow = Instantiate (myProjectile, transform.position, transform.rotation) as GameObject;
		
		animator.SetTrigger ("Arrow");
		
		if (spawnedArrow.CompareTag ("HomingProjectile"))
		{
			Arrow_Homing projectileScript = spawnedArrow.GetComponent <Arrow_Homing>();
			projectileScript.masterTurret = this.gameObject;
			projectileScript.target = myTarget;
		}
		else
		{
			Arrow_Normal projectileScript = spawnedArrow.GetComponent <Arrow_Normal>();
			projectileScript.masterTurret = this.gameObject;
		}
	}
}*/
