using UnityEngine;
using System.Collections;

public class BodyControls : PlayerCommon {

	CharacterController controller;

	[SerializeField]
	private float speed;
	[SerializeField]
	private float heightOfJump;
	[SerializeField]
	static float gravity;


	// Use this for initialization
	void Start () 
	{
		controller = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		DefaultMoves (this.gameObject, controller, speed, heightOfJump, gravity);
	}
}
