using UnityEngine;
using System.Collections;

public class PassageRiver : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "ThrowableRock")
		{
			this.animation.Play("PassageRiviere");
			Debug.Log("Play!");
		}
	}
	
}
