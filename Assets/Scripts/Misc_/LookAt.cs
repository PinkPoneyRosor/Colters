using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {


	public Transform Player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Player);
	}

	
	/*void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			transform.LookAt(Player);
		}
		
	}*/

}
