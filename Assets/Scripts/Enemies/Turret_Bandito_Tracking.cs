using UnityEngine;
using System.Collections;

public class Turret_Bandito_Tracking : MonoBehaviour {
	
	public GameObject myProjectile;
	public float reloadTime = 1f;
	public float turnSpeed = 5f;
	public float firePauseTime = .25f;
	public GameObject muzzleEffect;
	public float errorAmount = .001f;

	[HideInInspector]
	public Transform myTarget = null;

	public Transform muzzle;
	public float currentHealthPoint = 10f;
	
	private float nextFireTime;
	private float nextMoveTime;
	private Quaternion desiredRotation;
	private float aimError;
	
	// Use this for initialization
	void Start () 
	{
		muzzle = this.transform.GetChild (0);
	}
	
	// Update is called once per frame
	void Update () {


		if(myTarget != null)
			Debug.Log ("Name1 = "+myTarget.name);
		
		if(myTarget)
		{
			Debug.Log ("Name2 = "+myTarget.name);
			if(Time.time >= nextMoveTime){
				CalculateAimPosition(myTarget.position);
				transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime*turnSpeed);
				
			}
			if (Time.time >= nextFireTime){
				FireProjectile();
				Debug.Log ("TRACKING TURRET FIRED!!!");
			}
		}
	}
	
	
	void OnTriggerEnter(Collider other){
		
		if(other.gameObject.tag == "Player"){
			nextFireTime = Time.time+(reloadTime*1);
			myTarget = other.gameObject.transform;
			Debug.Log ("Name0 = "+myTarget.name);
		}
	}
	
	void OnTriggerExit(Collider other){
		
		if(other.gameObject.transform == myTarget)
		{
			myTarget = null;
		}
	}
	
	void CalculateAimPosition (Vector3 targetPos)
	{
		Vector3 aimPoint = new Vector3(targetPos.x - transform.position.x, targetPos.y - transform.position.y, targetPos.z - transform.position.z);
		desiredRotation = Quaternion.LookRotation (aimPoint);
	}

	void CalculateAimError()
	{
		aimError = Random.Range(-errorAmount, errorAmount);
	}
	
	void FireProjectile ()
	{
		nextFireTime = Time.time + reloadTime;
		nextMoveTime = Time.time + firePauseTime;
		CalculateAimError();
		Debug.Log ("Well almost...");

			Debug.Log ("BANG BANG");
			GameObject spawnedArrow;

			spawnedArrow = Instantiate (myProjectile, muzzle.position, transform.rotation) as GameObject;

			Projectile_Tracking projectileScript = spawnedArrow.GetComponent <Projectile_Tracking>();
			projectileScript.masterTurret = this.gameObject;
	}
	
	
}
