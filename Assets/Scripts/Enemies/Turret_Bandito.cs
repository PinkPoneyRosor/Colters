using UnityEngine;
using System.Collections;

public class Turret_Bandito : MonoBehaviour {

	public GameObject myProjectile;
	public float reloadTime = 1f;
	public float turnSpeed = 5f;
	public float firePauseTime = .25f;
	public GameObject muzzleEffect;
	public float errorAmount = .001f;
	public Transform myTarget;
	public Transform[] muzzlePositions;
	public Transform turretBall;
	public float currentHealthPoint = 10f;

	private float nextFireTime;
	private float nextMoveTime;
	private Quaternion desiredRotation;
	private float aimError;





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(myTarget){
			if(Time.time >= nextMoveTime){
				CalculateAimPosition(myTarget.position);
				turretBall.rotation = Quaternion.Lerp(turretBall.rotation, desiredRotation, Time.deltaTime*turnSpeed);

			}
			if (Time.time >= nextFireTime){
				FireProjectile();
			}
		}
	}


	void OnTriggerEnter(Collider other){

		RaycastHit rayHit;
		
		if(other.gameObject.tag == "Player")
		{
			if(Physics.Raycast ( transform.position, other.transform.position, out rayHit, Mathf.Infinity ))
			{
				nextFireTime = Time.time + (reloadTime * 1);
				myTarget = other.gameObject.transform;
			}
		}
	}

	void OnTriggerExit(Collider other){
		
		if(other.gameObject.transform == myTarget){
			myTarget = null;
		}
	}

	void CalculateAimPosition (Vector3 targetPos){
		Vector3 aimPoint = new Vector3(targetPos.x-turretBall.position.x,targetPos.y-turretBall.position.y,targetPos.z-turretBall.position.z);
		desiredRotation = Quaternion.LookRotation (aimPoint);
	}
	void CalculateAimError(){
		aimError = Random.Range(-errorAmount, errorAmount);
	}

	void FireProjectile(){
		nextFireTime = Time.time+reloadTime;
		nextMoveTime = Time.time+firePauseTime;
		CalculateAimError();

		foreach (Transform theMuzzlePos in muzzlePositions)
		{

		Instantiate (myProjectile, theMuzzlePos.position, theMuzzlePos.rotation);
		}
	}


}
