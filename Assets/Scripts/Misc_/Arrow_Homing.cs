using UnityEngine;
using System.Collections;

public class Arrow_Homing : MonoBehaviour {

	public float turnSpeed = 50f;
	public float mySpeed = 10;
	public float myRange = 10;
	public float damage = 1;

	private GameObject player;
	private float lateFrameDist;
	private bool hitSomething = false;
	private bool hitPlayer = false;
	private bool lostTarget = false;
	private Quaternion rotationNeeded;
	private Quaternion desiredRotation = Quaternion.identity;
	private float distBetweenMeAndTarget;
	private float myDist = 0;

	[HideInInspector]
	public GameObject masterTurret;
	[HideInInspector]
	public GameObject target;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find("Player");
		lateFrameDist = Vector3.SqrMagnitude (target.transform.position - transform.position);
	}
	
	// Update is called once per frame
	void Update () 
	{
		float distFromMaster = Vector3.SqrMagnitude(transform.position - masterTurret.transform.position);
		if (distFromMaster > 2 * 2)
		{
			collider.enabled = true;
		}
	
		if(!hitSomething)
		{
			myDist += Time.deltaTime * mySpeed;
			
			if (myDist >= myRange)
				Destroy(gameObject);
		}
	
		if (!hitPlayer && !hitSomething)
		{
			if(!lostTarget)
			{
				if ( target != null )
					this.CalculateAimPosition();
			}
			
			transform.Translate (Vector3.forward * Time.deltaTime * mySpeed);
			
			if (!lostTarget && target != null)
				distBetweenMeAndTarget = Vector3.SqrMagnitude (target.transform.position - transform.position);
			
			if (distBetweenMeAndTarget > lateFrameDist) //The arrow got further than the target
				lostTarget = true;
			
			lateFrameDist = distBetweenMeAndTarget;
		}
	}
	
	void CalculateAimPosition ()
	{
		rotationNeeded = Quaternion.LookRotation (target.transform.position - transform.position);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotationNeeded, Time.deltaTime * turnSpeed);
	}
	
	void OnTriggerEnter (Collider hit)
	{
		Debug.Log ("Homing hit "+ hit.name);
							
		if (hit.CompareTag ("Player"))
		{
			hit.SendMessage("GetHurt", damage, SendMessageOptions.DontRequireReceiver);
			hitPlayer = true;
		}
		
		if(hit.gameObject != masterTurret)
		{
			hitSomething = true;
			transform.SetParent (hit.transform, true);
			StartCoroutine(Disappear());
		}
		else
		{
			hitSomething = false;
		}
	}
	
	IEnumerator Disappear ()
	{
		yield return new WaitForSeconds(2);
		Destroy (this.gameObject);
	}
}
