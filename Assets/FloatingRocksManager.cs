using UnityEngine;
using System.Collections;

public class FloatingRocksManager : MonoBehaviour {

	[HideInInspector]
	public int FloatingRockNumber;
	[HideInInspector]
	public int readyToGoNumber;
	[HideInInspector]
	public bool releaseAll;

	// Use this for initialization
	void Start () 
	{
		FloatingRockNumber = transform.childCount;
		Debug.Log (FloatingRockNumber);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(readyToGoNumber == FloatingRockNumber)
		{
			releaseAll = true;
		}	
	}
	
	void IncrementReadyNumber()
	{
		readyToGoNumber++;
	}
}
