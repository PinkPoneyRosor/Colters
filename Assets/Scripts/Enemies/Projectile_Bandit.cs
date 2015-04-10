using UnityEngine;
using System.Collections;

public class Projectile_Bandit : Projectile_Global {

	void Start ()
	{
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate (Vector3.forward * Time.deltaTime * mySpeed);
		myDist += Time.deltaTime * mySpeed;

		if(myDist>= myRange)
			Destroy(gameObject);
	}
	
}
