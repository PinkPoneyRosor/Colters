using UnityEngine;
using System.Collections;
using System.Linq;

public class RockThrow : MonoBehaviour {

	public Transform Camera;
	GameObject SelectedRock;
	public LayerMask RockLayer;
	public LayerMask otherLayers;
	public int selectedRockCount = 0;
	Transform[] selectedRocks;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	if (Input.GetButtonDown ("SelectRock")) 
		{
			//First, we check with a sphereCast (in order to allow the player to be less precise) if the player is looking at a rock.
			RaycastHit HitObject;
			if(Physics.SphereCast(Camera.position, .5f, Camera.forward, out HitObject , Mathf.Infinity, RockLayer))
			{
				Rock hitObjectScript;
				hitObjectScript = HitObject.transform.GetComponent<Rock>();

				if(!hitObjectScript.isSelected)
				{
					hitObjectScript.getUp = true;
					selectedRockCount++;
				}
			}
			//Then, if the player is looking at anything that is not a selectable rock...
			else if(Physics.Raycast(Camera.position, Camera.forward, out HitObject , Mathf.Infinity, otherLayers))
			{
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
						Vector3 throwDirection = HitObject.point - rock.position;
						throwDirection.Normalize();

						Rock rockScript = rock.GetComponent<Rock>();
						rockScript.isSelected = false;
						//This line is just to make absolutely sure there is no more constraints so that we can throw the rock.
						rockScript.rigidbody.constraints = RigidbodyConstraints.None;

						rock.rigidbody.AddForce (throwDirection * 1000);
					}
				}
			}
		}
	}

}
