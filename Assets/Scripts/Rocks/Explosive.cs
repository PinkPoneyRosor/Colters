using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour {


	private ThrowableRock throwableRock;
	public Transform explosionPrefab;



	// Use this for initialization
	void Start () {
		throwableRock = GetComponent<ThrowableRock> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnCollisionEnter(Collision collision) {

		if(throwableRock.canExplode ==true){
		ContactPoint contact = collision.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		Instantiate (explosionPrefab, pos, rot);
		Destroy(gameObject);

		
		}
	}




}
