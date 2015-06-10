using UnityEngine;
using System.Collections;

public class GrotteExit : MonoBehaviour {
	
	public AudioSource grind;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	
	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "ThrowableRock")
		{
			this.animation.Play("GrotteSortie");
			Debug.Log("Play!");
			grind.Play();
		}
	}
}
