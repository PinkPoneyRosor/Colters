using UnityEngine;
using System.Collections;

public class Archer_Sight : MonoBehaviour {

	Enemy_Archer archerScript;

	// Use this for initialization
	void Start () 
	{
		archerScript = this.GetComponentInParent <Enemy_Archer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			archerScript.nextFireTime = Time.time + (archerScript.reloadTime * 1);
			archerScript.myTarget = other.gameObject;
		}
	}
}
