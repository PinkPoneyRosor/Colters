using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImainBehaviour : MonoBehaviour {

	//External scripts, objects, files,...
	ThirdPersonCamera mainCameraScript;

	GameObject crossHair;
	GameObject soulBar;

	GameObject mainCamera;

	Image crossHairPic;
	Image soulBarPic;

	// Use this for initialization
	void Start () 
	{
		mainCamera = Camera.main.gameObject;
		crossHair = GameObject.Find ("CrossHair");
		soulBar = GameObject.Find ("soulBar");

		mainCameraScript = mainCamera.GetComponent<ThirdPersonCamera> ();
		crossHairPic = crossHair.GetComponent<Image> ();
		soulBarPic = soulBar.GetComponent<Image> ();
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

		#region Soul Bar

		#endregion
	}

}
