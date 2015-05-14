using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour {

	public Transform explosionPrefab;
	
	[SerializeField]
	private float velocityNeededForExplosion = 4;

	void OnCollisionEnter(Collision collision) 
	{
		if(rigidbody.velocity.sqrMagnitude > velocityNeededForExplosion * velocityNeededForExplosion)
		{
			ContactPoint contact = collision.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Instantiate (explosionPrefab, pos, rot);
			Destroy(gameObject);
		}
	}
}
