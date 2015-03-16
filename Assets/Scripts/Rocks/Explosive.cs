using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour {

	private NewThrowableRock throwableRock;
	public Transform explosionPrefab;
	
	[SerializeField]
	private float velocityNeededForExplosion = 4;

	// Use this for initialization
	void Start () 
	{
		throwableRock = GetComponent <NewThrowableRock> ();
	}

	void OnCollisionEnter(Collision collision) 
	{
		if(throwableRock.canExplode == true
		   && rigidbody.velocity.sqrMagnitude > velocityNeededForExplosion * velocityNeededForExplosion)
		{
			ContactPoint contact = collision.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Instantiate (explosionPrefab, pos, rot);
			Destroy(gameObject);
		}
	}
}
