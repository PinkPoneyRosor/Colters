using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

	public bool isSelected = false;
	public bool getUp = false;

	RockThrow RockThrowScript;
	Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player").transform;
		RockThrowScript = player.GetComponent<RockThrow> ();
	}
	
	// Update is called once per frame
	void Update () {

		//This boolean must ALWAYS be executed BEFORE the isSelected one.
		if (getUp) {
			getUpNow();
			getUp = false;
				}

		if (isSelected) {
			Debug.Log (this.name+(" is selected"));
						getUp = false;
						rigidbody.constraints = RigidbodyConstraints.FreezePosition;
						rigidbody.useGravity = false;
				} else {
						rigidbody.constraints = RigidbodyConstraints.None;
						//rigidbody.useGravity = true;
				}

	}

	void getUpNow()
	{
		rigidbody.AddForce (Vector3.up * 300);
		StartCoroutine("holdIt");


		}

	void OnCollisionEnter (Collision collider)
	{
		if (!isSelected)
						rigidbody.useGravity = true;
		 }

	IEnumerator holdIt ()
	{
		Debug.Log ("Holding it");
		yield return new WaitForSeconds (1);
		isSelected = true;
		Debug.Log ("Finished");
	}
}
