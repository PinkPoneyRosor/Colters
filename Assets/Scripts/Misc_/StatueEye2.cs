using UnityEngine;
using System.Collections;

public class StatueEye2 : MonoBehaviour {
	
	
	
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	void	OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Statue Orbit 2") {
			GameObject.Destroy(gameObject);
		}
	}
	
	
}
