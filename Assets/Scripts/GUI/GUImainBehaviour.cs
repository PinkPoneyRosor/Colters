using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImainBehaviour : MonoBehaviour {

	//External scripts, objects, files,...

	GameObject soulBar;
	GameObject lifeBarCircular;
	
	GameObject player;

	PlayerController playerScript;
	
	[HideInInspector]
	public Slider soulBarSlide;
	
	Image lifeBarImage;

	float localDeltaTime;

	public float SoulBarSpeedRate = .1f;
	[HideInInspector]
	public float soulStartValue;
	[HideInInspector]
	public float lifeStartValue;

	// Use this for initialization
	void Start () 
	{
		soulBar = GameObject.Find ("SoulBar");
		lifeBarCircular = GameObject.Find ("RadialLife");
		player = GameObject.Find ("Player");

		soulBarSlide = soulBar.GetComponent<Slider> ();
		lifeBarImage = lifeBarCircular.GetComponent <Image> ();
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
		lifeBarImage.fillAmount = Mathf.MoveTowards (lifeBarImage.fillAmount, playerScript.currentHealth / playerScript.maxHealth, Time.deltaTime);
		
		if(playerScript.currentHealth <= 0)
		{
			player.SendMessage ("Die");
		}
		#endregion
	}

}
