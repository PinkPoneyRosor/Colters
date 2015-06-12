using UnityEngine;
using System.Collections;

public class CameraTrigger1 : MonoBehaviour {

	private GameObject mainGameCamera;
	private GameObject HUD;
	private GUImainBehaviour HUDScript;
	
	private GameObject player;
	private PlayerController playerScript; 
	private GameObject soul;
	
	private bool alreadyTriggered = false;

	// Use this for initialization
	void Start () 
	{
		mainGameCamera = Camera.main.gameObject;
		HUD = GameObject.Find("GameHUD") as GameObject;
		HUDScript = HUD.GetComponent <GUImainBehaviour>();
		
		player = GameObject.Find ("Player");
		playerScript = player.GetComponent <PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c)
	{
		if ( (c.gameObject.tag == "Player" || c.gameObject.tag == "PlayerSoul") && !alreadyTriggered)
		{
			mainGameCamera.animation.Play("Camera1");
			Debug.Log("Play Cam!");
			HUDScript.paused = true;
			HUD.GetComponent <Canvas>().enabled = false;
			
			alreadyTriggered = true;
			
			if (playerScript.soulMode)
			{
				soul = GameObject.Find ("Soul");
				soul.SendMessage ("revertBack", true);
			}
		}
	}
}
