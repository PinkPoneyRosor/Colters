using UnityEngine;
using System.Collections;

public class Archer_Sight : EnemySight {

	Enemy_Archer archerScript;
	GameObject player;
	
	bool playerNear = false;
	bool inSight = false;
	bool justGotInSight = true;

	// Use this for initialization
	void Start () 
	{
		archerScript = this.GetComponentInParent <Enemy_Archer>();
		player = GameObject.Find ("Player");
		isInArcherMode = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Vector3.SqrMagnitude (this.transform.position - player.transform.position) < 5 * 5 && inSight)
		{
			playerNear = true;
			isInArcherMode = false; 
			Debug.Log ("Yup");
			justGotInSight = true;
			
			if (!playerInSight && playerRecentlySeen) 
				launchTimer = true;
			
			if (launchTimer)
				Timer ();
			
			if (trackTimer >= timeBeforeLosingTracks)
			{
				ResetTimer ();
				playerRecentlySeen = false;
			}
		}
		else if (inSight)
		{
			isInArcherMode = true;
			playerNear = false;
			
			if (justGotInSight)
			{
				archerScript.nextFireTime = Time.time + (archerScript.reloadTime);
				justGotInSight = false;
			}
			
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
	}
}
