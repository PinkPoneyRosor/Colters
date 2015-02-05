using UnityEngine;
using System.Collections;

public class FallingRocks : MonoBehaviour {

	//public float speed = 5;
	// Use this for initialization
	void Start () {

		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		
	}
	
	// Update is called once per frame
	void Update () {


	}
	
	
	
	
	/*void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "ThrowableRock") {
			transform.Translate(Vector3.down * speed * Time.deltaTime);
	

	

			
		}
	}*/
}

