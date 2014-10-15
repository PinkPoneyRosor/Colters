using UnityEngine;
using System.Collections;

public class SoulMode : MonoBehaviour {

	float localDeltaTime;
	float horizontal;
	float vertical;
	public float speed = 4;
	CharacterController controller;
	PlayerController playerScript;

	ThirdPersonCamera mainCameraScript;

	// Use this for initialization
	void Start () {
		this.name = "Soul";
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
		mainCameraScript.birdsEyeActivated = true;
		playerScript = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();

		controller = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		if (Input.GetButtonDown ("SwitchMode")) 
		{
			mainCameraScript.birdsEyeActivated = false;
			playerScript.soulMode = false;
			Destroy (this.gameObject);
		}

		#region Get Axises
		//Get input from the main axis (Keyboard and stick)
		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");
		#endregion

		controller.Move (transform.forward * vertical * speed * localDeltaTime);
		controller.Move (transform.right * horizontal * speed * localDeltaTime);

		if(Input.GetButtonDown("Action"))
			placeGhost();
	}

	void placeGhost()
	{
		RaycastHit pointHit;
		if (Physics.Raycast (transform.position, -transform.up,out pointHit, 50)) {
		
		}
	}

}
