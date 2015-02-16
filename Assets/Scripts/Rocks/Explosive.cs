using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour {

	private ThrowableRock throwableRock;
	public Transform explosionPrefab;
	
	[SerializeField]
	private float velocityNeededForExplosion = 4;

	// Use this for initialization
	void Start () 
	{
		throwableRock = GetComponent<ThrowableRock> ();
	}

	void OnCollisionEnter(Collision collision) 
	{
		if(throwableRock.canExplode == true
		   && rigidbody.velocity.sqrMagnitude > velocityNeededForExplosion * velocityNeededForExplosion)
		{
			Debug.Log (rigidbody.velocity.sqrMagnitude);
			ContactPoint contact = collision.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Instantiate (explosionPrefab, pos, rot);
			Destroy(gameObject);
		}
	}




}
