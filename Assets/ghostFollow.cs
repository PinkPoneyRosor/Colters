using UnityEngine;
using System.Collections;

public class ghostFollow : MonoBehaviour {


	PlayerController controllerScript;
	int currentTargetedGhostNumber = 0;
	GameObject currentTargetedGhost;
	CharacterController controller;
	float localDeltaTime;
	ThirdPersonCamera mainCameraScript;
	BirdsEyeCam birdsEyeScript;

	public float speed = 10f;
	bool canGoToNext = true;

	[HideInInspector]
	public bool justGotActivated = false;

	// Use this for initialization
	void Start () {
		controllerScript = this.GetComponent<PlayerController> ();
		controller = this.GetComponent<CharacterController> ();
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
		birdsEyeScript = Camera.main.GetComponent<BirdsEyeCam> ();
	}
	
	// Update is called once per frame
	void Update () 
	{


						//localDeltaTime allows the script to not be influenced by the time scale change.
						localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

						//First, if there's at least one ghost, let's target it.

						if (GameObject.FindGameObjectWithTag ("Action Ghost") != null) {

								if (justGotActivated) {
										currentTargetedGhost = GameObject.Find ("actionGhost_" + currentTargetedGhostNumber);
										birdsEyeScript.followBody = true;
										justGotActivated = false;
								}
						if (canGoToNext) {
								Vector3 GhostPos = currentTargetedGhost.transform.position;
								//Then, let's make Phalene look into it's direction, now.
								transform.LookAt (new Vector3 (GhostPos.x, transform.position.y, GhostPos.z));

								//And let's go forward into that direction, speed of light, to infinity and beyond !
				controller.Move (transform.forward * speed * localDeltaTime); }
						} else {	
								controllerScript.soulMode = false;
								currentTargetedGhostNumber = 0;
								Time.timeScale = 1;
								Time.fixedDeltaTime = .02f;
								mainCameraScript.birdsEyeActivated = false;
								birdsEyeScript.followBody = false;
								this.GetComponent<ghostFollow> ().enabled = false;
						}
				}

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log (collider.name);
		if (collider.CompareTag ("Action Ghost")) 
		{
			canGoToNext = false;
			waitBeforeNextGhost(collider);
		}
	}

	IEnumerator waitBeforeNextGhost(Collider collider)
	{
		Destroy (collider.gameObject);
		currentTargetedGhostNumber ++;
		yield return new WaitForSeconds (.5f);
		Debug.Log ("Waited");
		if (GameObject.FindGameObjectWithTag ("Action Ghost") != null) {
		currentTargetedGhost = GameObject.Find ("actionGhost_" + currentTargetedGhostNumber);
	}
		canGoToNext = true;
	}

}
