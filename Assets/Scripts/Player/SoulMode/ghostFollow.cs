using UnityEngine;
using System.Collections;

public class ghostFollow : MonoBehaviour {

	#region external scripts and objects
	PlayerController controllerScript;
	GameObject currentTargetedGhost;
	CharacterController controller;
	ThirdPersonCamera mainCameraScript;
	SphereCollider sphereCollider;
	#endregion

	#region script behaviour
	float localDeltaTime;
	int currentTargetedGhostNumber = 0;
	bool canGoToNext = true;
	[HideInInspector]
	public bool justGotActivated = false;
	public float dashDamage = 1;
	#endregion

	#region other variables
	public float speed = 10f;
	#endregion

	public bool cameraInPlace = false;


	// Use this for initialization
	void Start () 
	{
		controllerScript = this.GetComponent<PlayerController> ();
		controller = this.GetComponent<CharacterController> ();
		mainCameraScript = Camera.main.GetComponent<ThirdPersonCamera> ();
		sphereCollider = this.GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//This script won't do anything until the camera is in place (See BirdseyeCam script).
		if (cameraInPlace) { 
						//localDeltaTime allows the script to not be influenced by the time scale change.
						localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

						//First, if there's at least one ghost, let's target it.
						if (GameObject.FindGameObjectWithTag ("Action Ghost") != null) {
								IgnoreCollisions (true); //Let's ignore the collisions with the rocks, so they won't block the player during this mode.

								//If this script just got activated, we have to find the first action ghost vefore we go on.
								if (justGotActivated) {
										currentTargetedGhost = GameObject.Find ("actionGhost_" + currentTargetedGhostNumber);
										sphereCollider.enabled = true;
										justGotActivated = false;
								}

								//If we are allowed to get to the next ghost...
								if (canGoToNext) {
										//Let's cache the next ghost's position.
										Vector3 GhostPos = currentTargetedGhost.transform.position;
										//Then, let's make Phalene look into its direction, now.
										transform.LookAt (new Vector3 (GhostPos.x, transform.position.y, GhostPos.z));
										//And let's go forward into that direction, speed of light, to infinity and beyond !
										transform.position = Vector3.Lerp (transform.position, GhostPos, speed * localDeltaTime); 
								}
						}
				}

		if(GameObject.FindGameObjectWithTag("Action Ghost") == null) //If there's no more ghosts, let's reset everything before exiting ghost follow mode.
			resetAndExitMode ();
	}

	//If we encounter somehting...
	void OnTriggerEnter(Collider collider)
	{
		//And that this something is a ghost...
		if (collider.gameObject == currentTargetedGhost) {
				//Stop right there and call our coroutine.
				canGoToNext = false;
				StartCoroutine (waitBeforeNextGhost (collider));
		}
	}

	//When this coroutine is called, the script will stop for a small time.
	//We have to destroy the encountered ghost and tell the script we are now aiming for the next one.
	IEnumerator waitBeforeNextGhost(Collider collider)
	{
		Destroy (collider.gameObject);
		currentTargetedGhostNumber ++;
		yield return new WaitForSeconds (1.5f * localDeltaTime);

		if (GameObject.FindGameObjectWithTag ("Action Ghost") != null)
			currentTargetedGhost = GameObject.Find ("actionGhost_" + currentTargetedGhostNumber);

		canGoToNext = true; //We're good to go, let's tell the update function it can continue Phalene's movements.
	}

	void IgnoreCollisions (bool ignoreBool)
	{
		Physics.IgnoreLayerCollision(12, 16, ignoreBool);
	}

	void resetAndExitMode() //This contains everything needed to revert back to standard mode.
	{
		mainCameraScript.birdsEyeActivated = false;
		cameraInPlace = false;
		IgnoreCollisions(false);
		controllerScript.soulMode = false;
		currentTargetedGhostNumber = 0;
		Time.timeScale = 1;
		Time.fixedDeltaTime = .02f;
		sphereCollider.enabled = false;
		this.GetComponent<ghostFollow> ().enabled = false;
	}

}
