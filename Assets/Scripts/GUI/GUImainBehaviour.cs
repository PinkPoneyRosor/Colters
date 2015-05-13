using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImainBehaviour : MonoBehaviour {

	//External scripts, objects, files,...

	GameObject soulBar;
	GameObject lifeBarCircular;
	GameObject rockBar;
	
	GameObject player;

	PlayerController playerScript;
	
	[HideInInspector]
	public Slider soulBarSlide;
	[HideInInspector]
	public Slider rockBarSlide;
	
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
		rockBar = GameObject.Find ("RockBar");

		soulBarSlide = soulBar.GetComponent<Slider> ();
		lifeBarImage = lifeBarCircular.GetComponent <Image> ();
		playerScript = player.GetComponent<PlayerController> ();
		
		soulStartValue = soulBarSlide.value;
		rockBarSlide = rockBar.GetComponent <Slider> ();
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
		
		#region RockBar
		if (rockBarSlide.value < 1)
			rockBarSlide.value += 0.1f * Time.deltaTime;
			
		#endregion
	}

}
