using UnityEngine;
using System.Collections;

public class Projectile_Bandit : MonoBehaviour {

	public float mySpeed = 10;
	public float myRange = 10;
	public float damage = 1;
	public GameObject player;
	public Transform target;
	private float myDist;



	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate (Vector3.forward * Time.deltaTime * mySpeed);
		myDist += Time.deltaTime * mySpeed;

		if(myDist>= myRange)
			Destroy(gameObject);
	}
	
	void OnTriggerEnter (Collider hit)
	{
		if (hit.CompareTag ("Player"))
			hit.SendMessage("GetHurt", damage, SendMessageOptions.DontRequireReceiver);
	}
	
	
}
