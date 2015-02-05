using UnityEngine;
using System.Collections;

public class particleSuicide : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate () 
	{
		if (!particleSystem.IsAlive())
			Object.Destroy (this.gameObject);	
	}
}
