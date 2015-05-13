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

	Vector3 newPosition;
	Vector3 newEnGardePosition;
	Vector3 moveDirection = Vector3.zero;
	
	[SerializeField]
	private float minRockVelocityToGetHurt = 5;
	
	float randomizeTimer = 0f;
	[HideInInspector]
	public bool canGetHit = true;
	bool justJumped = false;
	#endregion

	#region Enemy's components
	NavMeshAgent agent;
	EnemySight sightScript;
	CharacterController controller;
	Transform sightSphere;
	NavMeshAgent navMeshAgent;
	#endregion

	#region External scripts and objects
	GameObject player;
	public GameObject soul;
	#endregion
	
#endregion

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		sightSphere = transform.Find ("Sight") as Transform;

		agent = GetComponent<NavMeshAgent>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		controller = GetComponent<CharacterController>();
		sightScript = sightSphere.GetComponent<EnemySight>();

		newPosition = transform.position + (Random.insideUnitSphere * Random.Range (5,10));
	}

	// Update is called once per frame
	void Update ()
	{
		if (canGetHit) 
		{ //If the ennemy can't get hit, he can't move either.
			Debug.Log ("I can get hit");
			if (!navMeshAgent.enabled) { //Since the enemy can get hit, he can also moves, so we make sure the navAgent is activated.
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
					newEnGardePosition = player.transform.position + (Random.insideUnitSphere * Random.Range (5, 8));
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
			if (CurrentMode == EnemyMode.Chasing) //Movement when the player is in sight.
			{
				if (Vector3.Distance (this.transform.position, player.transform.position) > 5)
						agent.SetDestination (sightScript.lastKnownPosition);
				else
						agent.SetDestination (newEnGardePosition);

				this.transform.LookAt (player.transform);
			}

			if (CurrentMode == EnemyMode.Wandering) //Movement when the player's not in sight.
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

	void OnParticleCollision(GameObject other)
	{
		if (other.CompareTag ("EarthQuake"))
			gotHit (1);
		}

	IEnumerator temporaryIntangible() //Right after being hit, the enemy is stunned and Intangible. Consider this as a small recovery time for the enemy.
	{
		gameObject.layer = 17;
		canGetHit = false; //This prevents the ennemy to get hurt twice with a single attack and deactivate all of its NavMesh agent's behaviours.
		Jump (true);

		yield return new WaitForSeconds(recoveryTime);

		canGetHit = true;
		gameObject.layer = 10;
	}
}