using UnityEngine;
using System.Collections;

public class StatueEye1 : MonoBehaviour {





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void	OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Statue Orbit 1") {
			GameObject.Destroy(gameObject);
		}
	}


}
