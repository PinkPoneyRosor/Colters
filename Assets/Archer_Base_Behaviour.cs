using UnityEngine;
using System.Collections;

public class Archer_Base_Behaviour : BasicEnemy {

	// Use this for initialization
	void Start () {
		sightSphere = transform.Find ("Sight") as Transform;
		sightScript = sightSphere.GetComponent<Archer_Sight>();
		controller = GetComponent <CharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log ("Archer controller = " + controller.gameObject.name);
	}
}
