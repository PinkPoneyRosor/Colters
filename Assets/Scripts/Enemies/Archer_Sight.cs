using UnityEngine;
using System.Collections;

public class Archer_Sight : EnemySight {

	Enemy_Archer archerScript;
	Archer_Base_Behaviour archerBaseScript;
	
	bool playerNear = false;
	bool inSight = false;
	bool justGotInSight = true;

	// Use this for initialization
	void Start () 
	{
		archerScript = this.GetComponentInParent <Enemy_Archer>();
		player = GameObject.Find ("Player");
		isInArcherMode = true;
		archerBaseScript = this.GetComponentInParent <Archer_Base_Behaviour>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Vector3.SqrMagnitude (this.transform.position - player.transform.position) < 3 * 3 && inSight)
		{
			playerNear = true;
			isInArcherMode = false; 
			Debug.Log ("Yup");
			justGotInSight = true;
			
			if (!playerInSight && playerRecentlySeen) 
				launchTimer = true;
			
			if (launchTimer)
				Timer ();
			
			/*if (trackTimer >= timeBeforeLosingTracks)
			{
				ResetTimer ();
				playerRecentlySeen = false;
			}*/
		}
		else if (inSight && Vector3.SqrMagnitude (this.transform.position - player.transform.position) > 3 * 3)
		{
			isInArcherMode = true;
			playerNear = false;
			
			if (justGotInSight)
			{
				archerScript.nextFireTime = Time.time + (archerScript.reloadTime);
				justGotInSight = false;
				archerBaseScript.archerChasing = true;
			}
			
			playerRecentlySeen = false;
			
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
		justGotInSight = true;
		isInArcherMode = false;
		archerBaseScript.archerChasing = false;
	}
}
