using UnityEngine;
using System.Collections;

public class RockShield : MonoBehaviour {


	GameObject spawnedShieldRock;

	public GameObject shieldRock;

	[HideInInspector]
	public bool shieldUp = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetAxis ("RT") > .5f)		
						spawnShield ();
				else
						destroyShield ();
	}

	void spawnShield()
	{
		if(GameObject.Find("rockShield")==null)
		{
			spawnedShieldRock = Instantiate (shieldRock, transform.position + transform.forward*2, transform.rotation) as GameObject;
			spawnedShieldRock.gameObject.name="rockShield";

			shieldUp = true;
		}
	}
	
	void destroyShield()
	{
		if(spawnedShieldRock!=null)
			Destroy(spawnedShieldRock.gameObject);

		shieldUp = false;
	}

}
