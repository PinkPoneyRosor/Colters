using UnityEngine;
using System.Collections;

public class ThrowableRock : MonoBehaviour {

	#region inspector var
	public bool isSelected = false;
	public bool getUpInit = false;
	public bool nowThrowable = false;
	public bool gettingUp = false;
	public bool canExplode = false;
	public float maxSpeed = 50;
	public float getUpSpeed = 5;
	public float getUpRotateForce = 100;
	public float throwForce = 1000;
	public float changePosSpeed = 5;
	
	#endregion
	
	private Vector3 startScale;

	[HideInInspector]
	public bool homingAttackBool = false;
	[HideInInspector]
	public Transform aimHoming;
	[HideInInspector]
	public int selectionNumber = 0;
	
	[SerializeField]
	float distanceFromPlayer = 2;

	Vector3 previousPosition = Vector3.zero;
	
	GameObject player;
	RockThrow throwScript;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		throwScript = player.GetComponent < RockThrow >();
		startScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {

		//This boolean must ALWAYS be verified BEFORE the isSelected one.
		if (getUpInit) 
		{
			getUpInit = false;
			rigidbody.useGravity = false;
			previousPosition = transform.position;
			gettingUp = true;
		}

		if (gettingUp) 
		{
			transform.position = Vector3.Lerp (transform.position, transform.position + Vector3.up * 4, Time.deltaTime * 1.5f);

			RaycastHit hit;
			if(Physics.Raycast (transform.position, -Vector3.up, out hit, Mathf.Infinity))
			{
				if(hit.distance >= 3)
				{
					gettingUp = false;
					isSelected = true;
				}
			}
		}

		if (isSelected) //If the rock's in the air, it's now selected, and we nom make sur it won't move.
		{
			canExplode = true;
			gettingUp = false;
			transform.Rotate (Vector3.right * Time.deltaTime * 100);
			rigidbody.constraints = RigidbodyConstraints.FreezePosition;
			
			if(!CommonControls.aimingMode)
			setSelectionPos();
			else
			setAimSelectionPos();
			
			transform.localScale = Vector3.Lerp (transform.localScale, startScale/5, changePosSpeed * Time.deltaTime);
			
			Vector3 fromRockToPlayer = transform.position - player.transform.position;
			float distanceFromRockToPlayer = fromRockToPlayer.sqrMagnitude;
			
			if (distanceFromRockToPlayer < 8)
			{
				Debug.Log ("First rock ready to launch");
				nowThrowable = true;
			}
		} 
		else 
		{
			rigidbody.constraints = RigidbodyConstraints.None; //Else, we make sure there is no more movement constraints.
			transform.localScale = Vector3.Lerp (transform.localScale, startScale, changePosSpeed * Time.deltaTime);
		}

		if (homingAttackBool) //This variable is activated by the RockThrow script if it detects that the player is aiming at an enemy.
				homingAttack ();

	}

	void FixedUpdate ()
	{
		if(rigidbody.velocity.sqrMagnitude > maxSpeed * maxSpeed) //This is to ensure the rock won't be too fast when accelerating.
		{
			rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
		}
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

	//With this method, we make sure that if the player throwed the rock toward an enemy, he can be almost sure he will hit it.
	void homingAttack ()
	{
		if (aimHoming.GetComponent<BasicEnemy> ().canGetHit) 
		{
			Vector3 throwDir = aimHoming.position - this.transform.position;
			throwDir.Normalize ();

			isSelected = false;
			nowThrowable = false;
		

			this.rigidbody.constraints = RigidbodyConstraints.None;

			constantForce.force = throwDir * throwForce;
		} 
		else
			homingAttackBool = false;
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
}
