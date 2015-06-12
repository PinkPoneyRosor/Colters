using UnityEngine;
using System.Collections;

public class SoulModeHighlightYellow : MonoBehaviour {
	
	PlayerController playerScript;
	GameObject player;
	public Material highlightMaterial;
	
	Component[] renderers;
	
	float highlightWidth;
	float highlightIntens;
	float localDeltaTime;
	
	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find ("Player");
		playerScript = player.GetComponent <PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;
		
		highlightWidth = highlightMaterial.GetFloat ("_Highlight_Width");
		highlightIntens = highlightMaterial.GetFloat ("_Highlight_Intensity");
		
		if (playerScript.soulMode)
		{
			highlightMaterial.SetFloat ("_Highlight_Width", Mathf.MoveTowards (highlightWidth, 3, localDeltaTime * .1f));
			highlightMaterial.SetFloat ("_Highlight_Intensity", Mathf.MoveTowards (highlightIntens, .4f, localDeltaTime * .1f));
		}
		else
		{
			highlightMaterial.SetFloat ("_Highlight_Width", Mathf.MoveTowards (highlightWidth, 0, localDeltaTime * .1f));
			highlightMaterial.SetFloat ("_Highlight_Intensity", Mathf.MoveTowards (highlightIntens, 1, localDeltaTime * .1f));
		}
	}
}
