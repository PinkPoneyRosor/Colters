using UnityEngine;
using System.Collections;

public class NewRockThrow : MonoBehaviour {

	[HideInInspector]
	public GameObject firstSelected = null;
	[HideInInspector]
	public GameObject secondSelected = null;
	[HideInInspector]
	public GameObject thirdSelected = null;
	[HideInInspector]
	public GameObject fourthSelected = null;

	Transform mainCamera;
	
	private GameObject [] allRocks;
	
	int selectedRockCount = 0;
	
	public LayerMask RockLayer;
	public LayerMask otherLayers;
	bool canThrow = true;
	

	// Use this for initialization
	void Start () 
	{
		mainCamera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetButtonDown("SelectRock"))
		{		
			allRocks = GameObject.FindGameObjectsWithTag ("ThrowableRock");
			
			if (CommonControls.aimingMode)  
				controlsWhileAiming();
			else
				aimlessControls();
		}
		
		if(Input.GetAxisRaw("RT") != 0)
		{
			ThrowRock();
		}
	
	}
	
	void controlsWhileAiming()
	{
		//First, we check with a sphereCast (in order to allow the player to be less precise) if the player is looking at a rock.
		RaycastHit HitObject;
		if (Physics.SphereCast (transform.position, .2f, mainCamera.forward, out HitObject, Mathf.Infinity, RockLayer)) 
		{
			selectARock(HitObject.collider.gameObject);
		}
	}
	
	void aimlessControls()
	{
		
	}
	
	void selectARock (GameObject chosenRock)
	{
	
		NewThrowableRock chosenRockScript;
		chosenRockScript = chosenRock.GetComponent <NewThrowableRock>();
		
		if (firstSelected == null)
		{
			firstSelected = chosenRock.transform.gameObject;
		}
		else if (secondSelected == null)
		{
			secondSelected = chosenRock.transform.gameObject;
		}
		else if (thirdSelected == null)
		{
			thirdSelected = chosenRock.transform.gameObject;
		}
		else if (fourthSelected == null)
		{
			fourthSelected = chosenRock.transform.gameObject;
		}
		
		chosenRockScript.isSelected = true;
		selectedRockCount++;
	}
	
	void ThrowRock()
	{
		RaycastHit HitObject;
		GameObject currentThrowedRock;
		NewThrowableRock currentThrowedRockScript;
		
		if (canThrow
			&& selectedRockCount > 0
		    && Physics.Raycast (mainCamera.position, mainCamera.forward, out HitObject, Mathf.Infinity, otherLayers)) 
		{
			currentThrowedRock = firstSelected;
			currentThrowedRockScript = currentThrowedRock.GetComponent <NewThrowableRock>();
			firstSelected = null;
				
				if (HitObject.transform.CompareTag ("Enemy") && HitObject.transform.GetComponent<BasicEnemy> ().canGetHit) 
				{  //If what we aimed at is an enemy and that it's not knocked out, let's do a homing attack
					currentThrowedRockScript.aimHoming = HitObject.transform;
					currentThrowedRockScript.homingAttackBool = true;
				}
				else
				{
					//If what we aimed at is not an enemy or it is but he's knocked out, just throw the rock straightforward.
					Vector3 throwDirection = HitObject.point - currentThrowedRock.transform.position;
					throwDirection.Normalize ();
					
					//This line is just to make absolutely sure there is no more constraints so that we can throw the rock in a straight line.
					currentThrowedRockScript.rigidbody.constraints = RigidbodyConstraints.None;
					
					currentThrowedRock.rigidbody.constantForce.force = throwDirection * currentThrowedRockScript.throwForce;
				}
				
				currentThrowedRockScript.isSelected = false;
				currentThrowedRockScript.selectionNumber = 0;
				currentThrowedRock.rigidbody.isKinematic = false;
				currentThrowedRock.collider.isTrigger = false;
				currentThrowedRock = null;
				selectedRockCount -= 1;
				
				StartCoroutine("ShiftRockPositions"); //This method is also used as a coolDown for throwing rocks.
		}
	}
	
	IEnumerator ShiftRockPositions() 
	{
		yield return new WaitForSeconds(.5f);
		canThrow = true;
		
		if (secondSelected != null)
		{
			firstSelected = secondSelected;
			secondSelected = null;
		}
		
		if (thirdSelected != null)
		{
			secondSelected = thirdSelected;
			thirdSelected = null;
		}
		
		if (fourthSelected != null)
		{
			thirdSelected = fourthSelected;
			fourthSelected = null;
		}
	}	
}
