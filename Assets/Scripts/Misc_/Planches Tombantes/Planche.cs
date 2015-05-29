using UnityEngine;
using System.Collections;

public class Planche : MonoBehaviour {

	private Renfort renfort;
	private Renfort2 renfort2;

	public bool alive = true;
	public bool aliveToo = true;

	// Use this for initialization
	void Start () 
	{
		renfort = GetComponent<Renfort> ();
		renfort2 = GetComponent<Renfort2> ();
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(alive == false && aliveToo == false)
		{
			rigidbody.constraints = RigidbodyConstraints.None;
			Debug.Log ("Bon, ça marche !");
		}
	}
	
	
	

}