using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	#region inspector var
	public bool isSelected = false;
	public bool getUp = false;
	public float maxSpeed = 50;
	public float getUpForce = 600;
	public float getUpRotateForce = 100;
	public float throwForce = 1000;
	#endregion

	[HideInInspector]
	public bool homingAttackBool = false;
	[HideInInspector]
	public Transform aimHoming;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		//This boolean must ALWAYS be verified BEFORE the isSelected one.
		if (getUp) 
		{
			getUpNow();
			getUp = false;
		}

		if (isSelected) //If the rock's in the air, it's now selected, and we nom make sur it won't move.
		{
			getUp = false;
			rigidbody.constraints = RigidbodyConstraints.FreezePosition;
			rigidbody.useGravity = false;
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

	//Get the rock to be levitating before being considered selected.
	void getUpNow()
	{
		rigidbody.AddForce (Vector3.up * getUpForce);
		rigidbody.AddTorque (this.transform.forward * getUpRotateForce);
		StartCoroutine("holdIt");
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

	//Letting time to the rock to be in levitation before being considered selected.
	IEnumerator holdIt ()
	{
		yield return new WaitForSeconds (.2f);
		isSelected = true;
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

}
