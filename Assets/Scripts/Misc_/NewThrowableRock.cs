using UnityEngine;
using System.Collections;

public class NewThrowableRock : MonoBehaviour {

	public bool isSelected = false;
	public bool isGrowing = false;
	bool mustGetUp = true;
	public bool inTheAir = false;
	NewRockThrow rockThrowScript;
	GameObject player;
	[HideInInspector]
	public int selectionNumber = 0;
	
	private float changePosSpeed = 10;
	
	public Vector3 normalScale;
	
	[HideInInspector]
	public bool homingAttackBool = false;
	[HideInInspector]
	public Transform aimHoming;
	
	public float throwForce = 1000;
	
	public bool canExplode = false;
	public Vector3 posAtLaunch = Vector3.zero;
	public float maxTravelDistance = 15;
	public float growingRate = .5f;
	
	private bool growInit = true;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		rockThrowScript = player.GetComponent <NewRockThrow>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isGrowing)
		{
			isSelected = false;
			
			if(growInit)
			{
				this.transform.localScale = Vector3.zero;
				growInit = false;
			}
			
			transform.localScale = Vector3.MoveTowards (transform.localScale, normalScale/5, growingRate * Time.deltaTime);
			
			if ( Vector3.SqrMagnitude (this.transform.localScale - normalScale / 5) <= 0f)
			{
				Debug.Log ("DING!");
				isGrowing = false;
				isSelected = true;
			}
		}
		
		
		if(isSelected || isGrowing)
		{
			rigidbody.useGravity = false;
			collider.isTrigger = true;
			rigidbody.isKinematic = true;
			
			#region Get up
			if (mustGetUp && !isGrowing)
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
			else
			{
				mustGetUp = false;
				inTheAir = true;
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
				if(!isGrowing)
					transform.localScale = Vector3.Lerp (transform.localScale, normalScale/5, changePosSpeed * Time.deltaTime);
				
				transform.Rotate (Vector3.right * Time.deltaTime * 100);
				rigidbody.constraints = RigidbodyConstraints.FreezePosition;

				setSelectionPos();
			}
		}
		else if (!isGrowing)
		{
			transform.localScale = Vector3.Lerp (transform.localScale, normalScale, changePosSpeed * Time.deltaTime);
			mustGetUp = true;
		}
		
		#region track distance travelled
		if (Vector3.SqrMagnitude(transform.position - posAtLaunch) > maxTravelDistance * maxTravelDistance && !isSelected)
		{
			rigidbody.useGravity = true;
			constantForce.force = Vector3.zero;
			homingAttackBool = false;
		}
		#endregion
	}
	
	void setSelectionPos ()
	{
		switch (selectionNumber)
		{
		case 1:
			Vector3 firstOffset = rockThrowScript.firstOffset;
			transform.position = Vector3.Lerp (transform.position, player.transform.position + firstOffset, changePosSpeed * Time.deltaTime);
			break;
		case 2:
			Vector3 secondOffset = rockThrowScript.secondOffset;
			transform.position = Vector3.Lerp (transform.position, player.transform.position + secondOffset, changePosSpeed * Time.deltaTime);
			break;
		case 3:
			Vector3 thirdOffset = rockThrowScript.thirdOffset;
			transform.position = Vector3.Lerp (transform.position, player.transform.position + thirdOffset, changePosSpeed * Time.deltaTime);
			break;
		case 4:
			Vector3 fourthOffset = rockThrowScript.fourthOffset;
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
