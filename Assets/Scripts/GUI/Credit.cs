using UnityEngine;
using System.Collections;

public class Credit : MonoBehaviour {
	
	public GameObject image;
	private float maxY = 70f;
	private Vector3 startPos;
	private bool creditRoll = false;

	
	// Use this for initialization
	void Start () 
	{
	startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 pos = image.transform.position;
		
		if (pos.y < maxY && creditRoll) 
		{
			pos.y += 5f * Time.deltaTime;
			image.transform.position = pos;
		} 
		else if (creditRoll && pos.y > maxY)
		{
			Application.LoadLevel(0);
			creditRoll = false;
		}
	}
	
	public void StartCredits ()
	{
		creditRoll = true;
	}
	
	
	public void resetCredits ()
	{
		transform.position = startPos;
	}
	
}
