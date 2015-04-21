using UnityEngine;
using System.Collections;

public class Arrow_Homing : MonoBehaviour {

	public float turnSpeed = 50f;
	public float DistanceBeforeDestruction;
	public float mySpeed = 10;
	public float myRange = 10;

	private GameObject player;
	private float lateFrameDist;
	private bool hitSomething = false;
	private bool hitPlayer = false;
	private bool lostTarget = false;
	private Transform rotationNeeded;
	private Quaternion desiredRotation = Quaternion.identity;
	private float distBetweenMeAndTarget;

	[HideInInspector]
	public GameObject masterTurret;
	[HideInInspector]
	public Transform target;
	


	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find("Player");
		lateFrameDist = Vector3.SqrMagnitude (target.position - transform.position);
		Physics.IgnoreCollision (transform.GetComponent <Collider> (), masterTurret.GetComponent <Collider>());
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log (target.position);
	
		if (!hitPlayer && !hitSomething)
		{
			if(!lostTarget)
			{
				if ( target != null )
					this.CalculateAimPosition();
				
				rotationNeeded.rotation = Quaternion.Lerp(rotationNeeded.rotation, desiredRotation, Time.deltaTime * turnSpeed);
				
				DistanceBeforeDestruction += Time.deltaTime * mySpeed;
			}
			
			transform.Translate (Vector3.forward * Time.deltaTime * mySpeed);
			
			if (!lostTarget && target != null)
				distBetweenMeAndTarget = Vector3.SqrMagnitude (target.position - transform.position);
			
			if (distBetweenMeAndTarget > lateFrameDist) //The arrow got further than the target
				lostTarget = true;
			
			if (DistanceBeforeDestruction>= myRange)
				Destroy (gameObject);
			
			lateFrameDist = distBetweenMeAndTarget;
		}
	}
	
	void CalculateAimPosition ()
	{
		Vector3 aimPoint = new Vector3 (target.position.x - rotationNeeded.position.x, target.position.y - rotationNeeded.position.y, target.position.z - rotationNeeded.position.z);
		desiredRotation = Quaternion.LookRotation (aimPoint);
	}
}
