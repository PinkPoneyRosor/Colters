using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

	public bool mustBeExplosive;

	// Use this for initialization
	void Start () {
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
	
	// Update is called once per frame
	void Update () {}


	
	void	OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "ThrowableRock" || col.gameObject.tag == "ThrowableRock2") 
		{
			rigidbody.constraints = RigidbodyConstraints.None;
		}
	}
}