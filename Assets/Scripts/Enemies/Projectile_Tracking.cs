using UnityEngine;
using System.Collections;

public class Projectile_Tracking : MonoBehaviour {
	
	public float turnSpeed = 50f;
	private float DistanceBeforeDestruction;
	[HideInInspector]
	public Transform rotationNeeded;
	[HideInInspector]
	public Rigidbody rigidbody;
	public GameObject player;
	public float mySpeed = 10;
	public Transform target;

	private Quaternion desiredRotation = Quaternion.identity;
	private Turret_Bandito_Tracking turret_Bandito_Tracking;

	private float distBetweenMeAndTarget ;
	private float lateFrameDist;
	
	public float damage = 1;
	
	[HideInInspector]
	public bool hitSomething = false;
	[HideInInspector]
	public bool hitPlayer = false;

	private bool lostTarget = false;
	
	public float myRange = 10;
	
	[HideInInspector]
	public GameObject masterTurret;
	
	[HideInInspector]
	public Transform turretMuzzle;
	
	// Use this for initialization
	void Start () 
	{
		rigidbody = GetComponent<Rigidbody>();
		turret_Bandito_Tracking = masterTurret.GetComponent <Turret_Bandito_Tracking>();
		player = GameObject.Find("Player");
		lateFrameDist = Vector3.SqrMagnitude (turret_Bandito_Tracking.myTarget.position - transform.position);
		turretMuzzle = masterTurret.transform.GetChild(0);
		
		Debug.Log ("Setting up no collisions between me and " + masterTurret.name);
		Physics.IgnoreCollision (transform.GetComponent <Collider> (), turretMuzzle.GetComponent<Collider>());
		Physics.IgnoreCollision (transform.GetComponent <Collider> (), masterTurret.GetComponent<Collider>());
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (!hitPlayer && !hitSomething)
		{
			if(!lostTarget)
			{
				if ( turret_Bandito_Tracking.myTarget != null )
					CalculateAimPosition ( turret_Bandito_Tracking.myTarget.position );
				
				rotationNeeded.rotation = Quaternion.Lerp(rotationNeeded.rotation, desiredRotation, Time.deltaTime * turnSpeed);
	
				DistanceBeforeDestruction += Time.deltaTime * mySpeed;
			}
	
			transform.Translate (Vector3.forward * Time.deltaTime * mySpeed);
			
			if (!lostTarget && turret_Bandito_Tracking.myTarget != null)
				distBetweenMeAndTarget = Vector3.SqrMagnitude (turret_Bandito_Tracking.myTarget.position - transform.position);
	
			if (distBetweenMeAndTarget > lateFrameDist) //The arrow got further than the target
				lostTarget = true;
	
			if (DistanceBeforeDestruction>= myRange)
				Destroy (gameObject);
	
			lateFrameDist = distBetweenMeAndTarget;
		}
	}

	void CalculateAimPosition (Vector3 targetPos)
	{
		Vector3 aimPoint = new Vector3 (targetPos.x-rotationNeeded.position.x,targetPos.y-rotationNeeded.position.y,targetPos.z-rotationNeeded.position.z);
		desiredRotation = Quaternion.LookRotation (aimPoint);
	}
	
	void OnTriggerExit (Collider hit)
	{
		if(hit.CompareTag ("Player"))
		{
			lostTarget = true;
		}
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
