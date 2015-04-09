using UnityEngine;
using System.Collections;

public class Projectile_Tracking : MonoBehaviour {
	
	public float mySpeed = 10f;
	public float myRange = 10f;
	public float damage = 1;
	public float turnSpeed = 50f;
	private float DistanceBeforeDestruction;
	public Transform rotationNeeded;
	public Rigidbody rigidbody;

	private Quaternion desiredRotation = Quaternion.identity;
	private Turret_Bandito_Tracking turret_Bandito_Tracking;

	private float distBetweenMeAndTarget ;
	private float lateFrameDist;

	private bool lostTarget = false;

	[HideInInspector]
	public GameObject masterTurret;
	
	// Use this for initialization
	void Start () 
	{
		rigidbody = GetComponent<Rigidbody>();
		turret_Bandito_Tracking = masterTurret.GetComponent <Turret_Bandito_Tracking>();

		lateFrameDist = Vector3.SqrMagnitude (turret_Bandito_Tracking.myTarget.position - transform.position);
	}
	
	// Update is called once per frame
	void Update () 
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

	void CalculateAimPosition (Vector3 targetPos)
	{
		Vector3 aimPoint = new Vector3(targetPos.x-rotationNeeded.position.x,targetPos.y-rotationNeeded.position.y,targetPos.z-rotationNeeded.position.z);
		desiredRotation = Quaternion.LookRotation (aimPoint);
	}

	void OnTriggerEnter (Collider hit)
	{
		if(hit.CompareTag ("Player"))
		{
			hit.SendMessage("GetHurt", damage, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnTriggerExit (Collider hit)
	{
		if(hit.CompareTag ("Player"))
		{
			lostTarget = true;
		}
	}

}
