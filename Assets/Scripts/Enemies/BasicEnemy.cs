using UnityEngine;
using System.Collections;
public class BasicEnemy : MonoBehaviour {
	public float currentHealthPoint = 10f;
	NavMeshAgent agent;
	GameObject player;
	[HideInInspector]
	public enum EnemyMode { Wandering, Chasing, EnGarde }; //Définit le mode actuel dans lequel se trouve l'ennemi
	//[HideInInspector]
	public EnemyMode CurrentMode = EnemyMode.Wandering;
	EnemySight sightScript;
	Transform sightSphere;
	Vector3 newPosition;
	Vector3 newEnGardePosition;
	float randomizeTimer = 0f;
	NavMeshAgent navMeshAgent;
	bool canGetHit = true;
	bool Jump = false;
	CharacterController controller;
	public float heightOfJump = 8;
	public float gravity = 20;
	Vector3 moveDirection = Vector3.zero;
	bool activateNavMesh = false;
	bool pushed = false;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		agent = GetComponent<NavMeshAgent>();
		sightSphere = transform.Find("Sight") as Transform;
		sightScript = sightSphere.GetComponent<EnemySight>();
		newPosition = transform.position + (Random.insideUnitSphere * Random.Range (5,10));
		navMeshAgent = this.GetComponent<NavMeshAgent>();
		controller = GetComponent<CharacterController>();
	}
	// Update is called once per frame
	void Update ()
	{
		if (canGetHit) { //If the ennemy can't get hit, he can't move either.
			randomizeTimer += 1 * Time.deltaTime;
			if (randomizeTimer > 3) {
				randomizeTimer = 0;
				newPosition = transform.position + (Random.insideUnitSphere * Random.Range (5, 10));
			}
			if (randomizeTimer > 1) {
				newEnGardePosition = player.transform.position + (Random.insideUnitSphere * Random.Range (5, 8));
			}
			if (sightScript.playerInSight) {
				CurrentMode = EnemyMode.Chasing;
			} else if (!sightScript.playerInSight) {
				CurrentMode = EnemyMode.Wandering;
			}
			if (currentHealthPoint <= 0) {
				Destroy (this.gameObject);
			}
			if (CurrentMode == EnemyMode.Chasing) {
				if (Vector3.Distance (this.transform.position, player.transform.position) > 5)
					agent.SetDestination (player.transform.position);
				else
					agent.SetDestination (newEnGardePosition);
			} else if (CurrentMode == EnemyMode.Wandering) {
				agent.SetDestination (new Vector3 (newPosition.x, transform.position.y, newPosition.z));
			}
		}
		if (activateNavMesh && controller.isGrounded) {
			navMeshAgent.enabled = true;
			activateNavMesh = false;
		}
		if (pushed && !controller.isGrounded) {
			moveDirection = player.transform.right * 2;
		} else if (pushed && controller.isGrounded) {
			pushed = false;
			moveDirection = new Vector3 (0, this.transform.position.y,0);
		}
		if (Jump && controller.isGrounded)
		{
			moveDirection.y = this.heightOfJump;
			Jump = false;
			Debug.Log ("Jumpin'");
		}
		if(!controller.isGrounded)
			moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
	void OnTriggerEnter(Collider hit)
	{
		if (hit.CompareTag ("Player") && canGetHit)
		{
			gotHit (1);
		}
	}
	void gotHit (float damageAmount)
	{
		Jump = true;
		currentHealthPoint -= damageAmount;
		renderer.material.color = Color.white;
		StartCoroutine(temporaryIntangible ());
	}
	IEnumerator temporaryIntangible()
	{
		Physics.IgnoreCollision(this.collider, player.collider, true);
		canGetHit = false; //This prevents the ennemy to get hurt twice with a single attack.
		navMeshAgent.enabled = false;
		pushed = true;
		yield return new WaitForSeconds(3);
		Physics.IgnoreCollision(this.collider, player.collider, false);
		canGetHit = true;
		activateNavMesh = true;
	}
}