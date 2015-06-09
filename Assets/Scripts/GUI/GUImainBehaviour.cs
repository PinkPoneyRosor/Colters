using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUImainBehaviour : MonoBehaviour {

	//External scripts, objects, files,...

	GameObject soulBar;
	GameObject lifeBar;
	GameObject rockBar;
	
	GameObject player;

	PlayerController playerScript;
	
	[HideInInspector]
	public Image soulBarImage;

	public GameObject[] rockLights;
	public GameObject ShieldExplosive;
	public GameObject blueLifeBar;
	public GameObject soulSkull;
	
	Image explosivePic;
	Image lifeBarImage;
	Image blueLifeBarPic;
	Image blueSkull;

	float localDeltaTime;

	public float SoulBarSpeedRate = .1f;
	[HideInInspector]
	public float soulStartValue;
	[HideInInspector]
	public float lifeStartValue;
	[HideInInspector]
	public float rockPercent = 1;
	[HideInInspector]
	public bool NextRockExplosive = false;
	
	public float rockRefillRate = 0.9f;
	
	public bool paused = false;
	
	public GameObject FullyChargedRocksParticles;
	public GameObject FullyChargedSoulParticles;
	
	private float previousFrameRockpercent = 0;
	private float previousFrameSoulAmount = 0;

	// Use this for initialization
	void Start () 
	{
		soulBar = GameObject.Find ("SoulBar");
		lifeBar = GameObject.Find ("LifeBar");
		player = GameObject.Find ("Player");
		rockBar = GameObject.Find ("Shield");

		soulBarImage = soulBar.GetComponent<Image> ();
		lifeBarImage = lifeBar.GetComponent <Image> ();
		playerScript = player.GetComponent<PlayerController> ();
		explosivePic = ShieldExplosive.GetComponent<Image> ();
		blueLifeBarPic = blueLifeBar.GetComponent <Image> ();
		blueSkull = soulSkull.GetComponent <Image> ();
		
		soulStartValue = soulBarImage.fillAmount;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		
		if (Input.GetButtonDown ("PauseMenu") && Time.timeScale < .1f)
		{
			Time.timeScale = 1;
			Debug.Log ("Unpause");
			paused = false;
		}
		else if (Input.GetButtonDown ("PauseMenu") && Time.timeScale >= .1f)
		{
			Time.timeScale = 0.00001f;
			Debug.Log ("Paused");
			paused = true;
		}

		
		Debug.Log ("TimeScale = " + Time.timeScale);
			
	
		if(rockPercent > 1)
			rockPercent = 1;
	
		//localDeltaTime allows the script to not be influenced by the time scale change.
		localDeltaTime = (Time.timeScale == 0) ? 1 : Time.deltaTime / Time.timeScale;

		#region Soul Bar
 		if(playerScript.soulMode)
		{
			soulBarImage.fillAmount -= SoulBarSpeedRate * localDeltaTime;
		}
		else if (soulBarImage.fillAmount < 1)
		{
			soulBarImage.fillAmount += SoulBarSpeedRate * localDeltaTime;
		}
		
		if (soulBarImage.fillAmount == 1 && previousFrameSoulAmount < soulBarImage.fillAmount)
		{
			Instantiate (FullyChargedSoulParticles, player.transform.position, Quaternion.identity);
		}
		
		previousFrameSoulAmount = soulBarImage.fillAmount;
		#endregion
		
		#region Life Bar
		lifeBarImage.fillAmount = Mathf.MoveTowards (lifeBarImage.fillAmount, playerScript.currentHealth / playerScript.maxHealth, Time.deltaTime);
		blueLifeBarPic.fillAmount = Mathf.MoveTowards (lifeBarImage.fillAmount, playerScript.currentHealth / playerScript.maxHealth, Time.deltaTime);
		
		if(playerScript.currentHealth <= 0 && !playerScript.dead)
		{
			player.SendMessage ("Die");
		}
		#endregion
		
		#region RockBar
		if (rockPercent < 1 && player.GetComponent <CharacterController>().isGrounded)
			rockPercent += rockRefillRate * Time.deltaTime;
			
		if (rockPercent < 1)
			rockLights[0].GetComponent <Image> ().CrossFadeAlpha (0, .5f, true);
		else
			rockLights[0].GetComponent <Image> ().CrossFadeAlpha (1, .5f, true);
			
		if (rockPercent < .75f)
			rockLights[1].GetComponent <Image> ().CrossFadeAlpha (0, .5f, true);
		else
			rockLights[1].GetComponent <Image> ().CrossFadeAlpha (1, .5f, true);
			
		if (rockPercent < .5f)
			rockLights[2].GetComponent <Image> ().CrossFadeAlpha (0, .5f, true);
		else
			rockLights[2].GetComponent <Image> ().CrossFadeAlpha (1, .5f, true);
			
		if (rockPercent < .25f)
			rockLights[3].GetComponent <Image> ().CrossFadeAlpha (0, .5f, true);
		else
			rockLights[3].GetComponent <Image> ().CrossFadeAlpha (1, .5f, true);
			
		if (rockPercent == 1 && previousFrameRockpercent < rockPercent)
			Instantiate (FullyChargedRocksParticles, player.transform.position, Quaternion.identity);
			
		previousFrameRockpercent = rockPercent;
		
		if (NextRockExplosive)
		{
			explosivePic.CrossFadeAlpha (1 , .25f, true);
		}
		else
		{
			explosivePic.CrossFadeAlpha (0 , .25f, true);
		}
		#endregion
		
		#region soul mode
		if (playerScript.soulMode)
		{
			blueLifeBarPic.CrossFadeAlpha (1, .5f, true);
			blueSkull.CrossFadeAlpha (1, .5f, true);
		}
		else
		{
			blueLifeBarPic.CrossFadeAlpha (0, .5f, true);
			blueSkull.CrossFadeAlpha (0, .5f, true);
		}
		#endregion
	}

	void MaxRockBar ()
	{
		rockPercent = 1;
	}

}
