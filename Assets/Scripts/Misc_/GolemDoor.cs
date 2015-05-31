using UnityEngine;
using System.Collections;

public class GolemDoor : MonoBehaviour {


	public GameObject golemDead;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void	OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "ThrowableRock2") 
		{
			Instantiate (golemDead,transform.position,transform.rotation);
			Destroy(this.gameObject);
		}
	}
}
