﻿using UnityEngine;
using System.Collections;

public class NewThrowableRock : MonoBehaviour {

	public bool isSelected = false;
	public bool isGrowing = false;
	public bool inTheAir = false;
	[HideInInspector]
	public int selectionNumber = 0;
	
	public Vector3 normalScale;
	
	[HideInInspector]
	public bool homingAttackBool = false;
	[HideInInspector]
	public Transform aimHoming;
	[HideInInspector]
	public bool beingThrowned = false;
	[HideInInspector]
	public int throwedRockNumber = 0;
	[HideInInspector]
	public bool isThrowed = false;
	
	public float throwForce = 1000;
	
	public bool canExplode = false;
	public Vector3 posAtLaunch = Vector3.zero;
	public float maxTravelDistance = 15;
	public float DecelerationRate = 15;
	public float maxVelocityWhenDecelerating = 22;
	public float growingRate = .5f;
	
	public GameObject EarthQuakeParticles;
	
	public bool Melee = false;
	
	public GameObject ImpactParticles;
	public GameObject ImpactPrefab;
	
	private GameObject player;
	
	[HideInInspector]
	public bool growingMyself = true;
	
	bool meleeAlreadyHit = false;
	
	Vector3 startScale;
	
	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find ("Player");
		startScale = this.transform.localScale;
		transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.localScale = Vector3.Lerp (transform.localScale, startScale, Time.deltaTime * 10);
	
	
		#region track distance travelled
		/*if (Vector3.SqrMagnitude(transform.position - posAtLaunch) > maxTravelDistance * maxTravelDistance && !isSelected && isThrowed)
		{
			rigidbody.useGravity = true;
			constantForce.force = Vector3.zero;
			
			if (Vector3.SqrMagnitude(rigidbody.velocity) > maxVelocityWhenDecelerating * maxVelocityWhenDecelerating)
			{
				rigidbody.drag = DecelerationRate;
			}
			else
				rigidbody.drag = 0;
			
			homingAttackBool = false;
		}*/
		#endregion
		
		if (homingAttackBool)
			homingAttack();
	}
	
	//With this method, we make sure that if the player throwed the rock toward an enemy, he can be almost sure he will hit it.
	void homingAttack ()
	{
		Debug.Log ("Homing");
		if (aimHoming.GetComponent <BasicEnemy> ().canGetHit) 
		{	
			Vector3 throwDir = aimHoming.position - this.transform.position;
			throwDir.Normalize ();
			
			this.rigidbody.constraints = RigidbodyConstraints.None;
			
			constantForce.force = throwDir * throwForce;
		} 
		else
			homingAttackBool = false;
	}
	
	//To avoid the rock to be difficult to aim, we reactivate gravity only after the first hit, when it's not selected anymore.
	void OnCollisionEnter (Collision collider)
	{
		Debug.Log ("Rock Hit");
		JustHitSomething();
		
		GameObject impactGameObject;
		
		impactGameObject = Instantiate (ImpactPrefab, transform.position, Quaternion.identity) as GameObject;
		
		AudioSource impactSound = impactGameObject.GetComponent <AudioSource> ();
		
		if (Vector3.SqrMagnitude (rigidbody.velocity) >= 50)
		{
			//impactSound.volume = .5f;
			impactSound.pitch = 1;
		}
		else if (Vector3.SqrMagnitude (rigidbody.velocity) < 50 && Vector3.SqrMagnitude (rigidbody.velocity) >= 30)
		{
			//impactSound.volume = .4f;
			impactSound.pitch = .8f;
		}
		else if (Vector3.SqrMagnitude (rigidbody.velocity) < 30 && Vector3.SqrMagnitude (rigidbody.velocity) > 10)
		{
			//impactSound.volume = .3f;
			impactSound.pitch = .6f;
		}
		else if (Vector3.SqrMagnitude (rigidbody.velocity) <= 10)
		{
			//impactSound.volume = .2f;
			impactSound.pitch = .5f;
		}
		
		if (Melee && !meleeAlreadyHit)
		{
			GameObject EarthQuakeParticlesInstance;
			EarthQuakeParticlesInstance = Instantiate(EarthQuakeParticles, transform.position , Quaternion.Euler(90,0,0) ) as GameObject;
			meleeAlreadyHit = true;
		}
		
		if (ImpactParticles != null)
		Instantiate (ImpactParticles, transform.position, Quaternion.identity);
	}
	
	void JustHitSomething ()
	{
		if (!isSelected) 
		{
			rigidbody.useGravity = true;
			Debug.Log ("Gravity on");
			constantForce.force = Vector3.zero;
			homingAttackBool = false;
			beingThrowned = false;
		}
	}
	
	void InstantGrow ()
	{
		growingMyself = false;
	}
	
}
