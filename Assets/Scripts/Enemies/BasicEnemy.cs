using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour {
#region variables

	#region In Inspector Variables
	public float currentHealthPoint = 10f;
	public float heightOfJump = 8;
	public float gravity = 20;
	public float recoveryTime = 3;
	#endregion

	#region Enemy's behaviour var
	[HideInInspector]
	public enum EnemyMode { Wandering, Chasing }; //Define the current state of the enemy
	[HideInInspector]
	public EnemyMode CurrentMode = EnemyMode.Wandering;

	[HideInInspector]
	public Vector3 newPosition;
	Vector3 newEnGardePosition;
	Vector3 moveDirection = Vector3.zero;
	
	[SerializeField]
	private float minRockVelocityToGetHurt = 5;
	
	float randomizeTimer = 0f;
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
	
#endregion

	// Use this for initialization
	void Start () 
	{
	}

	// Update is called once per frame
	void Update ()
	{
		agent.updateRotation = false;
		
				
		if (canGetHit) 
		{ //If the ennemy can't get hit, he can't move either.
			if (!navMeshAgent.enabled) 
			{ //Since the enemy can get hit, he can also moves, so we make sure the navAgent is activated.
					navMeshAgent.enabled = true;
			}

			#region Changing target positions according to the random timer
			randomizeTimer += 1 * Time.deltaTime; //Let's increment our random Timer, it'll be usefull to make the enemy's randomly wander in some of its behaviour.

			if (randomizeTimer > 3) 
			{
					randomizeTimer = 0;
					newPosition = transform.position + (Random.insideUnitSphere * Random.Range (5, 10));
			}

			if (randomizeTimer > 1) 
			{
					newEnGardePosition = player.transform.position + (Random.insideUnitSphere * Random.Range (4, 7));
			}
			#endregion

			#region Is the player in sight?
			if (sightScript.playerRecentlySeen) //If the player's recently been in sight, let's chase him ! Otherwise, let's just continue to wander around.
			{
					CurrentMode = EnemyMode.Chasing;		
			}
			else
			{
					CurrentMode = EnemyMode.Wandering;
			}
			#endregion

			#region setting target position according to current behaviour
			if (CurrentMode == EnemyMode.Chasing) //Movement when the player is in sight, or was in sight recently.
			{
				if (Vector3.Distance (this.transform.position, player.transform.position) > 5)
						agent.SetDestination (sightScript.lastKnownPosition);
				else if (!canHit)
						agent.SetDestination (newEnGardePosition);
						
				Quaternion selfRotation = Quaternion.LookRotation (new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position);
				
				transform.rotation = Quaternion.Slerp (transform.rotation, selfRotation, Time.deltaTime * 15);
			}

			if (CurrentMode == EnemyMode.Wandering && !isAnArcher) //Movement when the player's not in sight.
			{
				agent.SetDestination (new Vector3 (newPosition.x, transform.position.y, newPosition.z));
			}
			#endregion

		} else
			navMeshAgent.enabled = false;

		if (currentHealthPoint <= 0) 
			Die();

		//Those last lines controls some of the enemy's movements
		if (!controller.isGrounded)
			moveDirection.y -= gravity * Time.deltaTime;

		if(!navMeshAgent.enabled)
			controller.Move(moveDirection * Time.deltaTime);
		
			Hit ();
	}

	void LateUpdate()
	{
		//If we just landed...
		//We have to check if the enemy's landed AFTER the update function, otherwise some variables won't get properly synced;
		if (controller.isGrounded && justJumped) 
		{
			moveDirection = Vector3.zero;
			justJumped = false;
		}
	}
	
	void Hit ()
	{	
		if (CurrentMode == EnemyMode.Chasing && Vector3.Distance (this.transform.position, player.transform.position) < 2 && canHit)
		{
			player.SendMessage ("GetHurt", 1 , SendMessageOptions.RequireReceiver);
			StartCoroutine (HitCoolDown());
		}
		else if (CurrentMode == EnemyMode.Chasing && canHit) //If the enemy is not near enough to the player...
		{
			agent.SetDestination (player.transform.position + (Random.insideUnitSphere * Random.Range (1,2)));
		}
	}
	
	IEnumerator HitCoolDown ()
	{
		canHit = false;
		yield return new WaitForSeconds(3);
		canHit = true;
	}

	void Jump (bool isPushed) //Jump func. 'Nuff said.
	{
		moveDirection.y = this.heightOfJump;

		if(isPushed)
				moveDirection += player.transform.right * 2;

		justJumped = true;
	}

	void OnTriggerEnter(Collider hit)
	{
		if (hit.CompareTag ("PlayerSoul") && canGetHit) //As this is in the OnTrigger Method, it'll be triggered only if the player is dashing in body mode.
		{
			Debug.Log ("Enemy hit player");
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
			renderer.material.color = Color.white;

			StartCoroutine (temporaryIntangible ());
		}
	}

	void Die () //DIE MOTHERFUCKER DIE
	{
		Destroy (this.gameObject);
	}

	IEnumerator temporaryIntangible() //Right after being hit, the enemy is stunned and Intangible. Consider this as a small recovery time for the enemy.
	{
		gameObject.layer = 17;
		canGetHit = false; //This prevents the ennemy to get hurt twice with a single attack and deactivate all of its NavMesh agent's behaviours.
		Jump (true);

		yield return new WaitForSeconds (recoveryTime);

		canGetHit = true;
		gameObject.layer = 10;
	}
}