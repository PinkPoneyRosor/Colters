using UnityEngine;
using System.Collections;

public class Enemy_Base_Behaviour : BasicEnemy {

	// Use this for initialization
	void Start () 
	{
		sightSphere = transform.Find ("Sight") as Transform;
		sightScript = sightSphere.GetComponent <EnemySight>();
		controller = GetComponent <CharacterController>();
		navMeshAgent = GetComponent <NavMeshAgent>();
		agent = GetComponent <NavMeshAgent>();
		player = GameObject.Find ("Player");
		newPosition = transform.position + (Random.insideUnitSphere * Random.Range (5,10));
		isAnArcher = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
