using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImainBehaviour : MonoBehaviour {

	//External scripts, objects, files,...

	GameObject soulBar;
	GameObject player;

	PlayerController playerScript;

	Slider soulBarSlide;

	float localDeltaTime;

	public float SoulBarSpeedRate = .1f;

	// Use this for initialization
	void Start () 
	{
		soulBar = GameObject.Find ("SoulBar");
		player = GameObject.Find ("Player");

		soulBarSlide = soulBar.GetComponent<Slider> ();
		playerScript = player.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		#region Soul Bar
 		if(playerScript.soulMode)
		{
			soulBarSlide.value -= SoulBarSpeedRate * localDeltaTime;
		}
		else if (soulBarSlide.value < 1)
		{
			soulBarSlide.value += SoulBarSpeedRate * localDeltaTime;
		}
		#endregion
	}

}
