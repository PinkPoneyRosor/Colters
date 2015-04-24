using UnityEngine;
using System.Collections;

public class Grue : MonoBehaviour 
{
	public bool activated;



	// Use this for initialization
	void Start () 
	{
		activated = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}



	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player" && !activated)
		{
			this.animation.Play("Grue");
			Debug.Log("La Grue !");
			activated = true;
		}
	}

}
