using UnityEngine;
using System.Collections;

public class Projectile_Global : MonoBehaviour 
{

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
	}
	
	public void OnTriggerEnter (Collider hit)
	{	
		if (hit.CompareTag ("Player"))
		{
			hit.SendMessage("GetHurt", damage, SendMessageOptions.DontRequireReceiver);
			hitPlayer = true;
		}
		
		if(hit.gameObject != masterTurret || hit.transform != turretMuzzle)
		{
			hitSomething = true;
			Debug.Log ("Hello, I'm " + this.name + ", and I've just hit " + hit.name + " and it SURELY WASN'T THE BOW THAT LAUNCHED ME");
		}
		else
		{
			hitSomething = false;
		}
			
		transform.SetParent (hit.transform, true);
	}
}
