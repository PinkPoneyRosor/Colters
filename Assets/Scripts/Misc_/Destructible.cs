using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

	// Use this for initialization
	void Start () {
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
	
	// Update is called once per frame
	void Update () {}


	
	void	OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "ThrowableRock") {
			Debug.Log("Destroy!");
			rigidbody.constraints = RigidbodyConstraints.None;
		}
	}




}