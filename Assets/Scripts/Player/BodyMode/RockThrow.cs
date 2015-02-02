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
			
			GameObject tempFirst = null;
			
			ThrowableRock rockScript;
			
			if(firstSelected != null && maxRockCount >= 1)
			{
				tempFirst = firstSelected;
				firstSelected = null;
			}

		if(secondSelected != null && maxRockCount >= 2)
			{
			firstSelected = secondSelected;
			secondSelected = null;
			rockScript = firstSelected.GetComponent <ThrowableRock>();
			rockScript.selectionNumber = 1;
			Debug.Log ("Second became first");
			}
			
		if(thirdSelected != null && maxRockCount >= 3)
			{
			secondSelected = thirdSelected;
			thirdSelected = null;
			rockScript = secondSelected.GetComponent <ThrowableRock>();
			rockScript.selectionNumber = 2;
			Debug.Log ("Third became second");
			}
		
		if(fourthSelected != null && maxRockCount == 4)
			{
			thirdSelected = fourthSelected;
			fourthSelected = null;
			rockScript = thirdSelected.GetComponent <ThrowableRock>();
			rockScript.selectionNumber = 3;
			Debug.Log ("Fourth became third");
			}
			
			if(tempFirst != null)
			{
				if(selectedRockCount == 4)
				{
					fourthSelected = tempFirst;
					rockScript = fourthSelected.GetComponent <ThrowableRock>();
					rockScript.selectionNumber = 4;
				}
				else if (selectedRockCount == 3)
				{
					thirdSelected = tempFirst;
					rockScript = thirdSelected.GetComponent <ThrowableRock>();
					rockScript.selectionNumber = 3;
				}
				else if (selectedRockCount == 2)
				{
					secondSelected = tempFirst;
					rockScript = secondSelected.GetComponent <ThrowableRock>();
					rockScript.selectionNumber = 2;
				}
				else if (selectedRockCount == 1)
				{
					firstSelected = tempFirst;
				}
				
				tempFirst = null;
	
				Debug.Log ("First became whatever");
			}
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
			chosenRock.rigidbody.isKinematic = true;
				
			if (firstSelected == null)
			{
				firstSelected = chosenRock.transform.gameObject;
				chosenRockScript.selectionNumber = 1;
			}
			else if (secondSelected == null)
			{
				secondSelected = chosenRock.transform.gameObject;
				chosenRockScript.selectionNumber = 2;
			}
			else if (thirdSelected == null)
			{
				thirdSelected = chosenRock.transform.gameObject;
				chosenRockScript.selectionNumber = 3;
			}
			else if (fourthSelected == null)
			{
				fourthSelected = chosenRock.transform.gameObject;
				chosenRockScript.selectionNumber = 4;
			}
		}
	}
	
} //END OF CLASS
