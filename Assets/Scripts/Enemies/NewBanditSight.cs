using UnityEngine;
using System.Collections;

public class NewBanditSight : MonoBehaviour {

	[HideInInspector]
	public bool inSight = false;
	
	public float fieldOfViewAngle = 110f;
	public GameObject player;
	public SphereCollider sightCollider;
	public LayerMask selectLayers;
	
	public bool isNear = false;
	public bool isFar = false;
	
	NewBanditAI banditScript;
	
	void Start ()
	{
		banditScript = this.GetComponentInParent <NewBanditAI>();
	}
			
	void OnTriggerStay (Collider other)
	{
			// If the player has entered the trigger sphere...
			if(other.CompareTag("Player"))
			{
				// By default the player is not in sight.
				//inSight = false;
				
				// Create a vector from the enemy to the player and store the angle between it and forward.
				Vector3 direction = other.transform.position - other.transform.up * .5f - transform.position;
				float angle = Vector3.Angle(direction, transform.forward);
				
				// If the angle between forward and where the player is, is less than half the angle of view...
					RaycastHit hit;
					
				//if(angle < fieldOfViewAngle * 0.5f || Vector3.Distance (this.transform.position, player.transform.position) < 5)
				//{
					// ... and if a raycast towards the player hits something...
					if(Physics.Raycast(transform.position + transform.up * 2, direction.normalized, out hit, sightCollider.radius, selectLayers))
					{
						Debug.DrawRay(transform.position + transform.up * 2, direction.normalized * sightCollider.radius, Color.red);
						// ... and if the raycast hits the player...
						
						if(hit.collider.CompareTag ("Player"))
						{
							// ... the player is in sight.
							inSight = true;
						}
						else
						{
							inSight = false;
						}
					}
				//}
				if (inSight && Vector3.SqrMagnitude (player.transform.position - transform.position) < 10 * 10)
				{
					if (isFar)
					{
						banditScript.newEnGarde = true;
					}
				
					isNear = true;
					isFar = false;
				}
				else if (inSight && Vector3.SqrMagnitude (player.transform.position - transform.position) > 10 * 10)
				{
					isNear = false;
					isFar = true;
				}
			}
	}
	
	void OnTriggerExit (Collider other)
	{
		if (other.CompareTag("Player"))
		{
			inSight = false;
		}
	}

	/*NewBanditAI archerScript;
	
	bool playerNear = false;
	bool inSight = false;
	bool justGotInSight = true;
	
	public float fieldOfViewAngle = 110f;           // Number of degrees, centred on forward, for the enemy see.
	[HideInInspector]
	public bool playerInSight = false;                      // Whether or not the player is currently in sight.
	public bool playerRecentlySeen = false;
	public Vector3 lastKnownPosition = Vector3.zero;
	
	
	private SphereCollider sightCollider;
	[HideInInspector]
	public GameObject player;                      // Reference to the player.
	
	public LayerMask layer;
	
	[HideInInspector]
	public float trackTimer = 0;
	public float timeBeforeLosingTracks = 5;
	[HideInInspector]
	public bool launchTimer = false;
	
	[HideInInspector]
	public bool isInArcherMode = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Vector3.SqrMagnitude (this.transform.position - player.transform.position) < 3 * 3 && inSight)
		{	
			archerScript.CurrentMode = NewBanditAI.EnemyMode.Chasing;
			
			if (!playerInSight && playerRecentlySeen) 
				launchTimer = true;
			
			if (launchTimer)
				Timer ();
		}
		else if (inSight && Vector3.SqrMagnitude (this.transform.position - player.transform.position) > 3 * 3)
		{
			archerScript.CurrentMode = NewBanditAI.EnemyMode.BowAttack;
			
			if (justGotInSight)
			{
				archerScript.nextFireTime = Time.time + (archerScript.reloadTime);
				justGotInSight = false;
			}
			
			playerRecentlySeen = false;
			
			archerScript.myTarget = player.gameObject;
		}
	}
	
	public void Timer ()
	{
		trackTimer += Time.deltaTime;
	}*/
	
}
