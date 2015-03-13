using UnityEngine;
using System.Collections;
using System.Linq;

public class RockThrow : MonoBehaviour {

	#region external scripts and objects
	Transform mainCamera;
	GameObject SelectedRock;
	Transform[] selectedRocks;
	#endregion

	#region inspector vars
	public LayerMask RockLayer;
	public LayerMask otherLayers;
	public float globalPickupRadius = 10;
	public int maxRockCount = 4;
	#endregion

	[HideInInspector]
	public int selectedRockCount = 0;

	public Vector3 currentHitPoint;
	// Use this for initialization

	private GameObject [] allRocks;
	CommonControls controlsScript;
	
	private bool canThrow = true;
	
	[HideInInspector]
	public GameObject firstSelected = null;
	[HideInInspector]
	public GameObject secondSelected = null;
	[HideInInspector]
	public GameObject thirdSelected = null;
	[HideInInspector]
	public GameObject fourthSelected = null;
	
	ThrowableRock firstRockScript;

	void Start () 
	{
		mainCamera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(firstSelected != null)
		Debug.Log (firstSelected.name);
		
		if(secondSelected != null)
		Debug.Log (secondSelected.name);
		
		if(thirdSelected != null)
		Debug.Log (thirdSelected.name);
		
		if(fourthSelected != null)
		Debug.Log (fourthSelected.name);
	
		if (firstSelected != null)
			firstRockScript = firstSelected.GetComponent < ThrowableRock > ();
				
		ThrowableRock anyRockScript;
		
		#region set number at all times
		if (firstSelected != null)
		{
			anyRockScript = firstSelected.GetComponent <ThrowableRock>();
			anyRockScript.selectionNumber = 1;
		}
		if (secondSelected != null)
		{
			anyRockScript = secondSelected.GetComponent <ThrowableRock>();
			anyRockScript.selectionNumber = 2;
		}
		if (thirdSelected != null)
		{
			anyRockScript = thirdSelected.GetComponent <ThrowableRock>();
			anyRockScript.selectionNumber = 3;
		}
		if (fourthSelected != null)
		{
			anyRockScript = fourthSelected.GetComponent <ThrowableRock>();
			anyRockScript.selectionNumber = 4;
		}
		#endregion
					
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
		
		if ( Input.GetAxis("Scroll") > 0)
			ManualScroll ();
		else if (Input.GetAxis ("Scroll") < 0)
			InvertedManualScroll ();
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
		
		void ManualScroll ()
		{
			GameObject tempFirst = null;
			
			if(firstSelected != null && maxRockCount >= 1)
			{
					tempFirst = firstSelected;
					firstSelected = null;
			}

		if(secondSelected != null && maxRockCount >= 2)
			{
					firstSelected = secondSelected;
					secondSelected = null;
			}
			
		if(thirdSelected != null && maxRockCount >= 3)
			{
					secondSelected = thirdSelected;
					thirdSelected = null;
			}
		
		if(fourthSelected != null && maxRockCount == 4)
			{
					thirdSelected = fourthSelected;
					fourthSelected = null;
			}
			
			if(tempFirst != null)
			{
					if(selectedRockCount == 4)
					{
						fourthSelected = tempFirst;
					}
					else if (selectedRockCount == 3)
					{
						thirdSelected = tempFirst;
					}
					else if (selectedRockCount == 2)
					{
						secondSelected = tempFirst;
					}
					else if (selectedRockCount == 1)
					{
						firstSelected = tempFirst;
					}
				
				tempFirst = null;
			}
		}
		
		void InvertedManualScroll()
		{
			GameObject tempPlace = null;
		
			if (selectedRockCount == 2)
			{
				tempPlace = secondSelected;
				
				secondSelected = firstSelected;
				firstSelected = tempPlace;
				
				tempPlace = null;
			}
			
			if(selectedRockCount == 3)
			{
				tempPlace = thirdSelected;
				
				thirdSelected = secondSelected;
				secondSelected = firstSelected;
				firstSelected = tempPlace;
				
				tempPlace = null;
			}
			
			if(selectedRockCount == 4)
			{
				tempPlace = fourthSelected;
				
				fourthSelected = thirdSelected;
				thirdSelected = secondSelected;
				secondSelected = firstSelected;
				firstSelected = tempPlace;
				
				tempPlace = null;
			}
			
		}
		
	void controlsWhileAiming ()
	{
			//First, we check with a sphereCast (in order to allow the player to be less precise) if the player is looking at a rock.
			RaycastHit HitObject;
			if (Physics.SphereCast (transform.position, .2f, mainCamera.forward, out HitObject, Mathf.Infinity, RockLayer)) 
			{
				selectARock(HitObject.collider.gameObject);
			}
	}
	
	void ThrowRock()
	{
		//Then, if the player is looking at anything that is not a selectable rock...
		RaycastHit HitObject;
		GameObject currentThrowedRock;
		ThrowableRock currentThrowedRockScript;
		
		if (canThrow
		    && selectedRockCount > 0
		    && firstRockScript.nowThrowable 
		    && Physics.Raycast (mainCamera.position, mainCamera.forward, out HitObject, Mathf.Infinity, otherLayers)) 
		{
			currentHitPoint = HitObject.point;
			currentThrowedRock = firstSelected;
			currentThrowedRockScript = currentThrowedRock.GetComponent <ThrowableRock>();
			
			firstSelected = null;
			
			//While at least a rock is selected....
			if (selectedRockCount > 0) 
			{
				
			
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
				
				canThrow = false;
				
				StartCoroutine("ShiftRockPositions"); //This method is also used as a coolDown for throwing rocks.
			}
		}
	}
	
	void aimlessControls()
	{
		foreach (GameObject rock in allRocks)
		{
			Vector3 fromPlayerToRock = transform.position - rock.transform.position;
			float distance = fromPlayerToRock.sqrMagnitude;
			
			if( distance < globalPickupRadius)
				selectARock(rock);
		}
	}
		
	void selectARock (GameObject chosenRock)
	{	
		ThrowableRock chosenRockScript;
		chosenRockScript = chosenRock.transform.GetComponent < ThrowableRock > ();
		
		if ( selectedRockCount < maxRockCount && // The player must have selected less than the maximum amount of rock allowed
			chosenRock != null &&
		    !chosenRockScript.isSelected && //The rock must not be already selected
		    !chosenRockScript.getUpInit && //The rock must not be preparing to get up
		    !chosenRockScript.gettingUp && //The rock must not be already getting up
		    chosenRock.rigidbody.velocity.sqrMagnitude < 3 * 3 ) //The rock must not be moving too fast (I.E. When it just launched)
		{
			selectedRockCount++;
			chosenRockScript.getUpInit = true;
			chosenRock.collider.isTrigger = true;
			chosenRock.rigidbody.isKinematic = true;
				
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
		}
		
		//Debug Section
		if ( chosenRock.rigidbody.velocity.sqrMagnitude > 3 * 3 )
			Debug.Log("Selected Rock is moving too fast!");
		if (chosenRockScript.gettingUp)
			Debug.Log ("Selected Rock is getting up!");
		if (chosenRockScript.getUpInit)
			Debug.Log ("Selected Rock is preparing to get up!");
		if (chosenRockScript.getUpInit)
			Debug.Log ("Selected Rock is preparing to get up!");
		if (chosenRockScript.getUpInit)
			Debug.Log ("Selected Rock is already in selection!");
	}
	
} //END OF CLASS
