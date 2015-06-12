using UnityEngine;
using System.Collections;

public class PlankCollisionDetect : MonoBehaviour 
{
	
	public bool mustBeExplosive;
	
	// Use this for initialization
	void Start () {
		transform.parent.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
	
	// Update is called once per frame
	void Update () {}
	
	
	
	void	OnTriggerEnter (Collider col){

		Debug.Log ("Triggered");
		if (col.gameObject.tag == "ThrowableRock" || col.gameObject.tag == "ThrowableRock2") 
		{
			transform.parent.rigidbody.constraints = RigidbodyConstraints.None;
		}
	}
}