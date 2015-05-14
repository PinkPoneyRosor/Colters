using UnityEngine;
using System.Collections;

public class Arrow_Normal : MonoBehaviour {

	public float mySpeed = 10;
	public float myRange = 10;
	public float damage = 1;
	
	private float myDist = 0;
	private bool hitSomething = false;
	private bool alreadyHit = false;
	
	[HideInInspector]
	public GameObject masterTurret;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		float distFromMaster = Vector3.SqrMagnitude(transform.position - masterTurret.transform.position);
		if (distFromMaster > 2 * 2)
		{
			collider.enabled = true;
		}
	
		if(!hitSomething)
		{
			transform.Translate (Vector3.forward * Time.deltaTime * mySpeed);
			myDist += Time.deltaTime * mySpeed;
			
			if (myDist >= myRange)
				Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter (Collider hit)
	{	
		if(!alreadyHit)
		{
			if (hit.CompareTag ("Player"))
			{
				hit.SendMessage("GetHurt", damage, SendMessageOptions.DontRequireReceiver);
				alreadyHit = true;
			}
			
			if(hit.gameObject != masterTurret)
			{
				hitSomething = true;
				transform.SetParent (hit.transform, true);
				alreadyHit = true;
				StartCoroutine (Disappear()) ;
			}
			else
			{
				hitSomething = false;
				alreadyHit = false;
			}
		}
	}
	
	IEnumerator Disappear ()
	{
		yield return new WaitForSeconds(2);
		Destroy (this.gameObject);
	}
}
