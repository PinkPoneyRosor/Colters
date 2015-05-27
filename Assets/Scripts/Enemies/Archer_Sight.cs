using UnityEngine;
using System.Collections;

public class Archer_Sight : EnemySight {

	Enemy_Archer archerScript;
	GameObject player;
	
	bool playerNear = false;
	bool inSight = false;

	// Use this for initialization
	void Start () 
	{
		archerScript = this.GetComponentInParent <Enemy_Archer>();
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Vector3.SqrMagnitude (this.transform.position - player.transform.position) < 5 * 5 && inSight)
		{
			playerNear = true;
			Debug.Log ("Yup");
		}
		else if (inSight)
		{
			playerNear = false;
			archerScript.nextFireTime = Time.time + (archerScript.reloadTime);
			archerScript.myTarget = player.gameObject;
			Debug.Log ("Nope");
		}
	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			inSight = true;
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		inSight = false;
	}
}
