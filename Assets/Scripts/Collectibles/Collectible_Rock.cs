using UnityEngine;
using System.Collections;

public class Collectible_Rock : MonoBehaviour 
{

	GameObject player;
	NewRockThrow rockThrowScript;
	
	private bool maxOutRockScale;
	
	

	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find ("Player");
		rockThrowScript = player.GetComponent <NewRockThrow>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (maxOutRockScale)
		{
			foreach (GameObject rock in rockThrowScript.allSelectedRocks)
			{
				rock.SendMessage ("InstantGrow");
			}
			Destroy (this.gameObject);
		}
	}
	
	void OnTriggerEnter (Collider hit)
	{
		if (hit.collider.CompareTag ("Player"))
			maxOutRockScale = true;
	}
}
