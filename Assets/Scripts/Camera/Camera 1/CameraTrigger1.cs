using UnityEngine;
using System.Collections;

public class CameraTrigger1 : MonoBehaviour {

	private GameObject mainGameCamera;

	// Use this for initialization
	void Start () 
	{
		mainGameCamera = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player" || c.gameObject.tag == "PlayerSoul")
		{
			//mainGameCamera.animation.Play("Camera1");
			Debug.Log("Play Cam!");
		}
	}
}
