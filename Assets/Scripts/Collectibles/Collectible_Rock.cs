using UnityEngine;
using System.Collections;

public class Collectible_Rock : MonoBehaviour 
{
	private bool maxOutRockBar;
	private GameObject GameGUI;
	
	

	// Use this for initialization
	void Start () 
	{
		GameGUI = GameObject.Find ("GameHUD");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (maxOutRockBar)
		{
			GameGUI.SendMessage("MaxRockBar");
			Destroy (this.gameObject);
		}
	}
	
	void OnTriggerEnter (Collider hit)
	{
		if (hit.collider.CompareTag ("Player"))
		{
			maxOutRockBar = true;
		}
	}
}
