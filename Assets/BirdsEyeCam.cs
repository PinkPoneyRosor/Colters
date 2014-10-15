using UnityEngine;
using System.Collections;

public class BirdsEyeCam : MonoBehaviour {
	
	Transform soul;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
			soul = GameObject.Find ("Soul").transform;
			this.transform.position = soul.position + new Vector3 (0,10,0) + soul.transform.forward * -5;
			this.transform.LookAt (soul.transform.position);
	}
}
