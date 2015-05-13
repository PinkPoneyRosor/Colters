using UnityEngine;
using System.Collections;

public class WaterVolume : MonoBehaviour {

	public float waterStreamSpeed;
	public Vector3 streamDirection;
	public bool KillPlayer = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter (Collider hit)
	{
		if (hit.CompareTag ("Player"))
		{
			hit.SendMessage ("Die", SendMessageOptions.DontRequireReceiver);
		}
	}
}
