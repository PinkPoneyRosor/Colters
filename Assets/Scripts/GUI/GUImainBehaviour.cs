using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImainBehaviour : MonoBehaviour {

	//External scripts, objects, files,...

	GameObject soulBar;
	GameObject lifeBar;
	
	GameObject player;

	PlayerController playerScript;
	
	[HideInInspector]
	public Slider soulBarSlide;
	
	Slider lifeBarSlide;

	float localDeltaTime;

	public float SoulBarSpeedRate = .1f;
	[HideInInspector]
	public float soulStartValue;

	// Use this for initialization
	void Start () 
	{
		soulBar = GameObject.Find ("SoulBar");
		lifeBar = GameObject.Find ("LifeBar");
		player = GameObject.Find ("Player");

		soulBarSlide = soulBar.GetComponent<Slider> ();
		lifeBarSlide = lifeBar.GetComponent<Slider> ();
		playerScript = player.GetComponent<PlayerController> ();
		
		soulStartValue = soulBarSlide.value;
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
		
		#region Life Bar
		lifeBarSlide.maxValue = playerScript.maxHealth;
		lifeBarSlide.value = playerScript.currentHealth;
		#endregion
	}

}
