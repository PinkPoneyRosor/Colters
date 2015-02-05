using UnityEngine;
using System.Collections;

public class StatueOrbit1: MonoBehaviour {

	// Use this for initialization
	void Start () {

		renderer.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		}

	void	OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "ThrowableRock") {
			Debug.Log("My eye !");
			renderer.enabled = true ;
		}
	}
}
