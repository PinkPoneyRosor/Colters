using UnityEngine;
using System.Collections;

public class Projectile_Global : MonoBehaviour {

	public float mySpeed = 10;
	public float myRange = 10;
	public float damage = 1;
	public GameObject player;
	public Transform target;
	
	[HideInInspector]
	public bool hitSomething = false;
	[HideInInspector]
	public bool hitPlayer = false;
	
	[HideInInspector]
	public float myDist;
	
	[HideInInspector]
	public GameObject masterTurret;
	
	[HideInInspector]
	public Transform turretMuzzle;
	
	void Start ()
	{
		Physics.IgnoreCollision(transform.GetComponent<Collider>(), masterTurret.GetComponent<Collider>());
	}
	
	public void OnTriggerEnter (Collider hit)
	{
		if (hit.CompareTag ("Player"))
		{
			hit.SendMessage("GetHurt", damage, SendMessageOptions.DontRequireReceiver);
			hitPlayer = true;
		}
		
		hitSomething = true;
		transform.SetParent (hit.transform, true);
	}
	
}
