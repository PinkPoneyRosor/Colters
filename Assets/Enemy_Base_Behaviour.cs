using UnityEngine;
using System.Collections;

public class Enemy_Base_Behaviour : BasicEnemy {

	// Use this for initialization
	void Start () {
		sightSphere = transform.Find ("Sight") as Transform;
		sightScript = sightSphere.GetComponent<EnemySight>();
		controller = GetComponent <CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Basic controller = " + controller.gameObject.name);
	}
}
