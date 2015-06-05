using UnityEngine;
using System.Collections;

public class ExplosionHurt : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter (Collider hit)
	{
		if(hit.CompareTag ("Enemy"))
		{
			hit.SendMessage ("gotHit", 5);
			Debug.Log ("Explosion touched enemy");
		}
	}
}
