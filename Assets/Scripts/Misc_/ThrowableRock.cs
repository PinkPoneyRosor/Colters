using UnityEngine;
using System.Collections;

public class ThrowableRock : MonoBehaviour {

	#region inspector var
	public bool isSelected = false;
	public bool getUpInit = false;
	public bool gettingUp = false;
	public float maxSpeed = 50;
	public float getUpSpeed = 5;
	public float getUpRotateForce = 100;
	public float throwForce = 1000;
	#endregion

	[HideInInspector]
	public bool homingAttackBool = false;
	[HideInInspector]
	public Transform aimHoming;
	[HideInInspector]
	public int selectionNumber = 0;

	Vector3 previousPosition = Vector3.zero;
	
	GameObject player;
	RockThrow throwScript;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
				throwScript = player.GetComponent < RockThrow >();
	}
	
	// Update is called once per frame
	void Update () {

		//This boolean must ALWAYS be verified BEFORE the isSelected one.
		if (getUpInit) 
		{
			getUpInit = false;
			rigidbody.useGravity = false;
			previousPosition = transform.position;
			gettingUp = true;
		}

		if (gettingUp) 
		{
			rigidbody.velocity = new Vector3(0,getUpSpeed);

			RaycastHit hit;
			if(Physics.Raycast (transform.position, -Vector3.up, out hit, Mathf.Infinity))
			{
				if(hit.distance >= 3)
				{
				rigidbody.velocity = Vector3.zero;
				gettingUp = false;
				isSelected = true;
				}
			}
		}

		if (isSelected) //If the rock's in the air, it's now selected, and we nom make sur it won't move.
		{
			gettingUp = false;
			rigidbody.AddTorque (this.transform.forward * getUpRotateForce);
			rigidbody.constraints = RigidbodyConstraints.FreezePosition;
			setSelectionPos();
		} 
		else 
		{
			rigidbody.constraints = RigidbodyConstraints.None; //Else, we make sure there is no more movement constraints.
		}

		if (homingAttackBool) //This variable is activated by the RockThrow script if it detects that the player is aiming at an enemy.
				homingAttack ();

	}

	void FixedUpdate ()
	{
		if(rigidbody.velocity.sqrMagnitude > maxSpeed * maxSpeed) //This is to ensure the rock won't be too fast when accelerating.
		{
			rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
		}
	}

	//To avoid the rock to be difficult to aim, we reactivate gravity only after the first hit, when it's not selected anymore.
	void OnCollisionEnter (Collision collider)
	{
		if (!isSelected) 
		{
			rigidbody.useGravity = true;
			constantForce.force = Vector3.zero;
			homingAttackBool = false;
		}
	}

	//With this method, we make sure that if the player throwed the rock toward an enemy, he can be almost sure he will hit it.
	void homingAttack ()
	{
		if (aimHoming.GetComponent<BasicEnemy> ().canGetHit) 
		{
			Vector3 throwDir = aimHoming.position - this.transform.position;
			throwDir.Normalize ();

			isSelected = false;

			this.rigidbody.constraints = RigidbodyConstraints.None;

			constantForce.force = throwDir * throwForce;
		} 
		else
			homingAttackBool = false;
	}
	
	void setSelectionPos ()
	{
		switch (selectionNumber)
		{
			case 1:
				transform.localPosition = throwScript.setFirstRockPos;
				break;
			case 2:
				transform.localPosition = throwScript.setSecondRockPos;
				break;
			case 3:
				transform.localPosition = throwScript.setThirdRockPos;
				break;
			case 4:
				transform.localPosition = throwScript.setFourthRockPos;
				break;
		}
	}
}
