using UnityEngine;
using System.Collections;

public class Credit : MonoBehaviour {
	
	public GameObject image;
	public float maxY = 70f;
	private Vector3 startPos;
	private bool creditRoll = false;

	
	// Use this for initialization
	void Start () 
	{
		startPos = image.transform.position;
		Debug.Log ("START");
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.Log (Screen.height);

		Vector3 pos = image.transform.position;
		
		if (pos.y < maxY && creditRoll) 
		{
			pos.y += .5f * Screen.height * Time.deltaTime;
			image.transform.position = pos;
		} 
		else if (creditRoll && pos.y > Screen.height)
		{
			Application.LoadLevel(0);
			creditRoll = false;
		}
			
	}
	
	public void StartCredits ()
	{
		creditRoll = true;
	}
	
	public void resetCredits()
	{
		image.transform.position = startPos;
	}
	
}
