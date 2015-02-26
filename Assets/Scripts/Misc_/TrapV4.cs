using UnityEngine;
using System.Collections;

public class TrapV4 : MonoBehaviour {


	public AnimationClip animation;

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
			GameObject.Find("TrapV4").animation.Play("StatuePlateforme");
			Debug.Log("Play!");
		}

}
}
