using UnityEngine;
using System.Collections;

public class CameraTrigger1 : MonoBehaviour {

	public GameObject camera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player" || c.gameObject.tag == "PlayerSoul")
		{
			camera.animation.Play("Camera1");
			Debug.Log("Play Cam!");
		}
	}
}
