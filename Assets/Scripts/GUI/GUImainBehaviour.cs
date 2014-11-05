using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImainBehaviour : MonoBehaviour {


	ThirdPersonCamera mainCameraScript;
	GameObject crossHair;
	GameObject mainCamera;
	Image crossHairPic;

	// Use this for initialization
	void Start () 
	{
		mainCamera = Camera.main.gameObject;
		crossHair = GameObject.Find ("CrossHair");

		mainCameraScript = mainCamera.GetComponent<ThirdPersonCamera> ();
		crossHairPic = crossHair.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		#region Show crosshair in aim mode
		if (mainCameraScript.aimingMode) 
			crossHairPic.enabled = true;
		else
			crossHairPic.enabled = false;
		#endregion
	}

}
