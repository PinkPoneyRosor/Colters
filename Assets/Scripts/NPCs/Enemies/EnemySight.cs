using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour 
{
	//This script is heavily inspired of the unity "Stealth" tutorial.

	public float fieldOfViewAngle = 110f;           // Number of degrees, centred on forward, for the enemy see.
	private bool playerInSight = false;                      // Whether or not the player is currently sighted.
	public bool playerRecentlySeen = false;
	public Vector3 lastKnownPosition = Vector3.zero;
	

	private SphereCollider sightCollider;
	private GameObject player;                      // Reference to the player.

	public LayerMask layer;

	private float trackTimer = 0;
	[SerializeField]
	private float timeBeforeLosingTracks = 5;
	private bool launchTimer = false;

	void Update()
	{
	if (!playerInSight && playerRecentlySeen) 
			launchTimer = true;
		
	if (launchTimer)
			Timer ();

	if (trackTimer >= timeBeforeLosingTracks)
		{
			ResetTimer ();
			playerRecentlySeen = false;
			Debug.Log("Lost the player");
		}
			
	}

	void Timer ()
	{
		trackTimer += Time.deltaTime;
	}

	void ResetTimer()
	{
		launchTimer = false;
		trackTimer = 0;
	}

	void Awake ()
	{
		sightCollider = GetComponent<SphereCollider>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void OnTriggerStay (Collider other)
	{
		// If the player has entered the trigger sphere...
		if(other.gameObject == player)
		{
			// By default the player is not in sight.
			playerInSight = false;
			
			// Create a vector from the enemy to the player and store the angle between it and forward.
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			// If the angle between forward and where the player is, is less than half the angle of view...
			if(angle < fieldOfViewAngle * 0.5f)
			{
				RaycastHit hit;
				
				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(transform.position + transform.up*.5f, direction.normalized, out hit, sightCollider.radius))
				{
					Debug.DrawRay(transform.position + transform.up*.5f, direction.normalized* sightCollider.radius);
					// ... and if the raycast hits the player...
					if(hit.collider.gameObject == player)
					{
						// ... the player is in sight.
						ResetTimer(); //Just to make sure the timer is inactive as the player is currently in sight.
						playerInSight = true;
						playerRecentlySeen = true;
						lastKnownPosition = player.transform.position;
					}
				}
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		// If the player leaves the trigger zone...
		if(other.gameObject == player)
			playerInSight = false; // ... the player is not in sight.
	}
}
