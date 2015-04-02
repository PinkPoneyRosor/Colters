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
	
	private NewThrowableRock firstScript;
	private NewThrowableRock secondScript;
	private NewThrowableRock thirdScript;
	private NewThrowableRock fourthScript;
	
	public GameObject[] allSelectedRocks = new GameObject[4];
	
	public GameObject RockPrefab;

	Transform mainCamera;
	
	private GameObject [] allRocks;
	private GameObject [] allSpawners;
	
	int selectedRockCount = 0;
	
	public LayerMask RockLayer;
	public LayerMask otherLayers;
	bool canThrow = true;
	
	public float globalPickUpRadius = 5;
	
	#region Selected Rocks Positions
	[HideInInspector]
	public Vector3 firstOffset;
	[HideInInspector]
	public Vector3 secondOffset;
	[HideInInspector]
	public Vector3 thirdOffset;
	[HideInInspector]
	public Vector3 fourthOffset;
	#endregion
	

	// Use this for initialization
	void Start () 
	{
		mainCamera = Camera.main.transform;
		
		selectedManagement();
	}
	
	// Update is called once per frame
	void Update () 
	{
		selectedManagement();
	
		firstOffset = Quaternion.AngleAxis(90, transform.up) * (-transform.forward * 1.5f) + (transform.up * 1.6f);
		secondOffset = Quaternion.AngleAxis(45, transform.up) * (-transform.forward * 1.5f) + (transform.up * 1.1f);
		thirdOffset = Quaternion.AngleAxis(0, transform.up) * (-transform.forward * 1.5f) + (transform.up * .6f);
		fourthOffset = Quaternion.AngleAxis(-45, transform.up) * (-transform.forward * 1.5f) + (transform.up * .1f);
		
		if(Input.GetButtonDown("SelectRock"))	
			aimlessControls();
		
		if(Input.GetAxisRaw("RT") != 0 || Input.GetButtonDown("Action"))
			ThrowRock();
		
		if (( Input.GetAxis("Scroll") > 0 || Input.GetButtonDown ("RockUp")) && canThrow)
			ManualScroll ();
		else if ((Input.GetAxis ("Scroll") < 0 || Input.GetButtonDown ("RockDown")) && canThrow)
			InvertedManualScroll ();
			
		RockGrow ();
	}
	
	void RockGrow ()
	{
		if (selectedRockCount < 4 )
		{
			Debug.Log ("Growing a rock...");
			
			if (firstSelected == null)
			{
				firstSelected = Instantiate ( RockPrefab, transform.position + firstOffset, Quaternion.identity ) as GameObject;
				selectedManagement();
				firstScript.isGrowing = true;
				Debug.Log ("Grown first");
			}
			else if (secondSelected == null)
			{
				secondSelected = Instantiate ( RockPrefab, transform.position + secondOffset, Quaternion.identity  ) as GameObject;
				selectedManagement();
				secondScript.isGrowing = true;
			}
			else if (thirdSelected == null)
			{
				thirdSelected = Instantiate ( RockPrefab, transform.position + thirdOffset, Quaternion.identity  ) as GameObject;
				selectedManagement();
				thirdScript.isGrowing = true;
			}
			else if (fourthSelected == null)
			{
				fourthSelected = Instantiate ( RockPrefab, transform.position + fourthOffset, Quaternion.identity  ) as GameObject;
				selectedManagement();
				fourthScript.isGrowing = true;
			}
			
			selectedRockCount++;
		}
	}
	
	void selectedManagement()
	{
		allSelectedRocks[0] = firstSelected;
		allSelectedRocks[1] = secondSelected;
		allSelectedRocks[2] = thirdSelected;
		allSelectedRocks[3] = fourthSelected;
		
		if (allSelectedRocks[0] != null)
		firstScript = allSelectedRocks[0].GetComponent <NewThrowableRock>();
		if (allSelectedRocks[1] != null)
		secondScript = allSelectedRocks[1].GetComponent <NewThrowableRock>();
		if (allSelectedRocks[2] != null)
		thirdScript = allSelectedRocks[2].GetComponent <NewThrowableRock>();
		if (allSelectedRocks[3] != null)
		fourthScript = allSelectedRocks[3].GetComponent <NewThrowableRock>();
	}
	
	void aimlessControls()
	{	
		allRocks = GameObject.FindGameObjectsWithTag ("ThrowableRock");
		
		foreach (GameObject rock in allRocks)
		{
			Vector3 fromPlayerToRock = transform.position - rock.transform.position;
			float distance = fromPlayerToRock.sqrMagnitude;
			
			if( distance < globalPickUpRadius)
				selectARock(rock);
		}
		
		
	}
	
	void selectARock (GameObject chosenRock)
	{
	
		NewThrowableRock chosenRockScript;
		chosenRockScript = chosenRock.GetComponent <NewThrowableRock>();
		
		if(selectedRockCount < 4 && // The player must have selected less than the maximum amount of rock allowed
		   chosenRock != null && //Let's make sure there isn't any mistake and that the player really got a rock
		   !chosenRockScript.isSelected && //The rock must not be already selected
		   !chosenRockScript.inTheAir && //The rock must not be already getting up
		   chosenRock.rigidbody.velocity.sqrMagnitude < 3 * 3)
		{
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
	}
	
	void ThrowRock()
	{
		RaycastHit HitObject;
		GameObject currentThrowedRock;
		NewThrowableRock currentThrowedRockScript;
		Ray ray;
		
			ray = mainCamera.camera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
		
		if (canThrow
			&& selectedRockCount > 0
			&& firstSelected != null
		    ) 
		{
			canThrow = false;	
			currentThrowedRock = firstSelected;
			currentThrowedRockScript = currentThrowedRock.GetComponent <NewThrowableRock> ();
			firstSelected = null;
				
			if (Physics.Raycast (ray.origin, ray.direction, out HitObject, Mathf.Infinity, otherLayers)
			    && HitObject.transform.CompareTag ("Enemy") && HitObject.transform.GetComponent<BasicEnemy> ().canGetHit)
			{ 
			  //If what we aimed at is an enemy and that it's not knocked out, let's do a homing attack
				currentThrowedRockScript.aimHoming = HitObject.transform;
				currentThrowedRockScript.homingAttackBool = true;
			}
			else 
			{
				//If what we aimed at is not an enemy or it is but he's knocked out, just throw the rock straightforward.
				Vector3 throwDirection = ray.direction;
				throwDirection.Normalize ();
				
				//This line is just to make absolutely sure there is no more constraints so that we can throw the rock in a straight line.
				currentThrowedRockScript.rigidbody.constraints = RigidbodyConstraints.None;
				
				currentThrowedRock.rigidbody.constantForce.force = throwDirection * currentThrowedRockScript.throwForce;
			}
				
				currentThrowedRockScript.isSelected = false;
				currentThrowedRockScript.inTheAir = false;
				currentThrowedRockScript.posAtLaunch = currentThrowedRock.transform.position;
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
		yield return new WaitForSeconds(1f);
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
		
		if(firstSelected != null)
		{
			tempFirst = firstSelected;
			firstSelected = null;
		}
		
		if(secondSelected != null)
		{
			firstSelected = secondSelected;
			secondSelected = null;
		}
		
		if(thirdSelected != null)
		{
			secondSelected = thirdSelected;
			thirdSelected = null;
		}
		
		if(fourthSelected != null)
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
}
