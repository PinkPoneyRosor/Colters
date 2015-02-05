using UnityEngine;
using System.Collections;

public class FallingBridge : MonoBehaviour {


	
	public float speed = 5;
	public bool falling = false;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider c){
		
		if(c.gameObject.tag == "Player")
		{
			falling = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(falling == true){
			transform.Translate(Vector3.down * speed * Time.deltaTime);
		}
	}
}
