using UnityEngine;
using System.Collections;

public class SoulModeHighlight : MonoBehaviour {

	PlayerController playerScript;
	GameObject player;
	public Material enemyMaterial;
	
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
	
		highlightWidth = enemyMaterial.GetFloat ("_Highlight_Width");
		highlightIntens = enemyMaterial.GetFloat ("_Highlight_Intensity");
	
		if (playerScript.soulMode)
		{
			enemyMaterial.SetFloat ("_Highlight_Width", Mathf.MoveTowards (highlightWidth, 3, localDeltaTime * .1f));
			enemyMaterial.SetFloat ("_Highlight_Intensity", Mathf.MoveTowards (highlightIntens, .4f, localDeltaTime * .1f));
		}
		else
		{
			enemyMaterial.SetFloat ("_Highlight_Width", Mathf.MoveTowards (highlightWidth, 0, localDeltaTime * .1f));
			enemyMaterial.SetFloat ("_Highlight_Intensity", Mathf.MoveTowards (highlightIntens, 1, localDeltaTime * .1f));
		}
	}
}