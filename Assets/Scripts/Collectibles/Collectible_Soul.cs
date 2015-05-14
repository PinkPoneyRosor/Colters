using UnityEngine;
using System.Collections;

public class Collectible_Soul : MonoBehaviour {

	GUImainBehaviour GUIScript;
	GameObject mainCanvas;

	// Use this for initialization
	void Start () 
	{
		mainCanvas = GameObject.Find ("GameHUD");
		GUIScript = mainCanvas.GetComponent <GUImainBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter (Collider hit)
	{
		if (hit.collider.CompareTag ("Player"))
		{
			GUIScript.soulBarSlide.value = GUIScript.soulStartValue;
			Destroy (this.gameObject);
		}
	}
}
