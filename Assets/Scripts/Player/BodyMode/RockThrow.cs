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

	void Start () {
		mainCamera = Camera.main.transform;
		//controlsScript = CommonControls.GetType ("CommonControls");
		allRocks = GameObject.FindGameObjectsWithTag ("ThrowableRock");
	}
	
	// Update is called once per frame
	void Update () 
	{
					
		if(Input.GetButtonDown("SelectRock"))
		{
			/*Vector3 distances = setFirstRockPos - transform.position;
			Vector3 relativePosition = Vector3.zero;
			relativePosition.x = Vector3.Dot(distances, transform.right.normalized);
			relativePosition.y = Vector3.Dot(distances, transform.up.normalized);
			relativePosition.z = Vector3.Dot(distances, transform.forward.normalized);*/
			
			//everything in this script happens when the player is hitting the selectRock Button
			if (CommonControls.aimingMode) 
			{ 
				//First, we check with a sphereCast (in order to allow the player to be less precise) if the player is looking at a rock.
						RaycastHit HitObject;
						if (Physics.SphereCast (transform.position, .5f, mainCamera.forward, out HitObject, Mathf.Infinity, RockLayer)) 
						{
								ThrowableRock hitObjectScript;
								hitObjectScript = HitObject.transform.GetComponent<ThrowableRock> ();

								if (!hitObjectScript.isSelected && selectedRockCount < maxRockCount) 
								{
										hitObjectScript.getUpInit = true;
										//HitObject.transform.SetParent(this.transform, true);
										selectedRockCount++;
										hitObjectScript.selectionNumber = selectedRockCount;
										
										switch (selectedRockCount)
										{
										case 1:
											firstSelected = HitObject.transform.gameObject;
											break;
										case 2:
											secondSelected = HitObject.transform.gameObject;
											break;
										case 3:
											thirdSelected = HitObject.transform.gameObject;
											break;
										case 4:
											fourthSelected = HitObject.transform.gameObject;
											break;
										}
								}
						}
						//Then, if the player is looking at anything that is not a selectable rock...
						else if ( canThrow && Physics.Raycast (mainCamera.position, mainCamera.forward, out HitObject, Mathf.Infinity, otherLayers)) 
						{
							currentHitPoint = HitObject.point;
							//While at least a rock is selected....
							if (selectedRockCount > 0) 
							{
								//firstSelected.transform.parent = null;
								ThrowableRock firstRockScript = firstSelected.GetComponent<ThrowableRock> ();

										if (!HitObject.transform.CompareTag ("Enemy")) 
										{ 
										//If what we aimed at is not an enemy, just throw it.
												Vector3 throwDirection = HitObject.point - firstSelected.transform.position;
												throwDirection.Normalize ();
												
												//This line is just to make absolutely sure there is no more constraints so that we can throw the rock in a straight line.
												firstRockScript.rigidbody.constraints = RigidbodyConstraints.None;

												firstSelected.rigidbody.constantForce.force = throwDirection * firstRockScript.throwForce;
										}
										else if (HitObject.transform.GetComponent<BasicEnemy> ().canGetHit) 
										{ //But if it's an enemy and that it's not knocked out, let's do a homing attack
												firstRockScript.aimHoming = HitObject.transform;
												firstRockScript.homingAttackBool = true;
										}
										
										firstRockScript.isSelected = false;
										firstRockScript.selectionNumber = 0;
										selectedRockCount -= 1;
										firstSelected.rigidbody.isKinematic = false;
										
										canThrow = false;
										
										StartCoroutine("ShiftRockPositions"); //This method is also used as a coolDown for throwing rocks.
								}
							}
				#region Not Aiming !
				} 
				else
				{ //If not in aiming Mode
					foreach (GameObject rock in allRocks)
					{
						Vector3 fromPlayerToRock = transform.position - rock.transform.position;
						float distance = fromPlayerToRock.sqrMagnitude;

						if( distance < globalPickupRadius * globalPickupRadius && selectedRockCount < maxRockCount)
						{
							ThrowableRock rockScript = rock.GetComponent<ThrowableRock>();
							rockScript.getUpInit = true;
							//rock.transform.SetParent(this.transform, true);
							selectedRockCount++;
							rockScript.selectionNumber = selectedRockCount;
							rock.rigidbody.isKinematic = true;
							
							switch (selectedRockCount)
							{
								case 1:
									firstSelected = rock;
									break;
								case 2:
									secondSelected = rock;
									break;
								case 3:
									thirdSelected = rock;
									break;
								case 4:
									fourthSelected = rock;
									break;
							}
							
						}
					}
				}
				#endregion
		}
		}
		
		IEnumerator ShiftRockPositions() {
			yield return new WaitForSeconds(.5f);
			canThrow = true;
			if(secondSelected != null)
			{
				firstSelected = secondSelected;
				secondSelected = null;
				ThrowableRock rockScript = firstSelected.GetComponent<ThrowableRock>();
				rockScript.selectionNumber = 1;
				Debug.Log ("Second one has become first one");
			}
			if(thirdSelected != null)
			{
				secondSelected = thirdSelected;
				thirdSelected = null;
				ThrowableRock rockScript = secondSelected.GetComponent<ThrowableRock>();
				rockScript.selectionNumber = 2;
				Debug.Log ("Third one has become second one");
			}
			if(fourthSelected != null)
			{
				thirdSelected = fourthSelected;
				fourthSelected = null;
				ThrowableRock rockScript = thirdSelected.GetComponent<ThrowableRock>();
				rockScript.selectionNumber = 3;
				Debug.Log ("Fourth one has become third one");
			}
		}
} //END OF CLASS
