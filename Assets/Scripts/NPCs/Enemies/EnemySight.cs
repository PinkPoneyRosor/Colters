using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

	public float fieldOfViewAngle = 110f;           // Number of degrees, centred on forward, for the enemy see.
	public bool playerInSight;                      // Whether or not the player is currently sighted.
	

	private SphereCollider collider;
	private GameObject player;                      // Reference to the player.

	public LayerMask layer;
	
	
	void Awake ()
	{
		collider = GetComponent<SphereCollider>();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	
	void Update ()
	{
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
			float angle = Vector3.Angle(direction, transform.forward); //This will be used later in order to implement the FOV

			RaycastHit hit;
				
			// ... and if a raycast towards the player hits something...
			if(Physics.Raycast(transform.position, direction.normalized, out hit, collider.radius, layer))
			{
				// ... and if the raycast hits the player...
				if(hit.collider.gameObject == player)
				{
					// ... the player is in sight.
					playerInSight = true;
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
