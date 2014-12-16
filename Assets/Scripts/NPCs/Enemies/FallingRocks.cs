using UnityEngine;
using System.Collections;

public class FallingRocks : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		
	}
	
	// Update is called once per frame
	void Update () {}
	
	
	
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "ThrowableRock") {
			rigidbody.constraints = RigidbodyConstraints.None;
			
		}
	}
}

