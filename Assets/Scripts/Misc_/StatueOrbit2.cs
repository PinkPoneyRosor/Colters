using UnityEngine;
using System.Collections;

public class StatueOrbit2: MonoBehaviour {




	// Use this for initialization
	void Start () {
		
		renderer.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void	OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "ThrowableRock2") {
			Debug.Log("My eye 2 !");
			renderer.enabled = true ;
		}
	}
}
