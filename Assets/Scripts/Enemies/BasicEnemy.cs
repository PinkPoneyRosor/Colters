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

	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player");
		agent = GetComponent<NavMeshAgent>();
		sightSphere = transform.Find("Sight") as Transform;
		sightScript = sightSphere.GetComponent<EnemySight>();
		newPosition = transform.position + (Random.insideUnitSphere * Random.Range (5,10));
	}
	
	// Update is called once per frame
	void Update () {

		randomizeTimer+= 1 * Time.deltaTime;

		if(randomizeTimer>3)
		{
			randomizeTimer=0;
			newPosition = transform.position + (Random.insideUnitSphere * Random.Range (5,10));

		}

		if(randomizeTimer>1){
			newEnGardePosition = player.transform.position + (Random.insideUnitSphere * Random.Range (5,8));
		}

		if(sightScript.playerInSight){
			CurrentMode = EnemyMode.Chasing;}
				else if (!sightScript.playerInSight){
					CurrentMode = EnemyMode.Wandering;
		}

		if (currentHealthPoint<=0){
			Destroy (this.gameObject);
		}

		if(CurrentMode == EnemyMode.Chasing){
			if (Vector3.Distance(this.transform.position, player.transform.position) > 5)
				agent.SetDestination(player.transform.position);
			else
				agent.SetDestination(newEnGardePosition);
		}else if(CurrentMode == EnemyMode.Wandering){
			agent.SetDestination(new Vector3(newPosition.x,transform.position.y,newPosition.z));
		}
	}

	void OnCollisionEnter(Collision hit)
	{
		if (hit.collider.CompareTag ("Player"))
						gotHit (1);
	}

	void gotHit (float damageAmount){
		currentHealthPoint -= damageAmount;
		renderer.material.color = Color.white;
		StartCoroutine(temporaryIntangible ());
}

	IEnumerator temporaryIntangible()
	{
		collider.isTrigger = true;
		yield return new WaitForSeconds(1);
		collider.isTrigger = false;
	}
}
