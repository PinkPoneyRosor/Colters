using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImainBehaviour : MonoBehaviour {

	//External scripts, objects, files,...
	ThirdPersonCamera mainCameraScript;

	GameObject crossHair;
	GameObject soulBar;
	GameObject player;

	GameObject mainCamera;

	PlayerController playerScript;

	Image crossHairPic;
	Slider soulBarSlide;

	float localDeltaTime;

	public float SoulBarSpeedRate = .1f;

	// Use this for initialization
	void Start () 
	{
		mainCamera = Camera.main.gameObject;
		crossHair = GameObject.Find ("CrossHair");
		soulBar = GameObject.Find ("SoulBar");
		player = GameObject.Find ("Player");

		mainCameraScript = mainCamera.GetComponent<ThirdPersonCamera> ();
		crossHairPic = crossHair.GetComponent<Image> ();
		soulBarSlide = soulBar.GetComponent<Slider> ();
		playerScript = player.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		#region Show crosshair in aim mode
		if (mainCameraScript.aimingMode) 
			crossHairPic.enabled = true;
		else
			crossHairPic.enabled = false;
		#endregion

		#region Soul Bar
		if(playerScript.soulMode)
		{
			soulBarSlide.value -= SoulBarSpeedRate * localDeltaTime;
		}
		else if (soulBarSlide.value < 1)
		{
			soulBarSlide.value += SoulBarSpeedRate * localDeltaTime;
		}
		#endregion
	}

}
