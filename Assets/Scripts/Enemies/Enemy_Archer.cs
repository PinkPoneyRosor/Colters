using UnityEngine;
using System.Collections;

public class Enemy_Archer : MonoBehaviour {

	private GameObject player;
	private bool holdFire = false;
	private Transform myTarget = null;
	private Quaternion desiredRotation;
	private float nextFireTime;
	private float nextMoveTime;
	
	public LayerMask sightObstructionLayers;
	public float turnSpeed = 5f;
	public float reloadTime = 1f;
	public float firePauseTime = .25f;
	public GameObject myProjectile;
	public float errorAmount = .001f;
	
	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		RaycastHit rayHit;
		
		Physics.Raycast ( transform.position,(player.transform.position - transform.position).normalized, out rayHit, Mathf.Infinity, sightObstructionLayers );
		
		if (rayHit.collider == player.collider)
			holdFire = false;
		else
			holdFire = true;
		
		if(myTarget)
		{
			if(Time.time >= nextMoveTime)
			{
				CalculateAimPosition(myTarget.position);
				transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * turnSpeed);
				
			}
			if (Time.time >= nextFireTime && !holdFire)
			{
				FireProjectile();
			}
		}
	}
	
	void CalculateAimPosition (Vector3 targetPos)
	{
		Vector3 aimPoint = new Vector3(targetPos.x - transform.position.x, targetPos.y - transform.position.y, targetPos.z - transform.position.z);
		desiredRotation = Quaternion.LookRotation (aimPoint);
	}
	
	void FireProjectile()
	{
		nextFireTime = Time.time + reloadTime;
		nextMoveTime = Time.time + firePauseTime;
		
		GameObject spawnedArrow;
		
		spawnedArrow = Instantiate (myProjectile, transform.position, transform.rotation) as GameObject;
		
		
		//TO-DO : Let's check here if our arrow is a homing one or not, and if yes, let's give it a target.
		Arrow_Homing projectileScript = spawnedArrow.GetComponent <Arrow_Homing>();
		projectileScript.masterTurret = this.gameObject;
		projectileScript.target = myTarget;
	}
	
	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			nextFireTime = Time.time + (reloadTime * 1);
			myTarget = other.gameObject.transform;
		}
	}
}
