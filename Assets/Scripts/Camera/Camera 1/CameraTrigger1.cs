using UnityEngine;
using System.Collections;

public class CameraTrigger1 : MonoBehaviour {

	private GameObject mainGameCamera;
	private GameObject HUD;
	private GUImainBehaviour HUDScript;
	

	// Use this for initialization
	void Start () 
	{
		mainGameCamera = Camera.main.gameObject;
		HUD = GameObject.Find("GameHUD") as GameObject;
		HUDScript = HUD.GetComponent <GUImainBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player" || c.gameObject.tag == "PlayerSoul")
		{
			mainGameCamera.animation.Play("Camera1");
			Debug.Log("Play Cam!");
			HUDScript.paused = true;
			HUD.GetComponent <Canvas>().enabled = false;
		}
	}
}
