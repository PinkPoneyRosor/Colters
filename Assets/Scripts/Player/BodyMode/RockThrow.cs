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
	#endregion

	[HideInInspector]
	public int selectedRockCount = 0;

	public Vector3 currentHitPoint;
	// Use this for initialization
	void Start () {
		mainCamera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.GetButtonDown ("SelectRock")) //everything in this script happens when the player is hitting the selectRock Button.
			{
				//First, we check with a sphereCast (in order to allow the player to be less precise) if the player is looking at a rock.
				RaycastHit HitObject;
				if(Physics.SphereCast(transform.position, .5f, mainCamera.forward, out HitObject , Mathf.Infinity, RockLayer))
				{
					Rock hitObjectScript;
					hitObjectScript = HitObject.transform.GetComponent<Rock>();

					if(!hitObjectScript.isSelected)
					{
						hitObjectScript.getUpInit = true;
						selectedRockCount++;
					}
				}
				//Then, if the player is looking at anything that is not a selectable rock...
				else if(Physics.Raycast(mainCamera.position, mainCamera.forward, out HitObject , Mathf.Infinity, otherLayers))
				{
				currentHitPoint = HitObject.point;
					//While at least a rock is selected....
					if(selectedRockCount > 0)
					{
						//Let's put all selected rocks in a single array
						selectedRocks = GameObject.FindGameObjectsWithTag("ThrowableRock")
								.Select(go => go.GetComponent<Rock>())
									.Where(go => go.isSelected)
										.Select(go => go.transform)
											.ToArray();

						//Then let's throw them in the direction the player is looking.
						foreach (Transform rock in selectedRocks)
						{
							Rock rockScript = rock.GetComponent<Rock>();

							if(!HitObject.transform.CompareTag("Enemy")) 
							//If what we aimed at is not an enemy, just throw it.
							{
								Vector3 throwDirection = HitObject.point - rock.position;
								throwDirection.Normalize();

								rockScript.isSelected = false;
								//This line is just to make absolutely sure there is no more constraints so that we can throw the rock in a straight line.
								rockScript.rigidbody.constraints = RigidbodyConstraints.None;

								rock.rigidbody.constantForce.force = throwDirection * rockScript.throwForce;
							}
							else if (HitObject.transform.GetComponent<BasicEnemy>().canGetHit) //But if it's an enemy and that it's not knocked out, let's do a homing attack
							{
								rockScript.aimHoming = HitObject.transform;
								rockScript.homingAttackBool = true;
							}
						}
					}
				}
			}
	}

} //END OF CLASS
