using UnityEngine;
using System.Collections;

public class DestructiblePart : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "ThrowableRock") {
			
			rigidbody.constraints = RigidbodyConstraints.None;
	

	

			
		}
	}

}
