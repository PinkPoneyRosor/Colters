using UnityEngine;
using System.Collections;

public class Projectile_Tracking : Projectile_Global {
	
	public float turnSpeed = 50f;
	private float DistanceBeforeDestruction;
	public Transform rotationNeeded;
	public Rigidbody rigidbody;

	private Quaternion desiredRotation = Quaternion.identity;
	private Turret_Bandito_Tracking turret_Bandito_Tracking;

	private float distBetweenMeAndTarget ;
	private float lateFrameDist;

	private bool lostTarget = false;
	
	// Use this for initialization
	void Start () 
	{
		rigidbody = GetComponent<Rigidbody>();
		turret_Bandito_Tracking = masterTurret.GetComponent <Turret_Bandito_Tracking>();
		player = GameObject.Find("Player");
		lateFrameDist = Vector3.SqrMagnitude (turret_Bandito_Tracking.myTarget.position - transform.position);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!hitPlayer && !hitSomething)
		{
			if(!lostTarget)
			{
				if ( turret_Bandito_Tracking.myTarget != null )
					CalculateAimPosition ( turret_Bandito_Tracking.myTarget.position );
				
				rotationNeeded.rotation = Quaternion.Lerp(rotationNeeded.rotation, desiredRotation, Time.deltaTime * turnSpeed);
	
				DistanceBeforeDestruction += Time.deltaTime * mySpeed;
			}
	
			transform.Translate (Vector3.forward * Time.deltaTime * mySpeed);
			
			if(!lostTarget && turret_Bandito_Tracking.myTarget != null)
				distBetweenMeAndTarget = Vector3.SqrMagnitude (turret_Bandito_Tracking.myTarget.position - transform.position);
	
			if( distBetweenMeAndTarget > lateFrameDist) //The arrow got further than the target
				lostTarget = true;
	
			if (DistanceBeforeDestruction>= myRange)
				Destroy(gameObject);
	
			lateFrameDist = distBetweenMeAndTarget;
		}
		
		turretMuzzle = masterTurret.transform.GetChild(0);
		Physics.IgnoreCollision(transform.GetComponent<Collider>(), turretMuzzle.GetComponent<Collider>());
		Physics.IgnoreCollision(transform.GetComponent<Collider>(), masterTurret.GetComponent<Collider>());
	}

	void CalculateAimPosition (Vector3 targetPos)
	{
		Vector3 aimPoint = new Vector3(targetPos.x-rotationNeeded.position.x,targetPos.y-rotationNeeded.position.y,targetPos.z-rotationNeeded.position.z);
		desiredRotation = Quaternion.LookRotation (aimPoint);
	}
	
	void OnTriggerExit (Collider hit)
	{
		if(hit.CompareTag ("Player"))
		{
			lostTarget = true;
		}
	}

}
