using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImainBehaviour : MonoBehaviour {


	ThirdPersonCamera mainCameraScript;
	GameObject crossHair;
	GameObject mainCamera;
	Image crossHairPic;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main.gameObject;
		mainCameraScript = mainCamera.GetComponent<ThirdPersonCamera> ();
		crossHair = GameObject.Find ("CrossHair");
		crossHairPic = crossHair.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!mainCameraScript.birdsEyeActivated && mainCameraScript.aimingMode) 
			crossHairPic.enabled = true;
		else
			crossHairPic.enabled = false;
	}

}
