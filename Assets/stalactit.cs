using UnityEngine;
using System.Collections;

public class stalactit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision hit)
	{
		if(hit.collider.CompareTag("ThrowableRock"))
		{
			animation.Play("RiverBridgeAnim");
			animation["RiverBridgeAnim"].speed = 1;
		}
	}
}
