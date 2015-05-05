using UnityEngine;
using System.Collections;

public class FallingBridge : MonoBehaviour {


	
	public float speed = 5;
	public bool falling = false;
	public bool soulIsHere = false;

	// Use this for initialization
	void Start () {
		
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
	
	void OnTriggerEnter(Collider c){
		
		if(c.gameObject.tag == "Player")
		{
			falling = true;
			rigidbody.constraints = RigidbodyConstraints.None;
			StartCoroutine ("GottaGetDestroyed");
		}

		if(c.gameObject.tag == "PlayerSoul")
		{
	
			falling = true;
			soulIsHere = true;
			rigidbody.constraints = RigidbodyConstraints.None;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(falling == true){
			transform.Translate(Vector3.down * speed * Time.deltaTime);
		}
	}

	IEnumerator GottaGetDestroyed ()
	{
		yield return new WaitForSeconds (1);
		Destroy(this.gameObject);
	}



}
