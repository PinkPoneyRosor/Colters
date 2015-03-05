using UnityEngine;
using System.Collections;

public class pillarFall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter (Collision hit)
	{
		if(hit.collider.CompareTag("ThrowableRock"))
		{
			Transform pillar = transform.GetChild(0);
			pillar.rigidbody.isKinematic = false;
			pillar.rigidbody.useGravity = true;
			this.transform.DetachChildren();
			Destroy (hit.collider.gameObject);
			Destroy (this.gameObject);
		}
	}
}
