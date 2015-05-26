using UnityEngine;
using System.Collections;

public class WaterVolume : MonoBehaviour {

	public float waterStreamSpeed;
	public Vector3 streamDirection;
	public bool KillPlayer = false;
	public GameObject splashEffect;
	public GameObject cubeSplashEffect;
	public Vector3 splashPoint;
	private float waterHeight;
	public 	float splashRate = 2f;
	private float nextSplash = 0;
	


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter (Collider hit)
	{
		if (hit.CompareTag ("Player") || hit.CompareTag ("ThrowableRock"))
		{
			Instantiate(splashEffect,hit.transform.position, hit.transform.rotation);
			hit.SendMessage ("Die", SendMessageOptions.DontRequireReceiver);
		}


	}

	void OnTriggerStay (Collider hit)
	{
		if (hit.CompareTag ("MovingPlatform") && Time.time > nextSplash)
		{
			nextSplash = Time.time + splashRate;
		
			Instantiate(cubeSplashEffect,hit.transform.position, hit.transform.rotation);

		}
	}
}

