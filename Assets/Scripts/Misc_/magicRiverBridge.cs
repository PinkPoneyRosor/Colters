using UnityEngine;
using System.Collections;

public class magicRiverBridge : MonoBehaviour {

	public float holdTime;

	// Use this for initialization
	void Start () 
	{
		animation["RiverBridgeAnim"].time = 0.1f;
		animation["RiverBridgeAnim"].speed = 0.0f;
		animation.Play("RiverBridgeAnim");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision hit)
	{
		if(hit.collider.CompareTag("ThrowableRock"))
		{
			animation.Play("RiverBridgeAnim");
			animation["RiverBridgeAnim"].speed = 1;
		}
	}
	
	public void HoldAnimation ()
	{
		StartCoroutine("HoldIt");
	}
	
 	IEnumerator HoldIt ()
 	{
		animation["RiverBridgeAnim"].speed = 0;
		Debug.Log ("Called");
		yield return new WaitForSeconds (holdTime /* Time.deltaTime*/);
		animation["RiverBridgeAnim"].speed = 1;
		Debug.Log ("Waited");
 	}
 	
 	public void Stop ()
 	{
 		animation["RiverBridgeAnim"].speed = 0;
 	}
}
