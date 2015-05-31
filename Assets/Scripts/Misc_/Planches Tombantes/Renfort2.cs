using UnityEngine;
using System.Collections;

public class Renfort2 : MonoBehaviour {

	private Planche planche;
	
	// Use this for initialization
	void Start () {
		planche = GetComponent<Planche>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "ThrowableRock2")
		{
			Destroy(gameObject);
			planche.aliveToo = false;
		}
	}
}
