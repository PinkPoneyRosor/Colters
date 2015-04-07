using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public int degats = 1;
	public float speed = 10;
	public GameObject explosionModelNormal;
	public GameObject explosionModelTracking; 
	public GameObject newExp;

	public Transform explosionParent;
	
	public enum projectileType { Normal = 0, Tracking = 1};
	public projectileType type = projectileType.Normal;
	
	public Transform target;

	void  Start ()
	{

		if (explosionParent == null)
		{
			GameObject root = GameObject.Find("Level/Explosions");
			
			if (root != null)
			{
				explosionParent = root.transform;
			}
		}
		
		switch (type)
		{
		case projectileType.Normal:

			StartCoroutine ("DoSomething");
			break;

			
		case projectileType.Tracking:
			
			StartCoroutine("DoSomethingElse");
			break;

			
		default:
			break;
		}
	}
	
	void  Update (){
		Vector3 direction;
		
		switch (type)
		{
		case projectileType.Normal:
			break;
			
		case projectileType.Tracking:
			if (target != null)
			{
				this.transform.LookAt (new Vector3 (target.position.x, target.position.y+0.5f, target.position.z) );
			}
			break;
			
		default:
			break;
		}
		direction = transform.forward;
		transform.position += direction * (speed * Time.deltaTime);
	}
	
	void  OnTriggerEnter ( Collider other  )
	{
		other.SendMessage("OnHit", degats, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
		switch (type)
		{
		case projectileType.Normal:
			
			//Destroy(gameObject);
			newExp = Instantiate(explosionModelNormal, transform.position, Quaternion.identity) as GameObject;
			//newExp.transform.parent = explosionParent;
			break;
			
		case projectileType.Tracking:
			
			//Destroy(gameObject);
			newExp = Instantiate(explosionModelTracking, transform.position, Quaternion.identity) as GameObject;
			//newExp.transform.parent = explosionParent;
			break;
			
		default:
			break;
		}
		newExp.transform.parent = explosionParent;
	}
	
	public void  SetTarget (  Transform newTarget  ){
		target = newTarget;
		transform.LookAt(new Vector3(target.position.x, target.position.y+0.5f, target.position.z));
	}


	IEnumerator DoSomething() {
		yield return new WaitForSeconds (3);
		Destroy(gameObject);
		newExp = Instantiate(explosionModelNormal, transform.position, Quaternion.identity) as GameObject;
		newExp.transform.parent = explosionParent;
	}

	IEnumerator DoSomethingElse() {
		yield return new WaitForSeconds (6);
		Destroy(gameObject);
		newExp = Instantiate(explosionModelTracking, transform.position, Quaternion.identity) as GameObject;
		newExp.transform.parent = explosionParent;
	}








}