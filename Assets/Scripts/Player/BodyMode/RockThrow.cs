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
	
	public Vector3 setFirstRockPos = Vector3.zero;
	public Vector3 setSecondRockPos = Vector3.zero;
	public Vector3 setThirdRockPos = Vector3.zero;
	public Vector3 setFourthRockPos = Vector3.zero;
	
	[HideInInspector]
	public GameObject firstSelected;
	[HideInInspector]
	public GameObject secondSelected;
	[HideInInspector]
	public GameObject thirdSelected;
	[HideInInspector]
	public GameObject fourthSelected;
	
	ThrowableRock firstRockScript;

	void Start () 
	{
		mainCamera = Camera.main.transform;
		allRocks = GameObject.FindGameObjectsWithTag ("ThrowableRock");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (firstSelected != null)
			firstRockScript = firstSelected.GetComponent < ThrowableRock > ();
					
		if(Input.GetButtonDown("SelectRock"))
		{
			//everything in this script happens when the player is hitting the selectRock Button
			if (CommonControls.aimingMode)  
				controlsWhileAiming();
			else
				aimlessControls();
		}
		
		if ( Input.GetAxis("Scroll") > 0)
			ManualScroll ();
	}
		
		IEnumerator ShiftRockPositions() 
		{
			yield return new WaitForSeconds(.5f);
			canThrow = true;
			
			if (secondSelected != null)
			{
				firstSelected = secondSelected;
				secondSelected = null;
				ThrowableRock rockScript = firstSelected.GetComponent<ThrowableRock>();
				rockScript.selectionNumber = 1;
			}
			
			if (thirdSelected != null)
			{
				secondSelected = thirdSelected;
				thirdSelected = null;
				ThrowableRock rockScript = secondSelected.GetComponent<ThrowableRock>();
				rockScript.selectionNumber = 2;
			}
			
			if (fourthSelected != null)
			{
				thirdSelected = fourthSelected;
				fourthSelected = null;
				ThrowableRock rockScript = thirdSelected.GetComponent<ThrowableRock>();
				rockScript.selectionNumber = 3;
			}
		}
		
		void ManualScroll ()
		{
			Debug.Log ("SCROLL SCROLL MOTHERFUCKER");
			
			GameObject tempFourth = null;
			
			ThrowableRock rockScript;
			
			tempFourth = firstSelected;

			firstSelected = secondSelected;
			rockScript = firstSelected.GetComponent<ThrowableRock>();
			rockScript.selectionNumber = 1;
			
			secondSelected = thirdSelected;
			rockScript = secondSelected.GetComponent<ThrowableRock>();
			rockScript.selectionNumber = 2;
		
			thirdSelected = fourthSelected;
			rockScript = thirdSelected.GetComponent<ThrowableRock>();
			rockScript.selectionNumber = 3;
		
			fourthSelected = tempFourth;
			rockScript = fourthSelected.GetComponent<ThrowableRock>();
			rockScript.selectionNumber = 4;
		
			/*if(firstSelected != null)
			{
				tempFourth = firstSelected;
				firstSelected = null;
			}
		
			if (secondSelected != null)
			{
				firstSelected = secondSelected;
				secondSelected = null;
				ThrowableRock rockScript = firstSelected.GetComponent<ThrowableRock>();
				rockScript.selectionNumber = 1;
			}
			
			if (thirdSelected != null)
			{
				secondSelected = thirdSelected;
				thirdSelected = null;
				ThrowableRock rockScript = secondSelected.GetComponent<ThrowableRock>();
				rockScript.selectionNumber = 2;
			}
			
			if (fourthSelected != null)
			{
				thirdSelected = fourthSelected;
				fourthSelected = null;
				
				if ( tempFourth != null )
				{
				fourthSelected = tempFourth;
				ThrowableRock fourthRockScript = fourthSelected.GetComponent<ThrowableRock>();
				fourthRockScript.selectionNumber = 4;
				}
				
				ThrowableRock rockScript = thirdSelected.GetComponent<ThrowableRock>();
				rockScript.selectionNumber = 3;
			}*/
		}
		
		void controlsWhileAiming ()
		{
				//First, we check with a sphereCast (in order to allow the player to be less precise) if the player is looking at a rock.
				RaycastHit HitObject;
				if (Physics.SphereCast (transform.position, .5f, mainCamera.forward, out HitObject, Mathf.Infinity, RockLayer)) 
				{
					selectARock(HitObject.collider.gameObject);
				}
				//Then, if the player is looking at anything that is not a selectable rock...
				else if (canThrow 
				         && firstRockScript.nowThrowable 
				         && Physics.Raycast (mainCamera.position, mainCamera.forward, out HitObject, Mathf.Infinity, otherLayers)) 
				{
					currentHitPoint = HitObject.point;
					//While at least a rock is selected....
					if (selectedRockCount > 0) 
					{
						if (HitObject.transform.CompareTag ("Enemy") && HitObject.transform.GetComponent<BasicEnemy> ().canGetHit) 
						{  //If what we aimed at is an enemy and that it's not knocked out, let's do a homing attack
							firstRockScript.aimHoming = HitObject.transform;
							firstRockScript.homingAttackBool = true;
						}
						else
						{
							//If what we aimed at is not an enemy or it is but he's knocked out, just throw the rock straightforward.
							Vector3 throwDirection = HitObject.point - firstSelected.transform.position;
							throwDirection.Normalize ();
							
							//This line is just to make absolutely sure there is no more constraints so that we can throw the rock in a straight line.
							firstRockScript.rigidbody.constraints = RigidbodyConstraints.None;
							
							firstSelected.rigidbody.constantForce.force = throwDirection * firstRockScript.throwForce;
						}
						
						firstRockScript.isSelected = false;
						firstRockScript.selectionNumber = 0;
						selectedRockCount -= 1;
						firstSelected.rigidbody.isKinematic = false;
						
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
		chosenRockScript = chosenRock.transform.GetComponent<ThrowableRock> ();
		
		if ( selectedRockCount < maxRockCount && // The player must have selected less than the maximum amount of rock allowed
		    !chosenRockScript.isSelected && //The rock must not be already selected
		    !chosenRockScript.getUpInit && //The rock must not be preparing to get up
		    !chosenRockScript.gettingUp && //The rock must not be already getting up
		    chosenRock.rigidbody.velocity.sqrMagnitude < 3 * 3) //The rock must not be moving too fast (I.E. When it just launched)
		{
			chosenRockScript.getUpInit = true;
			selectedRockCount++;
			chosenRockScript.selectionNumber = selectedRockCount;
			chosenRock.rigidbody.isKinematic = true;
			
			switch (selectedRockCount)
			{
			case 1:
				firstSelected = chosenRock.transform.gameObject;
				break;
			case 2:
				secondSelected = chosenRock.transform.gameObject;
				break;
			case 3:
				thirdSelected = chosenRock.transform.gameObject;
				break;
			case 4:
				fourthSelected = chosenRock.transform.gameObject;
				break;
			}
		}
	}
	
} //END OF CLASS
