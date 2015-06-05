using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public float health;
	public GameObject ragdoll;


	public void TakeDamage (float dmg){
		health -= dmg;

		if (health <=0){
			Die();
		}
	}

	public void Die() {
		Ragdoll r =(Instantiate (ragdoll,transform.position,transform.rotation)as GameObject).GetComponent<Ragdoll>();
		r.CopyPose(transform);
		Debug.Log("Die Mofo !");
		Destroy(this.gameObject);
		
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
