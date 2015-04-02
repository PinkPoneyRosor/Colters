using UnityEngine;
using System.Collections;

public class RockSpawner : MonoBehaviour {

	public GameObject RockPrefab;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void spawnARock ()
	{
		Instantiate (RockPrefab, this.transform.position - (transform.up * -2), Quaternion.identity);
	}
}
