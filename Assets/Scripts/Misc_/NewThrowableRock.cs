using UnityEngine;
using System.Collections;

public class NewThrowableRock : MonoBehaviour {

	public bool isSelected = false;
	bool mustGetUp = true;
	public bool inTheAir = false;
	NewRockThrow rockThrowScript;
	GameObject player;
	[HideInInspector]
	public int selectionNumber = 0;
	
	[SerializeField]
	float distanceFromPlayer = 2;
	
	public float changePosSpeed = 5;
	
	private Vector3 startScale;
	
	[HideInInspector]
	public bool homingAttackBool = false;
	[HideInInspector]
	public Transform aimHoming;
	
	public float throwForce = 1000;
	
	public bool canExplode = false;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		rockThrowScript = player.GetComponent <NewRockThrow>();
		startScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isSelected)
		{
			rigidbody.useGravity = false;
			collider.isTrigger = true;
			rigidbody.isKinematic = true;
			
			#region Get up
			if (mustGetUp)
			{
				transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.up * 4, Time.deltaTime * 1.5f);
				RaycastHit hit;
				if(Physics.Raycast (transform.position, -Vector3.up, out hit, Mathf.Infinity))
				{
					if(hit.distance >= 3)
					{
						mustGetUp = false;
						inTheAir = true;
					}
				}
			}
			#endregion
			
			#region set Number
			if (this.gameObject == rockThrowScript.firstSelected)
				selectionNumber = 1;
			else if (this.gameObject == rockThrowScript.secondSelected)
				selectionNumber = 2;
			else if (this.gameObject == rockThrowScript.thirdSelected)
				selectionNumber = 3;
			else if (this.gameObject == rockThrowScript.fourthSelected)
				selectionNumber = 4;
			#endregion
			
			if(inTheAir)
			{
				transform.localScale = Vector3.Lerp (transform.localScale, startScale/5, changePosSpeed * Time.deltaTime);
				
				if(!CommonControls.aimingMode)
					setSelectionPos();
				else
					setAimSelectionPos();
			}
		}
		else
		{
			transform.localScale = Vector3.Lerp (transform.localScale, startScale, changePosSpeed * Time.deltaTime);
			mustGetUp = true;
		}
	}
	
	void setSelectionPos ()
	{
		switch (selectionNumber)
		{
		case 1:
			Vector3 firstOffset = Quaternion.AngleAxis(90, player.transform.up) * (-player.transform.forward * 1.5f) + (player.transform.up * 1.6f);
			transform.position = Vector3.Lerp (transform.position, player.transform.position + firstOffset, changePosSpeed * Time.deltaTime);
			break;
		case 2:
			Vector3 secondOffset = Quaternion.AngleAxis(45, player.transform.up) * (-player.transform.forward * 1.5f) + (player.transform.up * 1.1f);
			transform.position = Vector3.Lerp (transform.position, player.transform.position + secondOffset, changePosSpeed * Time.deltaTime);
			break;
		case 3:
			Vector3 thirdOffset = Quaternion.AngleAxis(0, player.transform.up) * (-player.transform.forward * 1.5f) + (player.transform.up * .6f);
			transform.position = Vector3.Lerp (transform.position, player.transform.position + thirdOffset, changePosSpeed * Time.deltaTime);
			break;
		case 4:
			Vector3 fourthOffset = Quaternion.AngleAxis(-45, player.transform.up) * (-player.transform.forward * 1.5f) + (player.transform.up * .1f);
			transform.position = Vector3.Lerp (transform.position, player.transform.position + fourthOffset, changePosSpeed * Time.deltaTime);
			break;
		}
	}
	
	
	
	void setAimSelectionPos ()
	{
		
		switch (selectionNumber)
		{
		case 1:
			Vector3 firstOffset = Quaternion.AngleAxis(45, player.transform.up) * (player.transform.forward * distanceFromPlayer) + (player.transform.up * 1.2f);
			transform.position = Vector3.Lerp (transform.position, player.transform.position + firstOffset, changePosSpeed * Time.deltaTime);
			break;
		case 2:
			Vector3 secondOffset = Quaternion.AngleAxis(45, player.transform.up) * (player.transform.forward * distanceFromPlayer) + (player.transform.up * .7f);
			transform.position = Vector3.Lerp (transform.position, player.transform.position + secondOffset, changePosSpeed * Time.deltaTime);
			break;
		case 3:
			Vector3 thirdOffset = Quaternion.AngleAxis(45, player.transform.up) * (player.transform.forward * distanceFromPlayer) + (player.transform.up * .2f);
			transform.position = Vector3.Lerp (transform.position, player.transform.position + thirdOffset, changePosSpeed * Time.deltaTime);
			break;
		case 4:
			Vector3 fourthOffset = Quaternion.AngleAxis(45, player.transform.up) * (player.transform.forward * distanceFromPlayer) + (-player.transform.up * .3f);
			transform.position = Vector3.Lerp (transform.position, player.transform.position + fourthOffset, changePosSpeed * Time.deltaTime);
			break;
		}
	}
	
	//With this method, we make sure that if the player throwed the rock toward an enemy, he can be almost sure he will hit it.
	void homingAttack ()
	{
		if (aimHoming.GetComponent <BasicEnemy> ().canGetHit) 
		{
			Vector3 throwDir = aimHoming.position - this.transform.position;
			throwDir.Normalize ();
			
			isSelected = false;
			
			
			this.rigidbody.constraints = RigidbodyConstraints.None;
			
			constantForce.force = throwDir * throwForce;
		} 
		else
			homingAttackBool = false;
	}
	
	//To avoid the rock to be difficult to aim, we reactivate gravity only after the first hit, when it's not selected anymore.
	void OnCollisionEnter (Collision collider)
	{
		if (!isSelected) 
		{
			rigidbody.useGravity = true;
			constantForce.force = Vector3.zero;
			homingAttackBool = false;
		}
	}
	
}
