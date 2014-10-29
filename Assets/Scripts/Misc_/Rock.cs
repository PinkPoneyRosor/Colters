using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	public bool isSelected = false;
	public bool getUp = false;

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

		if (isSelected) 
		{
			getUp = false;
			rigidbody.constraints = RigidbodyConstraints.FreezePosition;
			rigidbody.useGravity = false;
		} 
		else 
		{
			rigidbody.constraints = RigidbodyConstraints.None;
		}

	}

	//Get the rock to be levitating before being considered selected.
	void getUpNow()
	{
		rigidbody.AddForce (Vector3.up * 300);
		rigidbody.AddTorque (this.transform.forward * 10);
		StartCoroutine("holdIt");
	}

	//To avoid the rock to be difficult to aim, we reactivate gravity only after the first hit, when it's not selected anymore.
	void OnCollisionEnter (Collision collider)
	{
		if (!isSelected) 
		{
			rigidbody.useGravity = true;
			constantForce.force = Vector3.zero; // BUG ! FIX IT !!!
		}
	}

	//Letting time to the rock to be in levitation before being considered selected.
	IEnumerator holdIt ()
	{
		yield return new WaitForSeconds (.2f);
		isSelected = true;
	}

}
