using UnityEngine;
using System.Collections;

public class TutoTrigger : MonoBehaviour {

	public int triggerNumber;
	public TutosMessage parentScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider hit)
	{
		if (hit.collider.CompareTag ("Player") || hit.collider.CompareTag ("PlayerSoul"))
		{

			parentScript.SendMessage ("Clear");

			switch (triggerNumber)
			{
				case 1:
				parentScript.SendMessage ("Display", 1);
				break;
				case 2:
				parentScript.SendMessage ("Display", 2);
				break;
				case 3:
				parentScript.SendMessage ("Display", 3);
				break;
				case 4:
				parentScript.SendMessage ("Display", 4);
				break;
				case 5:
				parentScript.SendMessage ("Display", 5);
				break;
				case 6:
				parentScript.SendMessage ("Display", 6);
				break;
			}
		}
	}
}
