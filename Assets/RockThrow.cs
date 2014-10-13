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
	bool justHitInactiveRock = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	if (Input.GetButtonDown ("SelectRock")) 
		{
			Debug.DrawRay(Camera.position, Camera.forward*10, Color.red); 
			RaycastHit HitObject;
			if(Physics.SphereCast(Camera.position, .5f, Camera.forward, out HitObject , Mathf.Infinity, RockLayer))
			   	{
					Debug.Log ("hit "+HitObject.transform.name);
					Rock hitObjectScript;
					hitObjectScript = HitObject.transform.GetComponent<Rock>();

						if(!hitObjectScript.isSelected)
						{
							hitObjectScript.getUp = true;
							//hitObjectScript.isSelected = true;
							selectedRockCount++;
						}
				}
			else if(Physics.Raycast(Camera.position, Camera.forward, out HitObject , Mathf.Infinity, otherLayers))
			{
				if(selectedRockCount > 0)
				{
					selectedRocks = GameObject.FindGameObjectsWithTag("ThrowableRock")
							.Select(go => go.GetComponent<Rock>())
								.Where(go => go.isSelected)
									.Select(go => go.transform)
										.ToArray();

					foreach (Transform rock in selectedRocks)
					{
						Vector3 throwDirection = HitObject.point - rock.position;
						throwDirection.Normalize();

						Rock rockScript = rock.GetComponent<Rock>();
						rockScript.isSelected = false;

						rock.rigidbody.AddForce (throwDirection * 1000);
					}
				}
			}

		}
	}

}
