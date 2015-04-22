using UnityEngine;
using System.Collections;

public class Enemy_Archer : MonoBehaviour {

	private GameObject player;
	private bool holdFire = false;
	private Quaternion desiredRotation;

	private float nextMoveTime;
	
	public LayerMask sightObstructionLayers;
	public float turnSpeed = 5f;
	public float reloadTime = 1f;
	public float firePauseTime = .25f;
	public GameObject myProjectile;
	public float errorAmount = .001f;
	
	[HideInInspector]
	public GameObject myTarget = null;
	[HideInInspector]
	public float nextFireTime;
	
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
				CalculateAimPosition(myTarget.transform.position);
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
		
		if (spawnedArrow.CompareTag ("HomingProjectile"))
		{
			Arrow_Homing projectileScript = spawnedArrow.GetComponent <Arrow_Homing>();
			projectileScript.masterTurret = this.gameObject;
			projectileScript.target = myTarget;
		}
		else
		{
			Arrow_Normal projectileScript = spawnedArrow.GetComponent <Arrow_Normal>();
			projectileScript.masterTurret = this.gameObject;
		}
	}
}
