using UnityEngine;
using System.Collections;

public class Turret_GlobalBehaviour : MonoBehaviour 
{

	public GameObject myProjectile;
	public float reloadTime = 1f;
	public float turnSpeed = 5f;
	public float firePauseTime = .25f;
	public GameObject muzzleEffect;
	public float errorAmount = .001f;
	
	[HideInInspector]
	public Transform myTarget = null;
	
	[HideInInspector]
	public Transform muzzle;
	public Transform turretBall;
	public float currentHealthPoint = 10f;
	
	[HideInInspector]
	public float nextFireTime;
	[HideInInspector]
	public float nextMoveTime;
	[HideInInspector]
	public Quaternion desiredRotation;
	[HideInInspector]
	public float aimError;
	[HideInInspector]
	public Transform player;
	[HideInInspector]
	public bool holdFire = false;
	
	public LayerMask sightObstructionLayers;

	// Use this for initialization
	public void Start () 
	{
		muzzle = this.transform.GetChild (0);
		player = GameObject.Find ("Player").transform;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		
		RaycastHit rayHit;
		
		Physics.Raycast ( transform.position,(player.position - transform.position).normalized, out rayHit, Mathf.Infinity, sightObstructionLayers );
		
		if (rayHit.collider == player.collider)
			holdFire = false;
		else
			holdFire = true;
		
		if(myTarget)
		{
			if(Time.time >= nextMoveTime)
			{
				CalculateAimPosition(myTarget.position);
				turretBall.rotation = Quaternion.Lerp(turretBall.rotation, desiredRotation, Time.deltaTime * turnSpeed);
				
			}
			if (Time.time >= nextFireTime && !holdFire)
			{
				FireProjectile();
			}
		}
	}
	
	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			nextFireTime = Time.time + (reloadTime * 1);
			myTarget = other.gameObject.transform;
		}
	}
	
	public void OnTriggerExit(Collider other)
	{
		if(other.gameObject.transform == myTarget)
			myTarget = null;
	}
	
	public void CalculateAimPosition (Vector3 targetPos)
	{
		Vector3 aimPoint = new Vector3(targetPos.x-turretBall.position.x,targetPos.y-turretBall.position.y,targetPos.z-turretBall.position.z);
		desiredRotation = Quaternion.LookRotation (aimPoint);
	}
	
	public void CalculateAimError()
	{
		aimError = Random.Range(-errorAmount, errorAmount);
	}
	
	public virtual void FireProjectile ()
	{
	}
}
