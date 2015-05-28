using UnityEngine;
using System.Collections;

public class Archer_Base_Behaviour : BasicEnemy {

	// Use this for initialization
	void Start () 
	{
		sightSphere = transform.Find ("Sight") as Transform;
		sightScript = sightSphere.GetComponent<Archer_Sight>();
		controller = GetComponent <CharacterController>();
		navMeshAgent = GetComponent <NavMeshAgent>();
		agent = GetComponent <NavMeshAgent>();
		player = GameObject.Find ("Player");
		newPosition = transform.position + (Random.insideUnitSphere * Random.Range (5,10));
		isAnArcher = true;
	}
	
	// Update is called once per frame
}
