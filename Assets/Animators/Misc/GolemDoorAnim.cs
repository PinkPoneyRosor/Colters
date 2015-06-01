using UnityEngine;
using System.Collections;

public class GolemDoorAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player")
		{
			this.animation.Play("GolemDoorHead");
		}
	}
}
