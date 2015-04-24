using UnityEngine;
using System.Collections;

public class DestructibleGroup : MonoBehaviour {


	public Component[] allWoodGroups;





	// Use this for initialization
	void Start ()
	{
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		//allWoodGroups = spawnedTrap.GetComponentsInChildren <Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "ThrowableRock")
		{
			
			foreach (Rigidbody child in allWoodGroups)
			{
				child.rigidbody.constraints = RigidbodyConstraints.None;
				
				//child.transform.Translate(Vector3.down * speed * Time.deltaTime);
			}
			
		
		}
	}







}
