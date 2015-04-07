using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	public float range = 5;
	public GameObject projectileModel;
	public float FireRate = 0.2f;
	public Transform projectileSpawn; 
	public Transform projectileParent;

	public GameObject player;
	
	void  Start (){
		if (projectileParent == null)
		{
			GameObject root = GameObject.Find("Level/Projectiles");
			
			if (root != null)
			{
				projectileParent = root.transform;
			}
		}

		player = GameObject.Find("Player");
	}
	
	public float nextProjectile = 0;
	public int numProjectile = 0;
	
	void  Update ()
	{	
		float sqrRange = range*range;
		Vector3 diff;
		Vector3 phalenePosition;
		GameObject phalene = GameObject.FindGameObjectWithTag("Player") as GameObject;
		float nearDistance = 1000000;
		GameObject nearPhalene = null;


			phalenePosition = phalene.transform.position;
			phalenePosition.y = 0;
			diff = phalenePosition - transform.position;
			
			if (diff.sqrMagnitude < sqrRange)
			{
			Debug.Log ("Coucou1");
				float distanceBetweenPlayerAndTower = Vector3.SqrMagnitude (this.transform.position - player.transform.position);
				
				if (distanceBetweenPlayerAndTower < nearDistance)
				{
				Debug.Log ("Coucou3");
					nearDistance = distanceBetweenPlayerAndTower;
					nearPhalene = phalene;
				}
			}
		if (nearPhalene != null)
		{
			Debug.Log ("Coucou2");
			phalenePosition = nearPhalene.transform.position;
			phalenePosition.y = transform.position.y;
			transform.LookAt(phalenePosition);
			
			if (Time.time >= nextProjectile)
			{
				GameObject newProj = Instantiate(projectileModel, projectileSpawn.position, Quaternion.identity) as GameObject;
				
				newProj.name = "Projectile_" + gameObject.name + "_" + numProjectile++;
				newProj.transform.parent = projectileParent;
				newProj.GetComponent<Projectile>().SetTarget(nearPhalene.transform);
				nextProjectile = Time.time + FireRate;
			}
			
		}
	}
	
	void  OnDrawGizmosSelected (){
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}